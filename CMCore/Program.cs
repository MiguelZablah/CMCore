using CMCore.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CMCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).SeedDatabase().Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
