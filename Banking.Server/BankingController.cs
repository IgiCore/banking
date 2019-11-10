using IgiCore.Banking.Server.Models;
using IgiCore.Banking.Server.Storage;
using IgiCore.Banking.Shared;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models;
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
				context.Banks.AddOrUpdate(new Bank
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
						},

						new BankBranch
						{
							Name = "Rockford Hills",
							Position = new Position(1.260034f, -5.850761f, -0.9303261f), // TODO: Exact
							Heading = 280.00f // TODO:
						}
					},
					ATMs = new List<BankATM>
					{
						// Legion Square
						new BankATM
						{
							Name = "FLCA LNS 0001",
							Hash = 506770882,
							Position = new Position(147.4731f, -1036.218f, 28.36778f)
						},
						new BankATM
						{
							Name = "FLCA LNS 0002",
							Hash = 506770882,
							Position = new Position(145.8392f, -1035.625f, 28.36778f)
						},

						// Rockford Hills
						new BankATM
						{
							Name = "FLCA RDH 0001",
							Hash = 506770882,
							Position = new Position(-1205.378f, -326.5286f, 36.85104f)
						},
						new BankATM
						{
							Name = "FLCA RDH 0002",
							Hash = 506770882,
							Position = new Position(-1206.142f, -325.0316f, 36.85104f)
						}
					}
				});

				context.Banks.AddOrUpdate(new Bank
				{
					Id = new Guid("1fde7503-18c1-4620-a5a9-057821e3b7da"),
					Name = "Union Depository",
					ATMs = new List<BankATM>
					{
						// Union Depository downtown building
						new BankATM
						{
							Name = "UDEP DTN 0001",
							Hash = 2930269768,
							Position = new Position(-30.09957f,-723.2863f,43.22287f)
						},
						new BankATM
						{
							Name = "UDEP DTN 0002",
							Hash = 2930269768,
							Position = new Position(-27.89034f,-724.1089f,43.22287f)
						},
						
						// Slaughter, Slaughter & Slaughter LLP downtown building
						new BankATM
						{
							Name = "UDEP SSS 0001",
							Hash = 2930269768,
							Position = new Position(-254.5219f,-692.8869f,32.57825f)
						},
						new BankATM
						{
							Name = "UDEP SSS 0002",
							Hash = 2930269768,
							Position = new Position(-259.2767f ,-723.2652f,32.70155f)
						},
						new BankATM
						{
							Name = "UDEP SSS 0003",
							Hash = 2930269768,
							Position = new Position(-256.6386f ,-715.8898f ,32.7883f)
						},

						// FIB HQ Pillbox building
						new BankATM
						{
							Name = "UDEP FIB 0001",
							Hash = 2930269768,
							Position = new Position(111.3886f,-774.8401f,30.43766f)
						},
						new BankATM
						{
							Name = "UDEP FIB 0002",
							Hash = 2930269768,
							Position = new Position(114.5474f,-775.9721f,30.41736f)
						},

						// Escapism Travel Legion Square building
						new BankATM
						{
							Name = "UDEP ETL 0001",
							Hash = 2930269768,
							Position = new Position(118.6416f, -883.5695f, 30.13945f)
						},

						// Maison Ricard Legion Square building
						new BankATM
						{
							Name = "UDEP MRD 0001",
							Hash = 2930269768,
							Position = new Position(112.4761f,-819.808f,30.33955f)
						},
						
						// Gruppe Sechs Pillbox building
						new BankATM
						{
							Name = "UDEP GPS 0001",
							Hash = 2930269768,
							Position = new Position(-204.0193f, -861.0091f, 29.27133f)
						},
						
						// LS Quik Pillbox building
						new BankATM
						{
							Name = "UDEP LSQ 0001",
							Hash = 2930269768,
							Position = new Position(-303.2257f, -829.3121f, 31.41977f)
						},
						new BankATM
						{
							Name = "UDEP LSQ 0002",
							Hash = 2930269768,
							Position = new Position(-301.6573f, -829.5886f, 31.41977f)
						}
					}
				});

				var wallPositions = new List<Position>
				{
					new Position(24.5933f, -945.543f, 28.33305f),
					new Position(5.686035f, -919.9551f, 28.48088f),
					new Position(296.1756f, -896.2318f, 28.29015f),
					new Position(296.8775f, -894.3196f, 28.26148f),
					new Position(527.7776f, -160.6609f, 56.13671f),
					new Position(285.3485f, 142.9751f, 103.1623f),
					new Position(1137.811f, -468.8625f, 65.69865f),
					new Position(1167.06f, -455.6541f, 65.81857f),
					new Position(1077.779f, -776.9664f, 57.25652f),
					new Position(289.53f, -1256.788f, 28.44057f),
					new Position(289.2679f, -1282.32f, 28.65519f),
					new Position(-846.6537f, -341.509f, 37.6685f),
					new Position(-847.204f, -340.4291f, 37.6793f),
					new Position(-720.6288f, -415.5243f, 33.97996f),
					new Position(-1410.736f, -98.92789f, 51.39701f),
					new Position(-1410.183f, -100.6454f, 51.39652f),
					new Position(-712.9357f, -818.4827f, 22.74066f),
					new Position(-710.0828f, -818.4756f, 22.73634f),
					new Position(-660.6763f, -854.4882f, 23.45663f),
					new Position(-594.6144f, -1160.852f, 21.33351f),
					new Position(-596.1251f, -1160.85f, 21.3336f),
					new Position(-1569.84f, -547.0309f, 33.93216f),
					new Position(-1570.765f, -547.7035f, 33.93216f),
					new Position(-1305.708f, -706.6881f, 24.31447f),
					new Position(-2071.928f, -317.2862f, 12.31808f),
					new Position(-821.8936f, -1081.555f, 10.13664f),
					new Position(-2295.853f, 357.9348f, 173.6014f),
					new Position(-2295.069f, 356.2556f, 173.6014f),
					new Position(-2294.3f, 354.6056f, 173.6014f),
					new Position(2558.324f, 350.988f, 107.5975f),
					new Position(156.1886f, 6643.2f, 30.59372f),
					new Position(173.8246f, 6638.217f, 30.59372f),
					new Position(-282.7141f, 6226.43f, 30.49648f),
					new Position(-95.87029f, 6457.462f, 30.47394f),
					new Position(-97.63721f, 6455.732f, 30.46793f),
					new Position(-132.6663f, 6366.876f, 30.47258f),
					new Position(-386.4596f, 6046.411f, 30.47399f)
				};

				var standupPositions = new List<Position>
				{
					new Position(228.0324f, 337.8501f, 104.5013f),
					new Position(158.7965f, 234.7452f, 105.6433f),
					new Position(-57.17029f, -92.37918f, 56.75069f),
					new Position(357.1284f, 174.0836f, 102.0597f),
					new Position(-1044.466f, -2739.641f, 8.12406f),
					new Position(-1415.48f, -212.3324f, 45.49542f),
					new Position(-1430.663f, -211.3587f, 45.47162f),
					new Position(-1282.098f, -210.5599f, 41.43031f),
					new Position(-1286.704f, -213.7827f, 41.43031f),
					new Position(-1289.742f, -227.165f, 41.43031f),
					new Position(-1285.136f, -223.9422f, 41.43031f),
					new Position(-1110.228f, -1691.154f, 3.378483f),
					new Position(-1091.887f, 2709.053f, 17.91941f),
					new Position(-3144.887f, 1127.811f, 19.83804f),
					new Position(2564f, 2584.553f, 37.06807f),
					new Position(1687.395f, 4815.9f, 41.00647f),
					new Position(1700.694f, 6426.762f, 31.63297f),
					new Position(1822.971f, 3682.577f, 33.26745f),
					new Position(1171.523f, 2703.139f, 37.1477f),
					new Position(1172.457f, 2703.139f, 37.1477f)
				};

				var number = 1;

				context.Banks.AddOrUpdate(new Bank
				{
					Id = new Guid("8c7aeb27-e252-4bff-be3b-1ba6632ce872"),
					Name = "Universal",
					ATMs = wallPositions.Select(p => new BankATM
					{
						Name = $"ATM {number++:D4}",
						Hash = 3168729781,
						Position = p
					}).Concat(standupPositions.Select(p => new BankATM
					{
						Name = $"ATM {number++:D4}",
						Hash = 870868698,
						Position = p
					})).ToList()
				});

				context.SaveChanges();
			}

			return base.Started();
		}
	}
}
