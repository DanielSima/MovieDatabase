using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MovieDatabase
{
    public class MovieModel : PageModel
    {
        public IActionResult OnGetMovie()
        {
            List<string> procedureParameters = new List<string>();

            procedureParameters.Add($"@movie_title = '{Request.Query["title"].ToString()}'");
            procedureParameters.Add("@array_name = 'movie'");

            var requestedMovies = new MovieRepository().ExecuteProcedure("mp_movie_info_array", procedureParameters);

            var requestedMoviesSerialized = JsonConvert.SerializeObject(requestedMovies);
            var requestedMoviesContent = Content(requestedMoviesSerialized);
            return requestedMoviesContent;
        }
    }
}