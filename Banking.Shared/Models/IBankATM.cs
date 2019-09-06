using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBankATM : IIdentityModel
	{
		string Name { get; set; }
		long Hash { get; set; }
		Position Position { get; set; }
	}
}
