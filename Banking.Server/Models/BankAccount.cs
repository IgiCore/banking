using System;
using IgiCore.Banking.Shared;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server.Models
{
	public class BankAccount : IdentityModel, IBankAccount
	{
		public string Name { get; set; }
		public int AccountNumber { get; set; }
		public BankAccountTypes Type { get; set; }
		public double Balance { get; set; }
		public bool Locked { get; set; }
	}
}
