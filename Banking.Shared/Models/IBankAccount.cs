using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBankAccount : IIdentityModel
	{
		string Name { get; set; }
		int AccountNumber { get; set; }
		BankAccountTypes Type { get; set; }
		double Balance { get; set; }
		bool Locked { get; set; }
	}
}
