using IgiCore.Characters.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Shared.Models
{
	public interface IBankAccountMember : IIdentityModel
	{
		ICharacter Member { get; set; }
	}
}
