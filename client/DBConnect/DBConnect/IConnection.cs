using System.Collections.Generic;

namespace DBConnect
{
    /// <summary>
    /// An interface for database management systems.
    /// </summary>
    public interface IConnection
    {
        void Connect(string server, string db, string username, string password);

        void Execute(string command);

        List<string> ExecuteRead(string command);

        void BeginTransaction();

        void Rollback();

        void Commit();

    }
}