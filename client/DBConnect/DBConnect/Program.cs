using System;

namespace DBConnect
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConnection database = new MSSQL("DESKTOP-I8PHK3E\\MYMSSQLSERVER", "person", "sa", "Password1");
            //IConnection database = new Oracle("localhost", "", "system", "student");
            //IConnection database = new MySQL("", "", "", "");

            Person p1 = new Person(1, "Peter", "Parker", new DateTime(1999, 8, 9));
            Person p2 = new Person(2, "Carol", "Danvers", new DateTime(1960, 4, 24));

            PersonRepository pr = new PersonRepository(database);
            pr.Create(p1);
            pr.Create(p2);
            //pr.Delete(p1);
            p2.DateOfBirth = new DateTime(1961, 4, 24);
            pr.Update(p2);
            Console.WriteLine(pr.GetById(1).ToString());
            Console.WriteLine(pr.GetById(2).ToString());
            Console.ReadKey();
        }
    }
}