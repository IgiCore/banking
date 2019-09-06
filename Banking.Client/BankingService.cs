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

			// Attach a tick handler
			this.Ticks.Attach(OnTick);

			this.Ticks.Attach(ATMTick);
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

			new Text("Press M to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!Input.IsControlJustPressed(Control.InteractionMenu)) return;

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

		public bool InAnim { get; set; }

		private async Task OnTick()
		{
			await Delay(TimeSpan.FromSeconds(1));
		}
	}
}
