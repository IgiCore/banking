using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using System;
using System.Collections.Generic;
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
using NFive.SDK.Client.Interface;
using NFive.SDK.Core.Extensions;
using Vector3 = CitizenFX.Core.Vector3;
using System.Drawing;
using NFive.SDK.Core.Input;
using Color = System.Drawing.Color;
using Font = CitizenFX.Core.UI.Font;

namespace IgiCore.Banking.Client
{
	[PublicAPI]
	public class ATMService : Service
	{
		private List<BankATM> atms;
		private bool InAnim { get; set; }

		private Camera Camera { get; set; }

		private readonly Hotkey animCancelKey = new Hotkey(InputControl.MoveUp);

		private readonly Hotkey interactKey = new Hotkey(InputControl.MultiplayerInfo);

		public ATMService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			this.atms = await this.Comms.Event(BankingEvents.GetATMs).ToServer().Request<List<BankATM>>();

			// Attach a tick handlers
			this.Ticks.On(ATMTick);
			this.Ticks.On(AnimHandler);
		}

		private void AnimHandler()
		{
			if (!this.InAnim || !this.animCancelKey.IsJustPressed()) return;
			Game.Player.Character.Task.ClearAllImmediately();
			Game.Player.Character.Task.ClearLookAt();
			World.DestroyAllCameras();
			this.Camera = World.RenderingCamera = null;
			this.InAnim = false;
		}

		private async Task ATMTick()
		{
			if (this.InAnim || Game.Player.Character.IsInVehicle()) return;

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

			if (!this.interactKey.IsJustPressed()) return;

			var atmModel = atm.Item2;

			var ts = new TaskSequence();
			ts.AddTask.LookAt(atmModel);
			var moveToPos = atmModel.Position.ToVector3().InFrontOf(atmModel.Heading + 180f, 0.5f);
			ts.AddTask.SlideTo(moveToPos.ToCitVector3(), atmModel.Heading);
			ts.AddTask.ClearLookAt();
			ts.Close();
			await Game.Player.Character.RunTaskSequence(ts);

			API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
			API.ResetScenarioTypesEnabled();
			API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);
			this.InAnim = true;

			// Camera
			var atmCameraPos = atmModel.Position.ToVector3().ToPosition().TranslateDir(atmModel.Heading - 50f, 1.5f);
			this.Camera = World.CreateCamera(
				new Vector3(atmCameraPos.X, atmCameraPos.Y, atmCameraPos.Z) + (Vector3.UnitZ * 1.8f),
				GameplayCamera.Rotation,
				50
			);
			this.Camera.PointAt(new Vector3(atmModel.Position.X, atmModel.Position.Y, atmModel.Position.Z) + Vector3.UnitZ * 1f);
			World.RenderingCamera.InterpTo(this.Camera, 1000, true, true);
			API.RenderScriptCams(true, true, 1000, true, false);
		}
	}
}
