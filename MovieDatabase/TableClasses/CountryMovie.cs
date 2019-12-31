using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class CountryMovie : EntityBase
    {
        public int country;
        public int movie;

        public CountryMovie(int country, int movie) : base()
        {
            this.country = country;
            this.movie = movie;
        }

        public CountryMovie(int id, int country, int movie) : base(id)
        {
            this.country = country;
            this.movie = movie;
        }
    }

    public class CountryMovieRepository : IRepository<CountryMovie>
    {
        private IConnection connection;
        private static CountryMovieRepository singleton = null;

        public CountryMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new CountryMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public CountryMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(CountryMovie entity)
        {
            connection.Execute(
               $"insert into Country_Movie (country, movie)" +
               $"values (" +
               $"{entity.country}, " +
               $"{entity.movie});");
        }

        public void Delete(CountryMovie entity)
        {
            throw new NotImplementedException();
        }

        public CountryMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CountryMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}
