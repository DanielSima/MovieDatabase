using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MovieDatabase
{
    public static class ImportFromTMDB
    {
        /// <summary>
        /// Saves movies to my DB.
        /// </summary>
        public static void SaveToDB(List<int> movieIds)
        {
            foreach (var movie in movieIds)
            {
                List<string> receivedAttributes = GetMovieAttributes(movie, 
                    new List<string> { "title", "overview", "release_date", "runtime", "vote_average", "poster_path", "budget" });
                string title = receivedAttributes[0];
                string description = receivedAttributes[1];
                DateTime relaseDate = DateTime.Parse(receivedAttributes[2]);
                int runtime = int.Parse(receivedAttributes[3]);
                double rating = double.Parse(receivedAttributes[4]);
                string posterPath = receivedAttributes[5];
                int budget = int.Parse(receivedAttributes[6]);

                new MovieRepository().Create(new Movie(title, description, relaseDate, runtime, rating, posterPath, budget));
            }
        }

        /// <summary>
        /// Gets movie ids from TMDB by popularity.
        /// </summary>
        public static List<int> GetMovieIdsByPopularity(int amount)
        {
            List<int> ids = new List<int>();

            int help = amount / 20;
            for (int i = 1; i <= help; i++)
            {
                var client = new RestClient(
                $"https://" +
                $"api.themoviedb.org/3/discover/movie?page={i}&include_video=false&" +
                $"include_adult=false&sort_by=popularity.desc&language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");
                var request = new RestRequest(Method.GET);
                request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = (JObject)JsonConvert.DeserializeObject(response.Content);
                foreach (var movie in content["results"])
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

            var client = new RestClient("" +
                $"https://" +
                $"api.themoviedb.org/3/movie/{tmbdId}?language=en-US&api_key=6dc57c84bc4a3872ae46d21b6202e6a1");
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var data = (JObject)JsonConvert.DeserializeObject(response.Content);
            foreach (var attribute in attributes)
            {
                values.Add(data[attribute].Value<string>());
            }
            return values;
        }
    }
}