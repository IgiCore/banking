using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using IgiCore.Banking.Server.Models;
using IgiCore.Banking.Server.Storage;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;
using IgiCore.Banking.Shared;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Models;

namespace IgiCore.Banking.Server
{
	[PublicAPI]
	public class BankingController : ConfigurableController<Configuration>
	{
		public BankingController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration)
		{
			// Send configuration when requested
			this.Rpc.Event(BankingEvents.Configuration).On(e => e.Reply(this.Configuration));
		}

		public override void Reload(Configuration configuration)
		{
			// Update local configuration
			base.Reload(configuration);

			// Send out new configuration
			this.Rpc.Event(BankingEvents.Configuration).Trigger(this.Configuration);
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
