using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase
{
    public class MovieModel : PageModel
    {
        public IActionResult OnGetMovie()
        {
            List<string> procedureParameters = new List<string>();

            var title = ($"{Request.Query["title"].ToString()}");

            var requestedMovie = new MovieRepository().GetMultiple(where: $"m.title = '{title}'")[0];
            var requestedActors = new PersonRepository().GetMultiple(joins: "inner join Actor_Movie am on (am.person = p.ID) inner join Movie m on(am.movie = m.ID)", where: $"m.title = '{title}'");
            var requestedActorMovies = new ActorMovieRepository().GetMultiple(joins: "inner join Person p on (am.person = p.ID) inner join Movie m on(am.movie = m.ID)", where: $"m.title = '{title}'");
            var requestedDirectors = new PersonRepository().GetMultiple(joins: "inner join Director_Movie am on (am.person = p.ID) inner join Movie m on(am.movie = m.ID)", where: $"m.title = '{title}'");
            var requestedGenres = new GenreRepository().GetMultiple(joins: "inner join Genre_Movie gm on (gm.genre = g.ID) inner join Movie m on(gm.movie = m.ID)", where: $"m.title = '{title}'");
            var requestedReviews = new ReviewRepository().GetMultiple(where: $"r.movie = '{requestedMovie.Id}'");

            JObject movie =
                new JObject(
                    new JProperty("movie",
                        new JObject(
                            new JProperty("title", requestedMovie.title),
                            new JProperty("description", requestedMovie.description),
                            new JProperty("release_date", requestedMovie.releaseDate),
                            new JProperty("runtime", requestedMovie.runtime),
                            new JProperty("rating", requestedMovie.rating),
                            new JProperty("poster_path", requestedMovie.posterPath),
                            new JProperty("budget", requestedMovie.budget),
                            new JProperty("genres",
                                new JArray(
                                    from genre in requestedGenres
                                    select new JObject(new JProperty("title", genre.title))
                                )
                            ),
                            new JProperty("directors",
                                new JArray(
                                    from director in requestedDirectors
                                    select new JObject(new JProperty("name", director.name))
                                )
                            ),
                            new JProperty("actors",
                                new JArray(
                                    from actorMovie in requestedActorMovies
                                    from actor in requestedActors
                                    where actor.Id == actorMovie.person
                                    select new JObject(
                                        new JProperty("name", actor.name),
                                        new JProperty("photo_path", actor.photoPath),
                                        new JProperty("character", actorMovie.character)
                                    )
                                )
                            ),
                             new JProperty("reviews",
                                new JArray(
                                    from review in requestedReviews
                                    select new JObject(
                                        new JProperty("author", review.author),
                                        new JProperty("description", review.desription),
                                        new JProperty("date_created", review.dateCreated)
                                    )
                                )
                            )
                        )
                    )
                );

            var requestedMoviesSerialized = JsonConvert.SerializeObject(movie);
            var requestedMoviesContent = Content(requestedMoviesSerialized);
            return requestedMoviesContent;
        }
    }
}