using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class LanguageMovie : EntityBase
    {
        public int language;
        public int movie;

        public LanguageMovie(int language, int movie) : base()
        {
            this.language = language;
            this.movie = movie;
        }

        public LanguageMovie(int id, int language, int movie) : base(id)
        {
            this.language = language;
            this.movie = movie;
        }
    }

    public class LanguageMovieRepository : IRepository<LanguageMovie>
    {
        private IConnection connection;
        private static LanguageMovieRepository singleton = null;

        public LanguageMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new LanguageMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public LanguageMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(LanguageMovie entity)
        {
            connection.Execute(
               $"insert into Language_Movie (language, movie)" +
               $"values (" +
               $"{entity.language}, " +
               $"{entity.movie});");
        }

        public void Delete(LanguageMovie entity)
        {
            throw new NotImplementedException();
        }

        public LanguageMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(LanguageMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}
