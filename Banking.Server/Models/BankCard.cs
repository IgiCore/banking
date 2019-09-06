using System;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server.Models
{
	public class BankCard : IdentityModel, IBankCard
	{
		public Guid ItemDefinitionId { get; set; }
		public Guid? ContainerId { get; set; }
		public float Weight { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int? X { get; set; }
		public int? Y { get; set; }
		public bool Rotated { get; set; }
		public int UsesRemaining { get; set; }
	}
}
