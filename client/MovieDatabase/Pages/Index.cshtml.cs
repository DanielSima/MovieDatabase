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
        public IActionResult OnGetList()
        {
            var title = $"{(Request.Query.ContainsKey("title") ? "%" + Request.Query["title"].ToString() + "%" : "%")}";
            var year = $"{(Request.Query.ContainsKey("release_date") ? Request.Query["release_date"].ToString() : "%")}";
            var genre = $"{(Request.Query.ContainsKey("genre") ? Request.Query["genre"].ToString() : "%")}";
            var orderby = $"{(Request.Query.ContainsKey("orderby") ? Request.Query["orderby"].ToString() : "m.release_date desc")}";

            var requestedMovies = new MovieRepository().GetMultiple(
                joins: $"inner join Genre_Movie gm on (gm.movie = m.ID) inner join Genre g on(gm.genre = g.ID)",
                where: $"m.title like '{title}' and g.title like '{genre}' and year(m.release_date) like '{year}'",
                orderBy: $"{orderby}");

            var requestedMoviesSerialized = JsonConvert.SerializeObject(requestedMovies);
            var requestedMoviesContent = Content(requestedMoviesSerialized);
            return requestedMoviesContent;
        }
    }
}