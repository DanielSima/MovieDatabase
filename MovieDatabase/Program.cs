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
            /*ImportFromTMDB.SaveMovieToDB(ImportFromTMDB.GetMovieIdsByPopularity(new List<int> { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 }));
            foreach (var movie in new MovieRepository().GetMultiple())
            {
                ImportFromTMDB.SavePeopleToDB(ImportFromTMDB.GetPersonIdsByMovie(movie.tmdbId), movie.Id);
                ImportFromTMDB.SaveGenresToDB(ImportFromTMDB.GetGenreIdsByMovie(movie.tmdbId), movie.Id);
                ImportFromTMDB.SaveLanguagesToDB(ImportFromTMDB.GetLanguagesByMovie(movie.tmdbId), movie.Id);
                ImportFromTMDB.SaveCountriesToDB(ImportFromTMDB.GetCountriesByMovie(movie.tmdbId), movie.Id);
                ImportFromTMDB.SaveReviewsToDB(ImportFromTMDB.GetReviewsByMovie(movie.tmdbId), movie.Id);
            }*/

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
