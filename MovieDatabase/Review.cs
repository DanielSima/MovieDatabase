using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class Review : EntityBase
    {
        public string desription;
        public DateTime dateCreated;
        public string author;
        public int movie;

        public Review(string desription, DateTime dateCreated, string author, int movie) : base()
        {
            this.desription = desription;
            this.dateCreated = dateCreated;
            this.author = author;
            this.movie = movie;
        }
        public Review(int id, string desription, DateTime dateCreated, string author, int movie) : base(id)
        {
            this.desription = desription;
            this.dateCreated = dateCreated;
            this.author = author;
            this.movie = movie;
        }
    }
    public class ReviewRepository : IRepository<Review>
    {
        private IConnection connection;
        private static ReviewRepository singleton = null;

        public ReviewRepository()
        {
            if (singleton == null)
            {
                singleton = new ReviewRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public ReviewRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Review entity)
        {
            connection.Execute(
                $"insert into Review ([description], date_created, author, movie) " +
                $"values (" +
                $"N'{entity.desription.Replace("'", "''")}', " + 
                $"'{entity.dateCreated.ToString("yyyy-MM-dd")}', " +
                $"N'{entity.author.Replace("'", "''")}', " +
                $"{entity.movie});");
        }

        public void Delete(Review entity)
        {
            throw new NotImplementedException();
        }

        public Review GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Review entity)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetByMovie(int movieId, int amount = 10)
        {
            List<string> returnedData = connection.ExecuteRead($"select top {amount} * from Review where movie={movieId};");
            try
            {
                List<Review> reviews = new List<Review>();
                for (int i = 0; i < returnedData.Count; i += 5)
                {
                    reviews.Add(new Review(
                    int.Parse(returnedData[i]),
                    returnedData[i + 1],
                    DateTime.Parse(returnedData[i + 2]),
                    returnedData[i + 3],
                    int.Parse(returnedData[i + 4])));
                }
                return reviews;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Review GetByAuthor(string author, int movieId)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Review where author=N'{author.Replace("'", "''")}' and movie={movieId};"); //N for unicode
            try
            {
                return new Review(
                    int.Parse(returnedData[0]),
                    returnedData[1],
                    DateTime.Parse(returnedData[2]),
                    returnedData[3],
                    int.Parse(returnedData[4]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
