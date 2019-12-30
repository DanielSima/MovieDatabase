using DBConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase
{
    public class Person : EntityBase
    {
        public int tmdbId;
        public string name;
        public DateTime dateOfBirth;
        public string placeOfBirth;
        public int gender;
        public string photoPath;

        public Person(int tmdbId, string name, DateTime dateOfBirth, string placeOfBirth, int gender, string photoPath) : base()
        {
            this.tmdbId = tmdbId;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.placeOfBirth = placeOfBirth;
            this.gender = gender;
            this.photoPath = photoPath;
        }
        public Person(int id, int tmdbId, string name, DateTime dateOfBirth, string placeOfBirth, int gender, string photoPath) : base(id)
        {
            this.tmdbId = tmdbId;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.placeOfBirth = placeOfBirth;
            this.gender = gender;
            this.photoPath = photoPath;
        }
    }

    public class PersonRepository : IRepository<Person>
    {
        private IConnection connection;
        private static PersonRepository singleton = null;

        public PersonRepository()
        {
            if (singleton == null)
            {
                singleton = new PersonRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "sa", "Password1"));
            }
            this.connection = singleton.connection;
        }

        public PersonRepository(IConnection connection)
        {
            this.connection = connection;
        }

        public void Create(Person entity)
        {
            connection.Execute(
               $"insert into Person (TMDB_ID, [name], date_of_birth, place_of_birth, gender, photo_path) " +
               $"values (" +
               $"{entity.tmdbId}, " +
               $"'{entity.name.Replace("'", "''")}', " +
               $"'{entity.dateOfBirth.ToString("yyyy-MM-dd")}', " +
               $"'{entity.placeOfBirth.Replace("'", "''")}', " +
               $"{entity.gender}, " +
               $"'{entity.photoPath}');");
        }

        public void Delete(Person entity)
        {
            connection.Execute($"delete from Person where id={entity.Id};");
        }

        public Person GetById(int id)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Person where id={id};");
            try
            {
                return new Person(
                    int.Parse(returnedData[0]),
                    int.Parse(returnedData[1]),
                    returnedData[2],
                    DateTime.Parse(returnedData[3]),
                    returnedData[4],
                    int.Parse(returnedData[5]),
                    returnedData[6]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Update(Person entity)
        {
            connection.Execute(
                $"update Person set " +
                $"TMDB_ID = {entity.tmdbId}, " +
                $"[name] = '{entity.name}', " +
                $"date_of_birth = '{entity.dateOfBirth.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                $"place_of_birth = '{entity.placeOfBirth}', " +
                $"gender = {entity.gender}, " +
                $"photo_path = '{entity.photoPath}");
        }

        public Person GetByName(string name)
        {
            List<string> returnedData = connection.ExecuteRead($"select * from Person where name='{name.Replace("'", "''")}';");
            try
            {
                return new Person(
                    int.Parse(returnedData[0]),
                    int.Parse(returnedData[1]),
                    returnedData[2],
                    DateTime.Parse(returnedData[3]),
                    returnedData[4],
                    int.Parse(returnedData[5]),
                    returnedData[6]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
