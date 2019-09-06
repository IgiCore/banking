using System.Data.Entity;
using IgiCore.Banking.Server.Models;
using NFive.SDK.Server.Storage;

namespace IgiCore.Banking.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<Bank> Banks { get; set; }
		public DbSet<BankAccount> BankAccounts { get; set; }
		public DbSet<BankAccountCard> BankAccountCards { get; set; }
		public DbSet<BankAccountMember> BankAccountMembers { get; set; }
		public DbSet<BankATM> BankATMs { get; set; }
		public DbSet<BankBranch> BankBranches { get; set; }
		public DbSet<BankCard> BankCards { get; set; }
	}
}
