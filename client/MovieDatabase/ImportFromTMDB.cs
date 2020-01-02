using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MovieDatabase
{
    /// <summary>
    /// Class used for importing data from TMDB.
    /// </summary>
    public static class ImportFromTMDB
    {
        /// <summary>
        /// Imports movies, people, and everything else needed.
        /// Doesn't check for duplicates.
        /// </summary>
        public static void ImportAll(List<int> pages)
        {
            SaveMovieToDB(GetMovieIdsByPopularity(pages));
            foreach (var movie in new MovieRepository().GetMultiple())
            {
                SavePeopleToDB(GetPersonIdsByMovie(movie.tmdbId), movie.Id);
                SaveGenresToDB(GetGenreIdsByMovie(movie.tmdbId), movie.Id);
                SaveLanguagesToDB(GetLanguagesByMovie(movie.tmdbId), movie.Id);
                SaveCountriesToDB(GetCountriesByMovie(movie.tmdbId), movie.Id);
                SaveReviewsToDB(GetReviewsByMovie(movie.tmdbId), movie.Id);
            }
        }

        ////////////////////////////////////////////////////
        //Reviews

        /// <summary>
        /// Saves reviews to my DB.
        /// </summary>
        private static void SaveReviewsToDB(Dictionary<string, string> reviews, int movieId)
        {
            foreach (var review in reviews)
            {
                //check if review exists
                if (new ReviewRepository().GetByAuthor(review.Key, movieId) == null)
                    //if not create it
                    new ReviewRepository().Create(new Review(review.Value, DateTime.Now, review.Key, movieId));
            }
        }

        /// <summary>
        /// Gets reviews from TMDB by movie.
        /// </summary>
        private static Dictionary<string, string> GetReviewsByMovie(int tmdbId)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmdbId}/reviews?api_key=6dc57c84bc4a3872ae46d21b6202e6a1&language=en-US&page=1");
            //it is apparently not possible to get star rating
            foreach (var review in data["results"])
            {
                values.TryAdd(review["author"].Value<string>(), review["content"].Value<string>());
            }
            return values;
        }

        ////////////////////////////////////////////////////
        //Countries

        /// <summary>
        /// Saves countries and their references to my DB.
        /// </summary>
        private static void SaveCountriesToDB(Dictionary<string, string> countryIds, int movieId)
        {
            foreach (var country in countryIds)
            {
                //check if language exists
                if (new CountryRepository().GetByName(country.Value) == null)
                    //if not create it
                    new CountryRepository().Create(new Country(country.Key, country.Value));

                //create Genre_movie relation
                new CountryMovieRepository().Create(
                    new CountryMovie(new CountryRepository().GetByName(country.Value).Id, movieId));
            }
        }

        /// <summary>
        /// Gets countries from TMDB by movie.
        /// </summary>
        private static Dictionary<string, string> GetCountriesByMovie(int tmdbId)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmdbId}?&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");
            foreach (var language in data["production_countries"])
            {
                values.Add(language["iso_3166_1"].Value<string>(), language["name"].Value<string>());
            }
            return values;
        }

        ////////////////////////////////////////////////////
        //Languages

        /// <summary>
        /// Saves languages and their references to my DB.
        /// </summary>
        private static void SaveLanguagesToDB(Dictionary<string, string> languageIds, int movieId)
        {
            foreach (var language in languageIds)
            {
                //check if language exists
                if (new LanguageRepository().GetByName(language.Value) == null)
                    //if not create it
                    new LanguageRepository().Create(new Language(language.Key, language.Value));

                //create Genre_movie relation
                new LanguageMovieRepository().Create(
                    new LanguageMovie(new LanguageRepository().GetByName(language.Value).Id, movieId));
            }
        }

        /// <summary>
        /// Gets languages from TMDB by movie.
        /// </summary>
        private static Dictionary<string, string> GetLanguagesByMovie(int tmdbId)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmdbId}?&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");
            foreach (var language in data["spoken_languages"])
            {
                values.Add(language["iso_639_1"].Value<string>(), language["name"].Value<string>());
            }
            return values;
        }

        ////////////////////////////////////////////////////
        //Genres

        /// <summary>
        /// Saves genres and their references to my DB.
        /// </summary>
        private static void SaveGenresToDB(List<int> genreIds, int movieId)
        {
            foreach (var genreId in genreIds)
            {
                string genre = GetGenreTitle(genreId);
                //check if genre exists
                if (new GenreRepository().GetByName(genre) == null)
                    //if not create it
                    new GenreRepository().Create(new Genre(genreId, genre));

                //create Genre_movie relation
                new GenreMovieRepository().Create(
                    new GenreMovie(new GenreRepository().GetByName(genre).Id, movieId));
            }
        }

        /// <summary>
        /// Gets genre ids from TMDB by movie.
        /// </summary>
        private static List<int> GetGenreIdsByMovie(int tmbdId)
        {
            List<int> ids = new List<int>();

            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmbdId}?&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");

            //save directors
            foreach (var genre in data["genres"])
            {
                ids.Add(genre["id"].Value<int>());
            }
            return ids;
        }

        /// <summary>
        /// Gets genre title.
        /// </summary>
        private static string GetGenreTitle(int tmdbId)
        {
            var data = TMDBRequest($"https://api.themoviedb.org/3/genre/movie/list?api_key=6dc57c84bc4a3872ae46d21b6202e6a1&language=en-US");
            foreach (var genre in data["genres"])
            {
                if (genre["id"].Value<int>() == tmdbId)
                    return genre["name"].Value<string>();
            }
            return "";
        }

        ////////////////////////////////////////////////////
        //People

        /// <summary>
        /// Saves people and their references to my DB.
        /// </summary>
        private static void SavePeopleToDB(Dictionary<int, string> peopleIds, int movieId)
        {
            foreach (var person in peopleIds)
            {
                //get person info from TMBD
                List<string> receivedAttributes = GetPersonAttributes(person.Key,
                    new List<string> { "id", "name", "birthday", "place_of_birth", "gender", "profile_path" });
                int tmdbId = int.Parse(receivedAttributes[0]);
                string name = receivedAttributes[1];
                DateTime dayOfBirth = DateTime.Parse(receivedAttributes[2] ?? "1970-01-01"); //feels like cheating
                string placeOfBirth = receivedAttributes[3] ?? "";
                int gender = int.Parse(receivedAttributes[4]) > 2 ? 0 : int.Parse(receivedAttributes[4]);//somebody had 3 even though official documentation says 0 to 2 ???
                string photoPath = receivedAttributes[5];
                //check if person exists
                if (new PersonRepository().GetByName(name) == null)
                    //if not create him/her/?
                    new PersonRepository().Create(new Person(tmdbId, name, dayOfBirth, placeOfBirth, gender, photoPath));

                //create either Director_movie or Actor_move relation
                if (person.Value == "Director")
                {
                    new DirectorMovieRepository().Create(
                        new DirectorMovie(new PersonRepository().GetByName(name).Id, movieId));
                }
                else
                {
                    new ActorMovieRepository().Create(
                        new ActorMovie(person.Value, new PersonRepository().GetByName(name).Id, movieId));
                }
            }
        }

        /// <summary>
        /// Gets person ids and either their character name or 'director' from TMDB by movie.
        /// </summary>
        private static Dictionary<int, string> GetPersonIdsByMovie(int tmbdId)
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();

            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmbdId}?&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1" +
                $"&append_to_response=credits");

            //save directors
            foreach (var person in data["credits"]["crew"])
            {
                if (person["job"].Value<string>() == "Director")
                {
                    ids.Add(person["id"].Value<int>(), "Director");
                }
            }
            //and top 10 actors
            foreach (var person in data["credits"]["cast"])
            {
                if (person["order"].Value<int>() < 10)
                {
                    ids.TryAdd(person["id"].Value<int>(), person["character"].Value<string>());
                }
            }
            return ids;
        }

        /// <summary>
        /// Gets needed attributes for person by its ID.
        /// </summary>
        private static List<string> GetPersonAttributes(int tmbdId, List<string> attributes)
        {
            List<string> values = new List<string>();
            var data = TMDBRequest($"https://api.themoviedb.org/3/person/{tmbdId}?api_key=6dc57c84bc4a3872ae46d21b6202e6a1&language=en-US");
            foreach (var attribute in attributes)
            {
                values.Add(data[attribute].Value<string>());
            }
            return values;
        }

        ////////////////////////////////////////////////////
        //Movies

        /// <summary>
        /// Saves movies to my DB.
        /// </summary>
        private static void SaveMovieToDB(List<int> movieIds)
        {
            foreach (var movie in movieIds)
            {
                List<string> receivedAttributes = GetMovieAttributes(movie,
                    new List<string> { "id", "title", "overview", "release_date", "runtime", "vote_average", "poster_path", "budget" });
                int tmdbId = int.Parse(receivedAttributes[0]);
                string title = receivedAttributes[1];
                string description = receivedAttributes[2];
                DateTime relaseDate = DateTime.Parse(receivedAttributes[3]);
                int runtime = int.Parse(receivedAttributes[4]);
                double rating = double.Parse(receivedAttributes[5]);
                string posterPath = receivedAttributes[6];
                int budget = int.Parse(receivedAttributes[7]);

                new MovieRepository().Create(new Movie(tmdbId, title, description, relaseDate, runtime, rating, posterPath, budget));
            }
        }

        /// <summary>
        /// Gets movie ids from TMDB by popularity.
        /// </summary>
        private static List<int> GetMovieIdsByPopularity(List<int> pages)
        {
            List<int> ids = new List<int>();

            foreach (var page in pages)
            {
                var data = TMDBRequest($"https://api.themoviedb.org/3/discover/movie?page={page}&include_video=false" +
                    $"&include_adult=false&sort_by=popularity.desc&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");

                foreach (var movie in data["results"])
                {
                    ids.Add(movie["id"].Value<int>());
                }
            }
            return ids;
        }

        /// <summary>
        /// Gets needed attributes for movie by its ID.
        /// </summary>
        private static List<string> GetMovieAttributes(int tmbdId, List<string> attributes)
        {
            List<string> values = new List<string>();
            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmbdId}?language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");
            foreach (var attribute in attributes)
            {
                values.Add(data[attribute].Value<string>());
            }
            return values;
        }

        ////////////////////////////////////////////////////

        /// <summary>
        /// Request data on url and returns them.
        /// </summary>
        private static JObject TMDBRequest(string url)
        {
            //this is bad
            Thread.Sleep(260);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(request);
                return (JObject)JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}