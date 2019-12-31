using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class GenreMovie : EntityBase
    {
        public int genre;
        public int movie;

        public GenreMovie(int genre, int movie) : base()
        {
            this.genre = genre;
            this.movie = movie;
        }

        public GenreMovie(int id, int genre, int movie) : base(id)
        {
            this.genre = genre;
            this.movie = movie;
        }
    }

    public class GenreMovieRepository : IRepository<GenreMovie>
    {
        private IConnection connection;
        private static GenreMovieRepository singleton = null;

        public GenreMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new GenreMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public GenreMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(GenreMovie entity)
        {
            connection.Execute(
               $"insert into Genre_Movie (genre, movie)" +
               $"values (" +
               $"{entity.genre}, " +
               $"{entity.movie});");
        }

        public void Delete(GenreMovie entity)
        {
            throw new NotImplementedException();
        }

        public GenreMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(GenreMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}
