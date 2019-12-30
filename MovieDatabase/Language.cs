using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class Language : EntityBase
    {
        public string isoCode;
        public string name;

        public Language(string isoCode, string name) : base()
        {
            this.isoCode = isoCode;
            this.name = name;
        }

        public Language(int id, string isoCode, string name) : base(id)
        {
            this.isoCode = isoCode;
            this.name = name;
        }
    }
    public class LanguageRepository : IRepository<Language>
    {
        private IConnection connection;
        private static LanguageRepository singleton = null;

        public LanguageRepository()
        {
            if (singleton == null)
            {
                singleton = new LanguageRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public LanguageRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Language entity)
        {
            connection.Execute(
                $"insert into Language (iso_code, name) " +
                $"values (" +
                $"'{entity.isoCode}', " +
                $"N'{entity.name.Replace("'", "''")}');"); //N for unicode
        }

        public void Delete(Language entity)
        {
            throw new NotImplementedException();
        }

        public Language GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Language entity)
        {
            throw new NotImplementedException();
        }

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
