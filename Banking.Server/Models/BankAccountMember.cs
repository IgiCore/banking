using System;
using IgiCore.Banking.Shared.Models;
using IgiCore.Characters.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server.Models
{
	public class BankAccountMember : IdentityModel, IBankAccountMember
	{
		public ICharacter Member { get; set; }
	}
}
