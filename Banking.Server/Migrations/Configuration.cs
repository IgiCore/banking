using JetBrains.Annotations;
using NFive.SDK.Server.Migrations;
using IgiCore.Banking.Server.Storage;

namespace IgiCore.Banking.Server.Migrations
{
	[UsedImplicitly]
	public sealed class Configuration : MigrationConfiguration<StorageContext> { }
}
