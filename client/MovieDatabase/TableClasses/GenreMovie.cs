using DBConnect;
using System;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Genre-Movie objects.
    /// </summary>
    public class GenreMovie : EntityBase
    {
        public int genre;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public GenreMovie(int genre, int movie) : base()
        {
            this.genre = genre;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public GenreMovie(int id, int genre, int movie) : base(id)
        {
            this.genre = genre;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class GenreMovieRepository : IRepository<GenreMovie>
    {
        private IConnection connection;
        private static GenreMovieRepository singleton = null;

        public GenreMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new GenreMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private GenreMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(GenreMovie entity)
        {
            connection.Execute(
               $"insert into Genre_Movie (genre, movie)" +
               $"values (" +
               $"{entity.genre}, " +
               $"{entity.movie});");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(GenreMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public GenreMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(GenreMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}