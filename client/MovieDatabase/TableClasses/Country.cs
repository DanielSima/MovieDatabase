using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for country objects.
    /// </summary>
    public class Country : EntityBase
    {
        public string isoCode;
        public string name;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public Country(string isoCode, string name) : base()
        {
            this.isoCode = isoCode;
            this.name = name;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
        public Country(int id, string isoCode, string name) : base(id)
        {
            this.isoCode = isoCode;
            this.name = name;
        }
    }

    /// <summary>
    /// Class for CRUD operations on DB with movie objects.
    /// </summary>
    public class CountryRepository : IRepository<Country>
    {
        private IConnection connection;
        private static CountryRepository singleton = null;

        public CountryRepository()
        {
            if (singleton == null)
            {
                singleton = new CountryRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private CountryRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        public void Create(Country entity)
        {
            connection.Execute(
                $"insert into Country (iso_code, name) " +
                $"values (" +
                $"'{entity.isoCode}', " +
                $"N'{entity.name.Replace("'", "''")}');"); //N for unicode
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(Country entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
        public Country GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(Country entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects entity by Name.
        /// </summary>
        public Country GetByName(string name)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Country where name=N'{name.Replace("'", "''")}';"); //N for unicode
            try
            {
                return new Country(
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