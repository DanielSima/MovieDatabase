using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DBConnect
{
    //not updated, see MSSQL for current version
    /// <summary>
    /// Connection to the MySQL DBMS.
    /// </summary>
    internal class MySQL : IConnection
    {
        private MySqlConnection connection;
        private MySqlTransaction transaction;

        /// <summary>
        /// Creates object and executes the Connect method.
        /// </summary>
        public MySQL(string server, string db, string username, string password)
        {
            Connect(server, db, username, password);
        }

        /// <summary>
        /// Connect to a database.
        /// </summary>
        /// <param name="server">server address</param>
        /// <param name="db">database name</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        public void Connect(string server, string db, string username, string password)
        {
            string connectionString = "server=" + server + ";user=" + username + ";database=" + db +
                ";port=3306" + ";password=" + password + "";

            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Executes any command, doesn't check for any returned data.
        /// </summary>
        /// <param name="command">the query to execute</param>
        public void Execute(string command)
        {
            MySqlCommand sql = new MySqlCommand(command);
            sql.Connection = connection;
            try
            {
                sql.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Executes any command, expects returned data.
        /// </summary>
        /// <param name="command">the query to execute</param>
        /// <returns>returned data</returns>
        public List<string> ExecuteRead(string command)
        {
            MySqlCommand sql = new MySqlCommand(command);
            sql.Connection = connection;
            MySqlDataReader dataReader = sql.ExecuteReader();
            List<string> returnedData = new List<string>();
            try
            {
                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        returnedData.Add(dataReader[i].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            dataReader.Close();
            return returnedData;
        }

        /// <summary>
        /// Begins transaction.
        /// </summary>
        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        /// <summary>
        /// Rollbacks transaction.
        /// </summary>
        public void Rollback()
        {
            if (transaction is null) { throw new NullReferenceException("You have to begin the transaction first."); }
            transaction.Rollback();
        }


        /// <summary>
        /// Commits transaction.
        /// </summary>
        public void Commit()
        {
            if (transaction is null) { throw new NullReferenceException("You have to begin the transaction first."); }
            transaction.Commit();
        }
    }
}