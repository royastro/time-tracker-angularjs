using System;
using System.Data.Entity;
using System.Linq;
using TimeTracker.DataAccess;

namespace TimeTracker.DatabaseInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDb();
        }

        private static void CreateDb()
        {
            Database.SetInitializer(new DbInitialiser());
            var db = new TimeTracker.DataAccess.Context();

            Console.Write("Recreating database...");
            var entries = db.Entries.ToList();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

       
    }
}
