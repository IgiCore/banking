using System;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Models;
using Vector3 = CitizenFX.Core.Vector3;

namespace IgiCore.Banking.Client.Extensions
{
	public static class PositionExtensions
	{
		public static Vector3 ToCitVector3(this Position pos) => new Vector3(pos.X, pos.Y, pos.Z);

		public static Vector3 ToCitVector3(this NFive.SDK.Core.Models.Vector3 pos) => new Vector3(pos.X, pos.Y, pos.Z);

		public static Position InFrontOf(this Position pos, float heading, float distance) => pos.TranslateDir(heading + 90f, distance);
	}
}
