using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Client.Models
{
	public class BankAccountCard : IdentityModel, IBankAccountCard
	{
		public int Pin { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public IBankCard Card { get; set; }
	}
}
