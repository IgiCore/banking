using System.Collections.Generic;
using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Client.Models
{
	public class Bank : IdentityModel, IBank
	{
		public string Name { get; set; }

		public List<BankAccount> Accounts { get; set; }
		public List<BankATM> ATMs { get; set; }
		public List<BankBranch> Branches { get; set; }
	}
}
