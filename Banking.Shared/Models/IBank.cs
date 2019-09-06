using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBank : IIdentityModel
	{
		string Name { get; set; }
	}
}
