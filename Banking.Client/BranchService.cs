using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Banking.Client.Models;
using IgiCore.Banking.Shared;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Client.Input;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Input;
using Font = CitizenFX.Core.UI.Font;

namespace IgiCore.Banking.Client
{
	[PublicAPI]
	public class BranchService : Service
	{
		private List<BankBranch> branches = new List<BankBranch>();
		private readonly Dictionary<BankBranch, Ped> tellers = new Dictionary<BankBranch, Ped>();
		private bool InAnim { get; set; }
		private Camera Camera { get; set; }

		private readonly Hotkey animCancelKey = new Hotkey(InputControl.MoveUp);

		private readonly Hotkey interactKey = new Hotkey(InputControl.MultiplayerInfo);

		public BranchService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			this.branches = await this.Comms.Event(BankingEvents.GetBranches).ToServer().Request<List<BankBranch>>();

			// Attach a tick handlers
			this.Ticks.On(BranchTick);
			this.Ticks.On(BranchInteractionTick);
		}

		private void BranchInteractionTick()
		{
			if (!this.InAnim || !this.animCancelKey.IsJustPressed()) return;
			Game.Player.Character.Task.ClearAllImmediately();
			Game.Player.Character.Task.ClearLookAt();
			World.DestroyAllCameras();
			this.Camera = World.RenderingCamera = null;
			this.InAnim = false;
		}

		private async Task BranchTick()
		{
			// Ped spawning/tracking
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

			if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

			// Get nearest ped
			var teller = this.tellers
				.Select(t => new { teller = t, distance = t.Value?.Position.DistanceToSquared(Game.Player.Character.Position) ?? float.MaxValue })
				.Where(t => t.distance < 5.0F) // Nearby
											   //.Where(a => Vector3.Dot(a.Item2.ForwardVector, Vector3.Normalize(a.Item2.Position - Game.Player.ActiveCharacter.Position)).IsBetween(0f, 1.0f)) // In front of
				.OrderBy(t => t.distance)
				.Select(t => t.teller)
				.FirstOrDefault();

			if (teller.Value == null) return;

			// Add UI text
			new Text($"Press Z to use Branch {teller.Key.Name}", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!this.interactKey.IsJustPressed()) return;

			// Initiate ped interaction
			this.InAnim = true;
			var bankTeller = teller.Value;
			var ts = new TaskSequence();
			ts.AddTask.LookAt(bankTeller);
			var moveToPos = bankTeller.GetPositionInFront(1.5f);
			ts.AddTask.SlideTo(new Vector3(moveToPos.X, moveToPos.Y, moveToPos.Z), bankTeller.Heading - 180);
			ts.AddTask.AchieveHeading(bankTeller.Heading - 180);
			ts.Close();
			await Game.Player.Character.RunTaskSequence(ts);
			Game.Player.Character.Task.LookAt(bankTeller);
			Game.Player.Character.Task.StandStill(-1);

			// Camera
			var tellerFrontPos = teller.Value.Position.ToVector3().TranslateDir(bankTeller.Heading + 110, 2.2f);
			this.Camera = World.CreateCamera(
				new Vector3(tellerFrontPos.X, tellerFrontPos.Y, tellerFrontPos.Z) + (Vector3.UnitZ * 0.8f),
				GameplayCamera.Rotation,
				30
			);
			this.Camera.PointAt(new Vector3(bankTeller.Position.X, bankTeller.Position.Y, bankTeller.Position.Z) + Vector3.UnitZ * 0.4f);
			World.RenderingCamera.InterpTo(this.Camera, 1000, true, true);
			API.RenderScriptCams(true, true, 1000, true, false);
		}
	}
}
