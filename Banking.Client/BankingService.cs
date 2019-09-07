using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Banking.Client.Models;
using IgiCore.Banking.Client.Overlays;
using IgiCore.Banking.Shared;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Client.Input;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Rpc;

namespace IgiCore.Banking.Client
{
	[PublicAPI]
	public class BankingService : Service
	{
		private Configuration config;
		private BankingOverlay overlay;

		private List<BankATM> atms = new List<BankATM>();
		private List<BankBranch> branches = new List<BankBranch>();
		private Dictionary<BankBranch, Ped> tellers = new Dictionary<BankBranch, Ped>();
		private bool InAnim { get; set; }
		private Camera Camera { get; set; }

		public BankingService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, ICommandManager commands, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, commands, overlay, user) { }

		public override async Task Started()
		{
			// Request server configuration
			this.config = await this.Rpc.Event(BankingEvents.Configuration).Request<Configuration>();

			// Update local configuration on server configuration change
			this.Rpc.Event(BankingEvents.Configuration).On<Configuration>((e, c) => this.config = c);

			this.atms = await this.Rpc.Event(BankingEvents.GetATMs).Request<List<BankATM>>();
			this.branches = await this.Rpc.Event(BankingEvents.GetBranches).Request<List<BankBranch>>();

			// Create overlay
			this.overlay = new BankingOverlay(this.OverlayManager);

			this.Logger.Debug($"Branch: {this.branches.First().Name}  |  Position: {this.branches.First().Position}");

			// Attach a tick handlers
			this.Ticks.Attach(ATMTick);
			this.Ticks.Attach(BranchTick);
		}

