using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class DirectorMovie : EntityBase
    {
        public int person;
        public int movie;

        public DirectorMovie(int person, int movie) : base()
        {
            this.person = person;
            this.movie = movie;
        }

        public DirectorMovie(int id, int person, int movie) : base(id)
        {
            this.person = person;
            this.movie = movie;
        }
    }

    public class DirectorMovieRepository : IRepository<DirectorMovie>
    {
        private IConnection connection;
        private static DirectorMovieRepository singleton = null;

        public DirectorMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new DirectorMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public DirectorMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(DirectorMovie entity)
        {
            connection.Execute(
               $"insert into Director_Movie (person, movie)" +
               $"values (" +
               $"{entity.person}, " +
               $"{entity.movie});");
        }

        public void Delete(DirectorMovie entity)
        {
            throw new NotImplementedException();
        }

        public DirectorMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(DirectorMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}
