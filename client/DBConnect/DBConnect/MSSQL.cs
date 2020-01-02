using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DBConnect
{
    /// <summary>
    /// Connection to the MSSQL DBMS.
    /// </summary>
    public class MSSQL : IConnection
    {
        public SqlConnection connection;
        public SqlTransaction transaction;

        /// <summary>
        /// Creates object and executes the Connect method.
        /// </summary>
        public MSSQL(string server, string db, string username, string password)
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
            string connectionString = "Data Source=" + server + "; Initial Catalog=" + db + "; User ID=" + username +
                "; Password=" + password + "";

            connection = new SqlConnection(connectionString);
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
            SqlCommand sql = new SqlCommand(command);
            sql.Connection = connection;
            sql.Transaction = transaction;
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
            SqlCommand sql = new SqlCommand(command);
            sql.Connection = connection;
            SqlDataReader dataReader = sql.ExecuteReader();
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