		private async Task BranchTick()
		{
			foreach (var bankBranch in this.branches)
			{
				var bankBranchPos = new Vector3(bankBranch.Position.X, bankBranch.Position.Y, bankBranch.Position.Z);
				if (this.tellers.ContainsKey(bankBranch) && this.tellers[bankBranch].Handle != 0)
				{
					this.tellers[bankBranch].Position = bankBranchPos;
					this.tellers[bankBranch].Heading = bankBranch.Heading;
					continue;
				}
				var tellerModel = new Model(PedHash.Bankman);
				await tellerModel.Request(-1);
				this.tellers[bankBranch] = await World.CreatePed(tellerModel, bankBranchPos, bankBranch.Heading);
				this.tellers[bankBranch].Task?.ClearAllImmediately();
				this.tellers[bankBranch].Task?.StandStill(1);
				this.tellers[bankBranch].AlwaysKeepTask = true;
				this.tellers[bankBranch].IsInvincible = true;
				this.tellers[bankBranch].IsPositionFrozen = true;
				this.tellers[bankBranch].BlockPermanentEvents = true;
				this.tellers[bankBranch].IsCollisionProof = false;
			}

			if (this.InAnim && Input.IsControlJustPressed(Control.MoveUpOnly))
			{
				Game.Player.Character.Task.ClearAll();
				Game.Player.Character.Task.ClearLookAt();
				World.DestroyAllCameras();
				this.Camera = null;
				World.RenderingCamera = null;
				this.InAnim = false;
			}

			if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

			var teller = this.tellers
				.Select(t => new { teller = t, distance = t.Value?.Position.DistanceToSquared(Game.Player.Character.Position) ?? float.MaxValue })
				.Where(t => t.distance < 5.0F) // Nearby
											   //.Where(a => Vector3.Dot(a.Item2.ForwardVector, Vector3.Normalize(a.Item2.Position - Game.Player.ActiveCharacter.Position)).IsBetween(0f, 1.0f)) // In front of
				.OrderBy(t => t.distance)
				.Select(t => t.teller)
				.FirstOrDefault();

			if (teller.Value == null) return;

			new Text($"Press Z to use Branch {teller.Key.Name}", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!Input.IsControlJustPressed(Control.MultiplayerInfo)) return;

			this.InAnim = true;

			var bankTeller = teller.Value;

			var ts = new TaskSequence();
			ts.AddTask.LookAt(bankTeller);
			var moveToPos = bankTeller.GetPositionInFront(1.5f);
			ts.AddTask.GoTo(new Vector3(moveToPos.X, moveToPos.Y, moveToPos.Z));
			ts.AddTask.AchieveHeading(bankTeller.Heading - 180);
			ts.Close();
			await Game.Player.Character.RunTaskSequence(ts);
			Game.Player.Character.Task.LookAt(bankTeller);
			Game.Player.Character.Task.StandStill(-1);

			// camera
			if (this.Camera == null) this.Camera = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
			World.RenderingCamera = this.Camera;

			for (float t = 0; t < 1f; t += 0.01f)
			{
				var interval = (float)(t < 0.5 ? 2.0 * t * t : -2.0 * t * t + 4.0 * t - 1.0);
				var tellerFrontPos = teller.Value.Position.ToVector3().TranslateDir(bankTeller.Heading + 110, 2.2f);
				var cameraPos = VectorExtensions.Lerp(
					GameplayCamera.Position.ToVector3(),
					(new Vector3(tellerFrontPos.X, tellerFrontPos.Y, tellerFrontPos.Z) + (Vector3.UnitZ * 0.8f))
					.ToVector3(),
					interval
				);
				this.Camera.Position = new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z);
				var cameraFocus = VectorExtensions.Lerp(Game.PlayerPed.Position.ToVector3(),
					bankTeller.Position.ToVector3(), interval);
				this.Camera.PointAt( new Vector3(cameraFocus.X, cameraFocus.Y, cameraFocus.Z) + Vector3.UnitZ * 0.4f);
				this.Camera.FieldOfView = MathHelpers.Lerp(GameplayCamera.FieldOfView, 30, interval);
				await Delay(TimeSpan.FromMilliseconds(1));
			}
		}

		private async Task ATMTick()
		{
			if (this.InAnim && Input.IsControlJustPressed(Control.MoveUpOnly))
			{
				Game.Player.Character.Task.ClearAllImmediately(); // Cancel animation
				this.InAnim = false;
			}
			if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

			var atm = this.atms
				.Select(a => new { atm = a, distance = new Vector3(a.Position.X, a.Position.Y, a.Position.Z).DistanceToSquared(Game.Player.Character.Position) })
				.Where(a => a.distance < 5.0F) // Nearby
				.Select(a => new { a.atm, prop = new Prop(API.GetClosestObjectOfType(a.atm.Position.X, a.atm.Position.Y, a.atm.Position.Z, 1, (uint)a.atm.Hash, false, false, false)), a.distance })
				.Where(p => p.prop.Model.IsValid)
				.Where(a => Vector3.Dot(a.prop.ForwardVector, Vector3.Normalize(a.prop.Position - Game.Player.Character.Position)).IsBetween(0f, 1.0f)) // In front of
				.OrderBy(a => a.distance)
				.Select(a => new Tuple<BankATM, Prop>(a.atm, a.prop))
				.FirstOrDefault();

			if (atm == null) return;

			new Text("Press Z to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!Input.IsControlJustPressed(Control.MultiplayerInfo)) return;

			var ts = new TaskSequence();
			ts.AddTask.LookAt(atm.Item2);
			ts.AddTask.GoTo(atm.Item2, Vector3.Zero, 2000);
			ts.AddTask.AchieveHeading(atm.Item2.Heading);
			ts.AddTask.ClearLookAt();
			ts.Close();
			await Game.Player.Character.RunTaskSequence(ts);

			API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
			API.ResetScenarioTypesEnabled();
			API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);
			this.InAnim = true;

			await Delay(TimeSpan.FromSeconds(1));
		}

		private async Task OnTick()
		{
			await Delay(TimeSpan.FromSeconds(1));
		}
	}
}
