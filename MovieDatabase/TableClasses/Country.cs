using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class Country : EntityBase
    {
        public string isoCode;
        public string name;

        public Country(string isoCode, string name) : base()
        {
            this.isoCode = isoCode;
            this.name = name;
        }

        public Country(int id, string isoCode, string name) : base(id)
        {
            this.isoCode = isoCode;
            this.name = name;
        }
    }
    public class CountryRepository : IRepository<Country>
    {
        private IConnection connection;
        private static CountryRepository singleton = null;

        public CountryRepository()
        {
            if (singleton == null)
            {
                singleton = new CountryRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public CountryRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Country entity)
        {
            connection.Execute(
                $"insert into Country (iso_code, name) " +
                $"values (" +
                $"'{entity.isoCode}', " +
                $"N'{entity.name.Replace("'", "''")}');"); //N for unicode
        }

        public void Delete(Country entity)
        {
            throw new NotImplementedException();
        }

        public Country GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Country entity)
        {
            throw new NotImplementedException();
        }

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
