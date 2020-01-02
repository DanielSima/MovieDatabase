using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Director-Movie objects.
    /// </summary>
    public class DirectorMovie : EntityBase
    {
        public int person;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public DirectorMovie(int person, int movie) : base()
        {
            this.person = person;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public DirectorMovie(int id, int person, int movie) : base(id)
        {
            this.person = person;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class DirectorMovieRepository : IRepository<DirectorMovie>
    {
        private IConnection connection;
        private static DirectorMovieRepository singleton = null;

        public DirectorMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new DirectorMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private DirectorMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(DirectorMovie entity)
        {
            connection.Execute(
               $"insert into Director_Movie (person, movie)" +
               $"values (" +
               $"{entity.person}, " +
               $"{entity.movie});");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(DirectorMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public DirectorMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(DirectorMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects multiple entities.
        /// </summary>
        public List<DirectorMovie> GetMultiple(int amount = -1, string joins = "", string where = "",
            string groupBy = "dm.ID, dm.person, dm.movie", string orderBy = "")
        {
            string query = "select ";
            if (amount > -1) query += $"top {amount} ";
            query += "dm.ID, dm.person, dm.movie Director_Movie dm ";
            if (joins != "") query += $"{joins} ";
            if (where != "") query += $"where {where} ";
            if (groupBy != "") query += $"group by {groupBy} ";
            if (orderBy != "") query += $"order by {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<DirectorMovie> DirectorMovies = new List<DirectorMovie>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 3)
                {
                    DirectorMovies.Add(new DirectorMovie(
                    int.Parse(returnedData[i]),
                    int.Parse(returnedData[i + 1]),
                    int.Parse(returnedData[i + 2])));
                }
                return DirectorMovies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}