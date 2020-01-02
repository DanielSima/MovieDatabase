using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for review objects.
    /// </summary>
    public class Review : EntityBase
    {
        public string desription;
        public DateTime dateCreated;
        public string author;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public Review(string desription, DateTime dateCreated, string author, int movie) : base()
        {
            this.desription = desription;
            this.dateCreated = dateCreated;
            this.author = author;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public Review(int id, string desription, DateTime dateCreated, string author, int movie) : base(id)
        {
            this.desription = desription;
            this.dateCreated = dateCreated;
            this.author = author;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with review objects.
    /// </summary>
    public class ReviewRepository : IRepository<Review>
    {
        private IConnection connection;
        private static ReviewRepository singleton = null;

        public ReviewRepository()
        {
            if (singleton == null)
            {
                singleton = new ReviewRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private ReviewRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
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

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(Review entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public Review GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(Review entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects multiple entities.
        /// </summary>
        public List<Review> GetMultiple(int amount = -1, string joins = "", string where = "",
            string groupBy = "r.ID, r.description, r.date_created, r.author, r.movie", string orderBy = "")
        {
            string query = "select ";
            if (amount > -1) query += $"top {amount} ";
            query += "r.ID, r.description, r.date_created, r.author, r.movie from Review r ";
            if (joins != "") query += $"{joins} ";
            if (where != "") query += $"where {where} ";
            if (groupBy != "") query += $"group by {groupBy} ";
            if (orderBy != "") query += $"order by {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<Review> reviews = new List<Review>();
            try
            {
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

        /// <summary>
        /// Selects entity by Author.
        /// </summary>
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