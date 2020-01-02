using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for language objects.
    /// </summary>
    public class Language : EntityBase
    {
        public string isoCode;
        public string name;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public Language(string isoCode, string name) : base()
        {
            this.isoCode = isoCode;
            this.name = name;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public Language(int id, string isoCode, string name) : base(id)
        {
            this.isoCode = isoCode;
            this.name = name;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class LanguageRepository : IRepository<Language>
    {
        private IConnection connection;
        private static LanguageRepository singleton = null;

        public LanguageRepository()
        {
            if (singleton == null)
            {
                singleton = new LanguageRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private LanguageRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(Language entity)
        {
            connection.Execute(
                $"insert into Language (iso_code, name) " +
                $"values (" +
                $"'{entity.isoCode}', " +
                $"N'{entity.name.Replace("'", "''")}');"); //N for unicode
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(Language entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public Language GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(Language entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by Name.
        /// </summary>
        public Language GetByName(string name)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Language where name=N'{name.Replace("'", "''")}';"); //N for unicode
            try
            {
                return new Language(
                    int.Parse(returnedData[0]),
                    returnedData[1],
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