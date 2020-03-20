using AssetWorx.Client;
using AssetWorx.Client.Libs;
using AssetWorx.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetWorxClientSampleProject
{
    /// <summary>
    /// This Program demonstrates doing web service calls using the AssetWorx.Client dll
    /// </summary>
    class Program
    {
        static AuthToken authToken;
        static ServiceOptions serviceOptions;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter Servername to test:");
            string readServerName = Console.ReadLine();
            Console.WriteLine("Enter Username:");
            string readUsername = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            string readPassword = Console.ReadLine();
            string server = String.IsNullOrWhiteSpace(readServerName) ? "http://localhost" : readServerName;
            string login = String.IsNullOrWhiteSpace(readServerName) ? "admin" : readUsername;
            string password = String.IsNullOrWhiteSpace(readPassword) ? "demo" : readPassword;

            Console.WriteLine(string.Format("Using servername {0}, username {1}, password {2}", server, login, password));

            //authToken instance should be a singleton to maximize efficiency.
            authToken = new AuthToken();
            serviceOptions = new ServiceOptions() { BaseUrl = server, Username = login, Password = password };
            TestSomeCalls().Wait();

            Console.ReadLine();
           
        }

        static async Task TestSomeCalls()
        {
            AssetClient assetClient = new AssetClient(serviceOptions, authToken);
            List<Asset> assets = await assetClient.GetAll(null, null, null);
            foreach (Asset asset in assets)
            {
                Console.WriteLine(string.Format("Got asset {0}", asset.Name));
            }
            assets = await assetClient.GetChangedAssets(DateTimeOffset.Now.AddDays(-10));
            Console.WriteLine("***************CHANGED ASSETS**************");
            foreach (Asset asset in assets)
            {
                Console.WriteLine(string.Format("Got changed asset {0}", asset.Name));
            }
        }
    }
}
