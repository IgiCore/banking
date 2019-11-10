using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Banking.Server.Models;
using IgiCore.Banking.Server.Storage;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using IgiCore.Banking.Shared;
using NFive.SDK.Core.Models;
using NFive.SDK.Server.Communications;

namespace IgiCore.Banking.Server
{
	[PublicAPI]
	public class BankingController : ConfigurableController<Configuration>
	{
		private readonly ICommunicationManager comms;

		public BankingController(ILogger logger, Configuration configuration, ICommunicationManager comms) : base(logger, configuration)
		{
			this.comms = comms;

			// Send configuration when requested
			this.comms.Event(BankingEvents.Configuration).FromClients().OnRequest(e => e.Reply(this.Configuration));
			this.comms.Event(BankingEvents.GetATMs).FromClients().OnRequest(e => e.Reply(this.GetATMs()));
			this.comms.Event(BankingEvents.GetBranches).FromClients().OnRequest(e => e.Reply(this.GetBranches()));
		}

		private List<BankBranch> GetBranches()
		{
			using (var context = new StorageContext())
			{
				return context.BankBranches.ToList();
			}
		}

		private List<BankATM> GetATMs()
		{
			using (var context = new StorageContext())
			{
				return context.BankATMs.ToList();
			}
		}

		public override Task Started()
		{
			using (var context = new StorageContext())
			{
				context.Banks.AddOrUpdate(
				new Bank
				{
					Id = new Guid("39e662f1-4dfe-96b3-ae2f-e36da99c1e80"),
					Name = "Fleeca",
					Branches = new List<BankBranch>
					{
						new BankBranch
						{
							Name = "Legion Square",
							Position = new Position(149.7f, -1042.2f, 28.33f),
							Heading = 336.00f
						},
						new BankBranch
						{
							Name = "Legion Square 02",
							Position = new Position(163.7f, -1004.2f, 28.35f),
							Heading = 280.00f
						}
					},
					ATMs = new List<BankATM>
					{
						new BankATM
						{
							Name = "FLCA LSS 0001",
							Hash = 506770882,
							Position = new Position(147.4731f, -1036.218f, 28.36778f)
						},
						new BankATM
						{
							Name = "FLCA LSS 0002",
							Hash = 506770882,
							Position = new Position(145.8392f, -1035.625f, 28.36778f)
						},
						new BankATM
						{
							Name = "Standup ATM 0001",
							Hash = -870868698,
							Position = new Position(228.0324f, 337.8501f, 104.5013f)
						},

					},
					Accounts = new List<BankAccount>()
				});

				context.SaveChanges();
			}

			return base.Started();
		}
	}
}
