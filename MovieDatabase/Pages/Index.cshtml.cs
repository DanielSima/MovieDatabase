using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MovieDatabase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method is called by client via HTTP GET and returns list of requested movies from DB.
        /// </summary>
        public IActionResult OnGetDefaultList()
        {
            List<string> procedureParameters = new List<string>();
            
            procedureParameters.Add($"@title = '{(Request.Query.ContainsKey("title") ? "%" + Request.Query["title"].ToString() + "%" : "%")}'");
            procedureParameters.Add($"@year = '{(Request.Query.ContainsKey("release_date") ? Request.Query["release_date"].ToString() : "%" )}'");
            procedureParameters.Add($"@genre = '{(Request.Query.ContainsKey("genre") ? Request.Query["genre"].ToString() : "%")}'");
            procedureParameters.Add($"@orderby = '{(Request.Query.ContainsKey("orderby") ? Request.Query["orderby"].ToString() : "m.title")}'");

            var requestedMovies = new MovieRepository().ExecuteProcedure("mp_get_movies", procedureParameters);

            var requestedMoviesSerialized = JsonConvert.SerializeObject(requestedMovies);
            var requestedMoviesContent = Content(requestedMoviesSerialized);
            return requestedMoviesContent;
        }

        public void OnPost()
        {
            //Movie.Test();
        }
    }
}
