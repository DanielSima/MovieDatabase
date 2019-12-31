using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class Genre : EntityBase
    {
        public int tmdbId;
        public string title;

        public Genre(int tmdbId, string title) : base()
        {
            this.tmdbId = tmdbId;
            this.title = title;
        }

        public Genre(int id, int tmdbId, string title) : base(id)
        {
            this.tmdbId = tmdbId;
            this.title = title;
        }
    }
    public class GenreRepository : IRepository<Genre>
    {
        private IConnection connection;
        private static GenreRepository singleton = null;

        public GenreRepository()
        {
            if (singleton == null)
            {
                singleton = new GenreRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public GenreRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Genre entity)
        {
            connection.Execute(
                $"insert into Genre (TMDB_ID, title) " +
                $"values (" +
                $"{entity.tmdbId}, " +
                $"'{entity.title.Replace("'", "''")}');");
        }

        public void Delete(Genre entity)
        {
            throw new NotImplementedException();
        }

        public Genre GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Genre entity)
        {
            throw new NotImplementedException();
        }

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
    }
}
