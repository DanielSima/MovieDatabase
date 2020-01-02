using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for genre objects.
    /// </summary>
    public class Genre : EntityBase
    {
        public int tmdbId;
        public string title;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public Genre(int tmdbId, string title) : base()
        {
            this.tmdbId = tmdbId;
            this.title = title;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public Genre(int id, int tmdbId, string title) : base(id)
        {
            this.tmdbId = tmdbId;
            this.title = title;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class GenreRepository : IRepository<Genre>
    {
        private IConnection connection;
        private static GenreRepository singleton = null;

        public GenreRepository()
        {
            if (singleton == null)
            {
                singleton = new GenreRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private GenreRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(Genre entity)
        {
            connection.Execute(
                $"insert into Genre (TMDB_ID, title) " +
                $"values (" +
                $"{entity.tmdbId}, " +
                $"'{entity.title.Replace("'", "''")}');");
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(Genre entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public Genre GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(Genre entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by Name.
        /// </summary>
        public Genre GetByName(string title)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Genre where title='{title.Replace("'", "''")}';");
            try
            {
                return new Genre(
                    int.Parse(returnedData[0]),
                    int.Parse(returnedData[1]),
                    returnedData[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Selects multiple entities.
        /// </summary>
        public List<Genre> GetMultiple(int amount = -1, string joins = "", string where = "",
            string groupBy = "g.ID, g.TMDB_ID, g.title", string orderBy = "")
        {
            string query = "select ";
            if (amount > -1) query += $"top {amount} ";
            query += "g.ID, g.TMDB_ID, g.title from Genre g ";
            if (joins != "") query += $"{joins} ";
            if (where != "") query += $"where {where} ";
            if (groupBy != "") query += $"group by {groupBy} ";
            if (orderBy != "") query += $"order by {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<Genre> genres = new List<Genre>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 3)
                {
                    genres.Add(new Genre(
                    int.Parse(returnedData[i]),
                    int.Parse(returnedData[i + 1]),
                    returnedData[i + 2]));
                }
                return genres;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}