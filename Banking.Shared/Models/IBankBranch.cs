using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBankBranch : IIdentityModel
	{
		string Name { get; set; }
		Position Position { get; set; }
		float Heading { get; set; }
	}
}
