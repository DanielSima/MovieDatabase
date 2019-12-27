using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MovieDatabase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ImportFromTMDB.SaveMovieToDB(ImportFromTMDB.GetMovieIdsByPopularity(new List<int> { 16 }));
            foreach (var movie in new MovieRepository().GetMultiple())
            {
                ImportFromTMDB.SavePeopleToDB(ImportFromTMDB.GetPersonIdsByMovie(movie.tmdbId), movie.Id);
            }

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
