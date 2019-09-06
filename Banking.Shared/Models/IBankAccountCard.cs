using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBankAccountCard : IIdentityModel
	{
		int Pin { get; set; }
		int Number { get; set; }
		string Name { get; set; }
		IBankCard Card { get; set; }
	}
}
