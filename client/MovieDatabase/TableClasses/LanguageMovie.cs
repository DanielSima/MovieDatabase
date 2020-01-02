using DBConnect;
using System;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Language-Movie objects.
    /// </summary>
    public class LanguageMovie : EntityBase
    {
        public int language;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public LanguageMovie(int language, int movie) : base()
        {
            this.language = language;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public LanguageMovie(int id, int language, int movie) : base(id)
        {
            this.language = language;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with Language-Movie objects.
    /// </summary>
    public class LanguageMovieRepository : IRepository<LanguageMovie>
    {
        private IConnection connection;
        private static LanguageMovieRepository singleton = null;

        public LanguageMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new LanguageMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private LanguageMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(LanguageMovie entity)
        {
            connection.Execute(
               $"insert into Language_Movie (language, movie)" +
               $"values (" +
               $"{entity.language}, " +
               $"{entity.movie});");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(LanguageMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public LanguageMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(LanguageMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}