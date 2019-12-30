using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class ActorMovie : EntityBase
    {
        public string character;
        public int person;
        public int movie;

        public ActorMovie(string character, int person, int movie) : base()
        {
            this.character = character;
            this.person = person;
            this.movie = movie;
        }

        public ActorMovie(int id, string character, int person, int movie) : base(id)
        {
            this.character = character;
            this.person = person;
            this.movie = movie;
        }
    }

    public class ActorMovieRepository : IRepository<ActorMovie>
    {
        private IConnection connection;
        private static ActorMovieRepository singleton = null;

        public ActorMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new ActorMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public ActorMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(ActorMovie entity)
        {
            connection.Execute(
               $"insert into Actor_Movie ([character], person, movie)" +
               $"values (" +
               $"'{entity.character.Replace("'", "''")}', " +
               $"{entity.person}, " +
               $"{entity.movie});");
        }

        public void Delete(ActorMovie entity)
        {
            throw new NotImplementedException();
        }

        public ActorMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ActorMovie entity)
        {
            throw new NotImplementedException();
        }
    }
}
