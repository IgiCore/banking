// <auto-generated />
// ReSharper disable all

using System;
using System.Data.Entity.Migrations;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations.Infrastructure;

namespace IgiCore.Banking.Server.Migrations
{
    [GeneratedCode("NFive.Migration", "0.3 Alpha Build 183")]
    public class Init : DbMigration, IMigrationMetadata
    {
        string IMigrationMetadata.Id => "201909062230083_Init";
        
        string IMigrationMetadata.Source => null;
        
        string IMigrationMetadata.Target => "H4sIAAAAAAAEAO1dW2/bRhZ+X2D/A8HHhSvKSVu0htUiluNGaGQbltPaeQnG5EgiQg5VcmhYWOwv24f9Sf0Le3gfzkUcXtyojtCXeDjz8dxnDucc9c///u/05yffMx5xGLkBmZjHo7FpYGIHjktWEzOmy29+MH/+6Z//OH3r+E/Gb8W818k8WEmiibmmdHNiWZG9xj6KRr5rh0EULOnIDnwLOYH1ajz+0To+tjBAmIBlGKc3MaGuj9M/4M9pQGy8oTHy5oGDvSgfhyeLFNW4RD6ONsjGE3O2cqdBiEdniHwGIkcLHAL5owUNQrTCpvHGcxFQtcDe0jQQIQFFFGg++RDhBQ0DslpsYAB5t9sNhnlL5EU45+Wkmq7L1vhVwpZVLSyg7Diigd8S8Ph1LieLX95J2mYpx1TG/sbDTwnbqTgn5nUQuRk2/7aTqRcmEyfm5YX7iEeL819HqdAz9YyKlUcG95xgelRayXgE5jT69vWRMY09God4QnBMQ+QdGdfxg+fav+LtbfAZkwmJPY8lFsi9DoMNDuk2p/XONDKCFqBzDxR2CUvQg4dLDVo719/3XP+x1fpTi5E2O/4WzJ5uGR0kVvzGtgPwhykKnV2qUBh+rhIO6MhQTK9pKNEP/NdJQfCsNsDI7AYvc/ZmjiBYi1/IizpZk3H8S+w6rTU1DTGiuMQ4hz9uIdS0xjnHHpbi7F527ZJiyYzQ169av/cy9h9w2BMDJYTmBktDUL9pzNHTe0xWdD0x4Z+mceE+YacYyVE/EBdCPyyiYSyYdGW7uhY9xxkrA9h0BnWw6r/EqrvoehAtH/T7haLWMwQM8SW5kocIcMnakl446bFGlB7sWiOeIQ/BIbQUXACm1F787wP7cyX9syDwMCKNh4U23nY77+lpt/ODl71kL3uHojXjXN9/25q7Ki9gHKwaHM6Yz0JwuXU/e84wDib9kk26j0FKHAQjJ6WzVULXwqz7J3KHDO4LmvSMYv8cL12SmldfXgJCkUtwyOPsXvY7dldr2vObxe+uQ9f9zlnvanR0w7jjlmt+qNGafZM4V9sDlwjzIcLRDfZBUUxg0OG2VVzoFxMO8eBFbXGX6NFdpUYgz5Igg7nBXjohWrsbJs35VM24CAP/JvBy+yoffFoEcZjmMoHs6S0KV5i2oOh2voua9ClPCQwqqEietKUgO+ThHVRUMzhKigdyasqnMorUzh378o8geeo5iy48tKouF1q4/RqF2BGyWYnvZxN7+D7Yp4NDbwtobLyrayT77FYcw+A9AUGw9DfkxTAwFjRYm38WR7DzRVE5/1iUbyZJdvBNFAW2m8qJEXBl9PUXviWOsdMD6p8IwHNBNu4GpAGKBQ5Go2OBCRVmYSPyzw489L94ZhnGdPhN3Go3XTUfG4BP1jN5HpPPDoPyVzrsTppE7+3Pp+DzHGaRlXZjNwsZ5bEvZzm/nUyG8ZPsWymcQPKgEOUbBs9DArvAVH5vBD5WxSrRNrOMRBCLEjJz4wbQ4uu+PmwDoB5Uut3IYRIj1YCo9gopTKF/DaRdoteWuQpAWMxYXB2BOREwc6RHBv6A1xQ/S4JLWi1diMK9OIiKVv4YWedPm/fUIFR8C1G0IY524JcNmzyvKW3D8FlZrYJXeUTViKnteRZCKAdR0arNe3HYKiNnVQNiZUUgRbGIpagWOZ2jzQZOM0z1SD5iLLLSkek3i/Z1FH6GYdk1wfJxvnxTFui5p8l3AAdfuGFEIetADyg5FU4dX5jG7ROK4FG8TLEViMorokuxMPl3nprtrKYR9hAOuhLxBUz3MaGpALDc5eUIRlLkgzwUSpLGaeDFPlElnrtWlzkjC1EO6uOUOSOLUw7q46RlASxGOqC/vrg0YyGKsRYoaSpbw0BiApyutzjF8kZlCVbFeTpvrW1tuTiDPI8154eXXvaswnhZFv3lLeG5bKCf9l+63vvHEBUCVwfAQnGP9DGzggAWKhvRRygLAFiQclAfpygBYGGKsf3yLvnZuJ9nQQrW0atkKw8epYeQ3fmzCNmIFCEvTd15x6ql8HKJ5C0qSu+4U1AO8elOTqwK5l4Bc98O5qMC5qNCdBYnu31y5x3pXy+Xzj+HdPNq1eKDYwsIB7d8PrdUxs2iFKQWOovBvdqtnyW175HTf83JvFghUuNNeNqCU7ZcpMYt+0Afr6gjYaGKsRYoWSFJDSQbanFEkVDyrjUlXJxShCet4KSISfK1ZZ0Ji1AO6uNwhSYsGvdor4LP4IGnY9D5WgPO/ny3q3+/Fy2FuYpqZRDMuhbfXJJ7CSGNl1xjiWLSMpoUS2Y5iZQcxWmjkab8zqQjTa3JgZ3DSXciYxYlBVNlVYoGp/x9TUeTSG/BWppDukbzQ4FS5MzN3l6YAHNV+PWov7oMbGcC1Tr9zFIlef7Ocx+sgb9EfRkWIdzj8lPKHaq8z+XubU9zK2j+KQDhUjWbYhrA+qPrJBeq8+3iD2+UPB+l/5x6Lk4qTYoZc0TcJY5oVhhnfjf6jvsFgf3p5reiyKmV7DZ3k9d19xfU964GqO914A+a1uVeh9h2sx+cGPet9m1G1epEqbq73cSMevV2d0JAVTHyIwrtNQpr1cjH43EDUz0auQ/m1MucOrVVH2Q+sAv39yARU9rZ3MW92b7mLuu5LmZniC7mB7cLHfnxgbUpoeh1Rhz8NDH/nS46MWZ3n/J1R8ZVCJvziTE2/jOkT/Gpw8Gf9tOf2B7mB3fVxRPYK4wMaOkFqAfO/UA4H/vh7KdnybKxg3Ptp3O9VM/g+rpfknsdkrnncC5Vo3c3vsVGbx5Hh6Z643c3G671fXfZO+td310Q7hSLdURw32Mt1w/e5egq7QVvlkG7TvCDL+/dRqlSYJ9+1CH6FcvmuE5tpwAHJoRDTJLfPIUgFdEQTFu4FrwOXWK7G+Sx1ItfpHXMMhFlCcc/OccbDJsnoRIOdV634wt8Cc05SRP/g3fmDqL1pJexdRPu30Lb0hLhvdS0Vo/yENouWk67dCL/HXSurCH9smqX92qLrXq86na2Yut0Ymf3VLD3PQSg9GxHEju6BZNp6NfWa9dueHfZ+q3/9qau7oY3ar4rCX7qtm/lO2QduOqm8IaWcNVblP2uqqbxnT3jqrfom4Wyo1wFLcI+f7O50DUs8XnFUUhRHdRwWa3NYM+O8g6MVcZdq3MZiKGhWsfbM1b3Kr54Q5+9Ft3hYvUA7BDM/24ANqjIXVUQST0EwXZtbyjnzMgyKPYojqJiCpeIzDFFkNKgNyF1l8im8NjGUZTmkvnP4LyFCOvMyFVMNzEFliHieltWGMlWt+v9aQt8nebTq036s0hDsABkuklWdkXOYtdzSrovJFmvAiLZQ3/BMJ7pErZkilfbEukyIJpAufjKrf8W+xsPwKIrskCPuAttkOC/xytkb4saEDVIsyLqYj89d9EqRH6UY1Tr4U+wYcd/+un/6wFVj3VjAAA=";
        
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccountCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        Pin = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Name = c.String(maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAccountMembers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 1000, unicode: false),
                        AccountNumber = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        Locked = c.Boolean(nullable: false, storeType: "bit"),
                        Bank_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.Bank_Id)
                .Index(t => t.Bank_Id);
            
