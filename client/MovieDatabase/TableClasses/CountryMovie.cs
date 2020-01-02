using DBConnect;
using System;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Country-movie objects.
    /// </summary>
    public class CountryMovie : EntityBase
    {
        public int country;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public CountryMovie(int country, int movie) : base()
        {
            this.country = country;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public CountryMovie(int id, int country, int movie) : base(id)
        {
            this.country = country;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class CountryMovieRepository : IRepository<CountryMovie>
    {
        private IConnection connection;
        private static CountryMovieRepository singleton = null;

        public CountryMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new CountryMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private CountryMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(CountryMovie entity)
        {
            connection.Execute(
               $"insert into Country_Movie (country, movie)" +
               $"values (" +
               $"{entity.country}, " +
               $"{entity.movie});");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(CountryMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public CountryMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(CountryMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}