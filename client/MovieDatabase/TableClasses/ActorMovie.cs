using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Actor-Movie objects.
    /// </summary>
    public class ActorMovie : EntityBase
    {
        public string character;
        public int person;
        public int movie;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public ActorMovie(string character, int person, int movie) : base()
        {
            this.character = character;
            this.person = person;
            this.movie = movie;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public ActorMovie(int id, string character, int person, int movie) : base(id)
        {
            this.character = character;
            this.person = person;
            this.movie = movie;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class ActorMovieRepository : IRepository<ActorMovie>
    {
        private IConnection connection;
        private static ActorMovieRepository singleton = null;

        public ActorMovieRepository()
        {
            if (singleton == null)
            {
                singleton = new ActorMovieRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private ActorMovieRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(ActorMovie entity)
        {
            connection.Execute(
               $"insert into Actor_Movie ([character], person, movie)" +
               $"values (" +
               $"'{entity.character.Replace("'", "''")}', " +
               $"{entity.person}, " +
               $"{entity.movie});");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(ActorMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public ActorMovie GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(ActorMovie entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects multiple entities.
        /// </summary>
        public List<ActorMovie> GetMultiple(int amount = -1, string joins = "", string where = "",
            string groupBy = "am.ID, am.character, am.person, am.movie", string orderBy = "")
        {
            string query = "select ";
            if (amount > -1) query += $"top {amount} ";
            query += "am.ID, am.character, am.person, am.movie from Actor_Movie am ";
            if (joins != "") query += $"{joins} ";
            if (where != "") query += $"where {where} ";
            if (groupBy != "") query += $"group by {groupBy} ";
            if (orderBy != "") query += $"order by {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<ActorMovie> actorMovies = new List<ActorMovie>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 4)
                {
                    actorMovies.Add(new ActorMovie(
                    int.Parse(returnedData[i]),
                    returnedData[i + 1],
                    int.Parse(returnedData[i + 2]),
                    int.Parse(returnedData[i + 3])));
                }
                return actorMovies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}