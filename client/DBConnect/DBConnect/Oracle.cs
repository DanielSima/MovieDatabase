using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DBConnect
{
    //not updated, see MSSQL for current version
    /// <summary>
    /// Connection to the Oracle DBMS.
    /// </summary>
    internal class Oracle : IConnection
    {
        private OracleConnection connection;
        private OracleTransaction transaction;

        /// <summary>
        /// Creates object and executes the Connect method.
        /// </summary>
        public Oracle(string server, string db, string username, string password)
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
            string connectionString = "User Id=" + username + ";Password=" + password + ";Data Source=" + server + "";

            connection = new OracleConnection(connectionString);
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
            OracleCommand sql = connection.CreateCommand();
            sql.CommandText = command;
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
            OracleCommand sql = connection.CreateCommand();
            sql.CommandText = command;
            OracleDataReader dataReader = sql.ExecuteReader();
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