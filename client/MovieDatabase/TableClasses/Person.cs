using DBConnect;
using System;
using System.Collections.Generic;

namespace MovieDatabase
{
    /// <summary>
    /// Class for Person objects.
    /// </summary>
    public class Person : EntityBase
    {
        public int tmdbId;
        public string name;
        public DateTime dateOfBirth;
        public string placeOfBirth;
        public int gender;
        public string photoPath;

        /// <summary>
        /// Constructor for new object which will be written to DB.
        /// </summary>
        public Person(int tmdbId, string name, DateTime dateOfBirth, string placeOfBirth, int gender, string photoPath) : base()
        {
            this.tmdbId = tmdbId;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.placeOfBirth = placeOfBirth;
            this.gender = gender;
            this.photoPath = photoPath;
        }

        /// <summary>
        /// Constructor for object received from DB.
        /// </summary>
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

    /// <summary>
    /// Class for CRUD operations on DB with person objects.
    /// </summary>
    public class PersonRepository : IRepository<Person>
    {
        private IConnection connection;
        private static PersonRepository singleton = null;

        public PersonRepository()
        {
            if (singleton == null)
            {
                singleton = new PersonRepository(new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "Movie_Database", "client", "password"));
            }
            this.connection = singleton.connection;
        }

        private PersonRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
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

        /// <summary>
        /// Deletes entity.
        /// </summary>
        public void Delete(Person entity)
        {
            connection.Execute($"delete from Person where id={entity.Id};");
        }

        /// <summary>
        /// Selects entity by ID.
        /// </summary>
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

        /// <summary>
        /// Updates entity.
        /// </summary>
        public void Update(Person entity)
        {
            connection.Execute(
                $"update Person set " +
                $"TMDB_ID = {entity.tmdbId}, " +
                $"[name] = '{entity.name}', " +
                $"date_of_birth = '{entity.dateOfBirth.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                $"place_of_birth = '{entity.placeOfBirth}', " +
                $"gender = {entity.gender}, " +
                $"photo_path = '{entity.photoPath} " +
                $"where id={entity.Id}");
        }

        /// <summary>
        /// Selects entity by name.
        /// </summary>
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

        /// <summary>
        /// Selects multiple entities.
        /// </summary>
        public List<Person> GetMultiple(int amount = -1, string joins = "", string where = "",
            string groupBy = "p.ID, p.TMDB_ID, p.name, p.date_of_birth, p.place_of_birth, p.gender, p.photo_path", string orderBy = "")
        {
            string query = "select ";
            if (amount > -1) query += $"top {amount} ";
            query += "p.ID, p.TMDB_ID, p.name, p.date_of_birth, p.place_of_birth, p.gender, p.photo_path from Person p ";
            if (joins != "") query += $"{joins} ";
            if (where != "") query += $"where {where} ";
            if (groupBy != "") query += $"group by {groupBy} ";
            if (orderBy != "") query += $"order by {orderBy} ";
            query += ";";
            List<string> returnedData = connection.ExecuteRead(query);
            List<Person> people = new List<Person>();
            try
            {
                for (int i = 0; i < returnedData.Count; i += 7)
                {
                    people.Add(new Person(
                    int.Parse(returnedData[i]),
                    int.Parse(returnedData[i + 1]),
                    returnedData[i + 2],
                    DateTime.Parse(returnedData[i + 3]),
                    returnedData[i + 4],
                    int.Parse(returnedData[i + 5]),
                    returnedData[i + 6]));
                }
                return people;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}