            CreateTable(
                "dbo.BankATMs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 1000, unicode: false),
                        Hash = c.Long(nullable: false),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        Bank_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.Bank_Id)
                .Index(t => t.Bank_Id);
            
            CreateTable(
                "dbo.BankBranches",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 1000, unicode: false),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        Heading = c.Single(nullable: false),
                        Bank_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.Bank_Id)
                .Index(t => t.Bank_Id);
            
            CreateTable(
                "dbo.BankCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        ItemDefinitionId = c.Guid(nullable: false),
                        ContainerId = c.Guid(),
                        Weight = c.Single(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        X = c.Int(),
                        Y = c.Int(),
                        Rotated = c.Boolean(nullable: false, storeType: "bit"),
                        UsesRemaining = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 1000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankBranches", "Bank_Id", "dbo.Banks");
            DropForeignKey("dbo.BankATMs", "Bank_Id", "dbo.Banks");
            DropForeignKey("dbo.BankAccounts", "Bank_Id", "dbo.Banks");
            DropIndex("dbo.BankBranches", new[] { "Bank_Id" });
            DropIndex("dbo.BankATMs", new[] { "Bank_Id" });
            DropIndex("dbo.BankAccounts", new[] { "Bank_Id" });
            DropTable("dbo.Banks");
            DropTable("dbo.BankCards");
            DropTable("dbo.BankBranches");
            DropTable("dbo.BankATMs");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.BankAccountMembers");
            DropTable("dbo.BankAccountCards");
        }
    }
}