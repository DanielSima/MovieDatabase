using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBConnect;

namespace MovieDatabase
{
    public class Movie : EntityBase
    {
        public string title;
        public string description;
        public DateTime releaseDate;
        public int runtime;
        public double rating;
        public string posterPath;
        public int budget;

        public Movie(string title, string description, DateTime releaseDate, int runtime, double rating, string posterPath, int budget)
        {
            this.title = title;
            this.description = description;
            this.releaseDate = releaseDate;
            this.runtime = runtime;
            this.rating = rating;
            this.posterPath = posterPath;
            this.budget = budget;
        }

        public Movie(int id, string title, string description, DateTime releaseDate, int runtime, double rating, string posterPath, int budget) : base(id)
        {
            this.title = title;
            this.description = description;
            this.releaseDate = releaseDate;
            this.runtime = runtime;
            this.rating = rating;
            this.posterPath = posterPath;
            this.budget = budget;
        }

        /*public static void Test()
        {
            MovieRepository.GetMovieRepository().Create(new Movie("Iron man", new DateTime(2008, 12, 12)));
            MovieRepository.GetMovieRepository().Create(new Movie("Doctor strange", new DateTime(2016, 1, 12)));
            MovieRepository.GetMovieRepository().Create(new Movie("Black Widow", new DateTime(2020, 3, 1)));
        }*/
        /*
        public static List<Movie> movies = new List<Movie> { 
            new Movie(1, "Iron man", new DateTime(2008, 12, 12)),
            new Movie(2, "Doctor strange", new DateTime(2016, 1, 12)),
            new Movie(3, "Black Widow", new DateTime(2020, 3, 1))};*/
    }

    public class MovieRepository : IRepository<Movie>
    {
        private IConnection connection;
        private static MovieRepository singleton = null;

        public MovieRepository()
        {
            if (singleton == null)
            {
                singleton = new MovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "MovieDatabase", "sa", "Password1"));
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
               $"insert into Movie (title, [description], release_date, runtime, rating, poster_path, budget) " +
               $"values (" +
               $"'{entity.title}', " +
               $"'{entity.description}', " +
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
                    returnedData[1], 
                    returnedData[2], 
                    DateTime.Parse(returnedData[3]), 
                    int.Parse(returnedData[4]),
                    double.Parse(returnedData[5]), 
                    returnedData[6], 
                    int.Parse(returnedData[7]));
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
                for (int i = 0; i < returnedData.Count; i += 8)
                {
                    movies.Add(new Movie(
                    int.Parse(returnedData[i]),
                    returnedData[i + 1],
                    returnedData[i + 2],
                    DateTime.Parse(returnedData[i + 3]),
                    int.Parse(returnedData[i + 4]),
                    double.Parse(returnedData[i + 5]),
                    returnedData[i + 6],
                    int.Parse(returnedData[i + 7])));
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
