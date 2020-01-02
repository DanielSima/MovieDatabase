using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace MovieDatabase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Used to import data, in future could let user add movie by name or something.
			
			//If you want to test the application, import data from sql. This code runs more than 45
			// minutes because of api limitation.
			
            //ImportFromTMDB.ImportAll(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}