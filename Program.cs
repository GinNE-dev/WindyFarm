using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using WindyFarm.Gin;
using WindyFarm.Gin.Database.Models;
using WindyFarm.Gin.SystemLog;
using WindyFarm.Utils;

namespace WindyFarm
{
    public class Program
    {
        private static Server? server;
        private static int Main(string[] args)
        {
            TextDictionary.Setup(CultureCode.Default);
            IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets<Program>()
                    .Build();

            var connectionString = configuration.GetConnectionString("WindyFarmDatabase");

            if (connectionString == null)
            {
                GinLogger.Warning($"Can not found connection string named 'WindyFarmDatabase'");
                return -1;
            }

            WindyFarmDatabaseContext dbcontext;
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<WindyFarmDatabaseContext>();
                optionsBuilder.UseSqlServer(connectionString);
                dbcontext = new(optionsBuilder.Options);

            }
            catch (Exception ex)
            {

                GinLogger.Fatal(ex);
                return -1;
            }

            int port = 44433;
            server = new Server(IPAddress.Loopback, port, dbcontext);
            server.Start();
            Console.CancelKeyPress += OnExit;
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            for (; ; )
            {
                string? line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    continue;

                GinLogger.Print(line);
                switch (line.ToLower().Trim())
                {
                    case "start":
                        if (!server.IsStarted)
                            server.Start();
                        else
                            GinLogger.Print("The server is running now, can not start!");
                        break;
                    case "stop":
                        if (server.IsStarted)
                            server.Stop();
                        else
                            GinLogger.Print("The server is not running now, can not stop!");
                        break;
                    case "rs":
                    case "restart":
                        if (server.IsStarted)
                        {
                            server.Restart();
                        }
                        else
                        {
                            GinLogger.Print("The server is not running now, can not restart!");
                        }
                        break;
                    case "cls":
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        server.SendAllText(line);
                        break;
                }
            }
        }

        private static void OnExit(object? sender, EventArgs e)
        {
            server?.Stop();
        }
    }
}