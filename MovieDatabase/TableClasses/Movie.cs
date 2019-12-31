using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBConnect;

namespace MovieDatabase
{
    public class Movie : EntityBase
    {
        public int tmdbId;
        public string title;
        public string description;
        public DateTime releaseDate;
        public int runtime;
        public double rating;
        public string posterPath;
        public int budget;

        public Movie(int tmdbId, string title, string description, DateTime releaseDate, int runtime, double rating, string posterPath, int budget)
        {
            this.tmdbId = tmdbId;
            this.title = title;
            this.description = description;
            this.releaseDate = releaseDate;
            this.runtime = runtime;
            this.rating = rating;
            this.posterPath = posterPath;
            this.budget = budget;
        }

        public Movie(int id, int tmdbId, string title, string description, DateTime releaseDate, int runtime, double rating, string posterPath, int budget) : base(id)
        {
            this.tmdbId = tmdbId;
            this.title = title;
            this.description = description;
            this.releaseDate = releaseDate;
            this.runtime = runtime;
            this.rating = rating;
            this.posterPath = posterPath;
            this.budget = budget;
        }
    }

    public class MovieRepository : IRepository<Movie>
    {
        private IConnection connection;
        private static MovieRepository singleton = null;

        public MovieRepository()
        {
            if (singleton == null)
            {
                singleton = new MovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public MovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Movie entity)
        {
            connection.Execute(
               $"insert into Movie (TMDB_ID, title, [description], release_date, runtime, rating, poster_path, budget) " +
               $"values (" +
               $"{entity.tmdbId}, " +
               $"'{entity.title.Replace("'", "''")}', " +
               $"'{entity.description.Replace("'", "''")}', " +
               $"'{entity.releaseDate.ToString("yyyy-MM-dd")}', " +
               $"{entity.runtime}, " +
               $"{entity.rating}, " +
               $"'{entity.posterPath}', " +
               $"{entity.budget});");
        }

        public void Delete(Movie entity)
        {
            connection.Execute($"delete from movie where id={entity.Id};");
        }

        public Movie GetById(int id)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Movie where id={id};");
            try
            {
                return new Movie(
                    int.Parse(returnedData[0]),
                    int.Parse(returnedData[1]),
                    returnedData[2], 
                    returnedData[3], 
                    DateTime.Parse(returnedData[4]), 
                    int.Parse(returnedData[5]),
                    double.Parse(returnedData[6]), 
                    returnedData[7], 
                    int.Parse(returnedData[8]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Update(Movie entity)
        {
            connection.Execute(
                $"update movie set " +
                $"TMDB_ID = {entity.tmdbId}, " +
                $"title = '{entity.title}', " +
                $"[description] = '{entity.description}', " +
                $"release_date = '{entity.releaseDate.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                $"runtime = {entity.runtime}, " +
                $"rating = {entity.rating}, " +
                $"poster_path = '{entity.posterPath}', " +
                $"budget = {entity.budget}");

        }

        public List<Movie> GetMultiple(int amount = 0, string where = "", string orderBy = "")
        {
            string query = "select ";
            if (amount > 0) query += $"top {amount} ";
            query += "* from Movie ";
            if (where != "") query += $"where {where} ";
            if (orderBy != "") query += $"orderby {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<Movie> movies = new List<Movie>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 9)
                {
                    movies.Add(new Movie(
                    int.Parse(returnedData[i]),
                    int.Parse(returnedData[i + 1]),
                    returnedData[i + 2],
                    returnedData[i + 3],
                    DateTime.Parse(returnedData[i + 4]),
                    int.Parse(returnedData[i + 5]),
                    double.Parse(returnedData[i + 6]),
                    returnedData[i + 7],
                    int.Parse(returnedData[i + 8])));
                }
                return movies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<Movie> ExecuteProcedure(string name, List<string> variables)
        {
            string query = $"execute {name}";
            for(int i = 0; i < variables.Count; i++)
            {
                query += $" {variables[i]}";
                if(i + 1 < variables.Count)
                    query += ",";
            }
            List<string> returnedData = connection.ExecuteRead(query);
            List<Movie> movies = new List<Movie>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 9)
                {
                    movies.Add(new Movie(
                    int.Parse(returnedData[i]),
                    int.Parse(returnedData[i + 1]),
                    returnedData[i + 2],
                    returnedData[i + 3],
                    DateTime.Parse(returnedData[i + 4]),
                    int.Parse(returnedData[i + 5]),
                    double.Parse(returnedData[i + 6]),
                    returnedData[i + 7],
                    int.Parse(returnedData[i + 8])));
                }
                return movies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
