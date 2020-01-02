using System;
using System.Collections.Generic;

namespace DBConnect
{
    /// <summary>
    /// Sample implementation of the IRepository interface.
    /// </summary>
    public class PersonRepository : IRepository<Person>
    {
        private IConnection connection;

        public PersonRepository(IConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Inserts an object into the database.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        public void Create(Person entity)
        {
            connection.Execute(
                "insert into person (id, first_name, last_name, date_of_birth) " +
                "values (" + entity.Id + ", '" + entity.FirstName + "', '" + entity.LastName + "', '" + entity.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss") + "');");
        }

        /// <summary>
        /// Deletes an object from the database.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        public void Delete(Person entity)
        {
            connection.Execute("delete from person where id=" + entity.Id + ";");
        }

        /// <summary>
        /// Finds an object by ID.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        public Person GetById(int id)
        {
            List<string> returnedData = connection.ExecuteRead("select * from person where id=" + id + ";");
            try
            {
                return new Person(int.Parse(returnedData[0]), returnedData[1], returnedData[2], DateTime.Parse(returnedData[3]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Person(-1);
            }
        }

        /// <summary>
        /// Updates an object in the database.
        /// </summary>
        /// <param name="entity">Object to update.</param>
        public void Update(Person entity)
        {
            connection.Execute(
                "update person set " +
                "first_name = '" + entity.FirstName + "', " +
                "last_name = '" + entity.LastName + "', " +
                "date_of_birth = '" + entity.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "where id = " + entity.Id + ";");
        }
    }
}