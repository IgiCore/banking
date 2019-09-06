using IgiCore.Banking.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Client.Models
{
	public class BankBranch : IdentityModel, IBankBranch
	{
		public string Name { get; set; }
        public Position Position { get; set; }
        public float Heading { get; set; }
	}
}
