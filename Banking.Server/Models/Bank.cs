using System;
using System.Collections.Generic;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server.Models
{
	public class Bank : IdentityModel, IBank
	{
		public string Name { get; set; }

		public virtual List<BankAccount> Accounts { get; set; }
		public virtual List<BankATM> ATMs { get; set; }
		public virtual List<BankBranch> Branches { get; set; }
	}
}
