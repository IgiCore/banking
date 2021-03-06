using System;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server.Models
{
	public class BankATM : IdentityModel, IBankATM
	{
		public string Name { get; set; }
		public long Hash { get; set; }
		public Position Position { get; set; }
	}
}
