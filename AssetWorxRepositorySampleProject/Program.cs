using AssetWorx.Entities;
using AssetWorx.Repository;
using AssetWorx.Repository.Libs.SqlFormatters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetWorxRepositorySampleProject
{
    /// <summary>
    /// This Program demonstrates doing db calls directly using the AssetWorx.Repository dll
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            TestSomeCalls().Wait();
        }

        static async Task TestSomeCalls()
        {
            Console.WriteLine(@"Enter Servername to test(i.e. mydbserver\sqlexpress):");
            string readServerName = Console.ReadLine();
            Console.WriteLine("Enter Username:");
            string readUsername = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            string readPassword = Console.ReadLine();
            string server = String.IsNullOrWhiteSpace(readServerName) ? @"localhost\sqlexpress" : readServerName;
            string login = String.IsNullOrWhiteSpace(readServerName) ? "sa" : readUsername;
            string password = String.IsNullOrWhiteSpace(readPassword) ? "my_sa_passworx" : readPassword;

            DbOptions dbOption = new DbOptions()
            {
                DbHostname = server,
                DbName = "AssetWorx",
                DbUsername = login,
                DbPassword = password,
                DbType = "Sql Server"
            };
            SqlServerFormatter sqlServerFormatter = new SqlServerFormatter(dbOption);
            LocationRepository locationRepo = new LocationRepository(dbOption, sqlServerFormatter);
            ApplicationSettingRepository appSettingsRepo = new ApplicationSettingRepository(dbOption, sqlServerFormatter);
            LocationHistoryRepository locationHistoryRepo = new LocationHistoryRepository(dbOption, sqlServerFormatter);
            EventRepository eventRepo = new EventRepository(dbOption, appSettingsRepo, sqlServerFormatter);
            MapAreaRepository mapAreaRepo = new MapAreaRepository(dbOption, sqlServerFormatter);
            AssetRepository assetRepo = new AssetRepository(dbOption, locationRepo, locationHistoryRepo, eventRepo,
                appSettingsRepo, mapAreaRepo, sqlServerFormatter);

            List<Asset> assets = await assetRepo.GetAll(null, null, null);
            Console.WriteLine("***************ALL ASSETS**************");
            foreach (Asset asset in assets)
            {
                Console.WriteLine(string.Format("Got asset {0}", asset.Name));
            }
            assets = await assetRepo.GetChangedAssets(DateTimeOffset.Now.AddDays(-10));
            Console.WriteLine("***************CHANGED ASSETS**************");
            foreach (Asset asset in assets)
            {
                Console.WriteLine(string.Format("Got changed asset {0}", asset.Name));
            }
            Console.ReadLine();
        }
    }
}
