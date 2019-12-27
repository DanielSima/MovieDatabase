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
        ////////////////////////////////////////////////////
        //People

        /// <summary>
        /// Saves people and their references to my DB.
        /// </summary>
        public static void SavePeopleToDB(Dictionary<int, string> peopleIds, int movieId)
        {
            foreach (var person in peopleIds)
            {
                List<string> receivedAttributes = GetPersonAttributes(person.Key,
                    new List<string> { "id", "name", "birthday", "place_of_birth", "gender", "profile_path" });
                int tmdbId = int.Parse(receivedAttributes[0]);
                string name = receivedAttributes[1].Replace("'", "''");
                DateTime dayOfBirth = DateTime.Parse(receivedAttributes[2]);
                string placeOfBirth = receivedAttributes[3];
                int gender = int.Parse(receivedAttributes[4]);
                string photoPath = receivedAttributes[5];

                new PersonRepository().Create(new Person(tmdbId, name, dayOfBirth, placeOfBirth, gender, photoPath));
                if(person.Value == "Director")
                {
                    new DirectorMovieRepository().Create(new DirectorMovie(
                        new PersonRepository().GetByName(name).Id,
                        movieId));
                }
                else
                {
                    new ActorMovieRepository().Create(new ActorMovie(
                        person.Value,
                        new PersonRepository().GetByName(name).Id,
                        movieId));
                }

            }
        }

        /// <summary>
        /// Gets person ids and either their character name or 'director' from TMDB by movie.
        /// </summary>
        public static Dictionary<int, string> GetPersonIdsByMovie(int tmbdId)
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();

            var data = TMDBRequest($"https://api.themoviedb.org/3/movie/{tmbdId}?&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1" +
                $"&append_to_response=credits");

            foreach (var person in data["results"]["cast"])
            {
                //save directors
                if(person["job"].Value<string>() == "Director")
                {
                    ids.Add(person["id"].Value<int>(), "Director");
                }
                //and top 10 actors
                else if(person["order"].Value<int>() <= 10)
                {
                    ids.Add(person["id"].Value<int>(), person["character"].Value<string>());
                }
            }
            return ids;
        }

        /// <summary>
        /// Gets needed attributes for person by its ID.
        /// </summary>
        public static List<string> GetPersonAttributes(int tmbdId, List<string> attributes)
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
        public static void SaveMovieToDB(List<int> movieIds)
        {
            foreach (var movie in movieIds)
            {
                List<string> receivedAttributes = GetMovieAttributes(movie,
                    new List<string> { "id", "title", "overview", "release_date", "runtime", "vote_average", "poster_path", "budget" });
                int tmdbId = int.Parse(receivedAttributes[0]);
                string title = receivedAttributes[1].Replace("'", "''");
                string description = receivedAttributes[2].Replace("'", "''");
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
        public static List<int> GetMovieIdsByPopularity(List<int> pages)
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
                //api is limited to 4 requests per second
                Thread.Sleep(260);
            }
            return ids;
        }

        /// <summary>
        /// Gets needed attributes for movie by its ID.
        /// </summary>
        public static List<string> GetMovieAttributes(int tmbdId, List<string> attributes)
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
        public static JObject TMDBRequest(string url)
        {
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