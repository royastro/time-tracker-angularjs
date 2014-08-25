using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Security;
using TimeTracker.Model;
using WebMatrix.WebData;

namespace TimeTracker.DataAccess
{
    public class DbInitialiser : DropCreateDatabaseAlways<Context>
    {
        private int userId = 1;

        private void InitializeSimpleMembershipProvider()
        {
            Console.Write("Creating security tables...");
            WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("TimeTracker", "UserProfile", "User_Id", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (membership.GetUser("roy", false) == null)
            {
                membership.CreateUserAndAccount("roy", "password");
            }
            if (!roles.GetRolesForUser("roy").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "roy" }, new[] { "admin" });
            }
            if (membership.GetUser("joe", false) == null)
            {
                membership.CreateUserAndAccount("joe", "password");
            }
            Console.WriteLine("done");
        }

        protected override void Seed(Context context)
        {
            Console.Write("Creating sample Tasks...");
            // create tasks
            IList<Task> tasks = new List<Task>()
                {
                    new Task { Name = "Tempus Development", User_Id = userId},
                    new Task { Name = "Upskilling - HTML5, AngularJs, JavaScript", User_Id = userId},                    
                    new Task { Name = "DTL-CR3A - Split Purchase Orders", User_Id = userId},
                    new Task { Name = "ADM-0001 - Company Meeting", User_Id = userId},
                    new Task { Name = "ADM-0002 - Company Function", User_Id = userId},
                    new Task { Name = "ADM-0003 - Scheduled Training", User_Id = userId},
                    new Task { Name = "ADM-0004 - Downtime", User_Id = userId},
                    new Task { Name = "ADM-0005 - Management Meeting", User_Id = userId}
                };

            foreach (Task task in tasks)
            {
                context.Tasks.Add(task);
            }

            context.SaveChanges();
            Console.WriteLine("done");
            Console.Write("Creating sample Entries...");
            // create entries
            IList<EntryHeader> entryHeaders = new List<EntryHeader>();
            entryHeaders.Add(new EntryHeader
                {
                    WeekStartDate = new DateTime(2013, 9, 16),
                    Task = context.Tasks.Single(t => t.Name == "Tempus Development"),
                    Entries = new Collection<Entry>()
                        {

                            new Entry()
                                {
                                    EntryDate = new DateTime(2013, 9, 16),
                                    Hours = 2.5m,
                                    Notes = "Added filters to Monthly Summary Report"
                                }
                        },
                    User_Id = userId
                });

            entryHeaders.Add(new EntryHeader
                {
                    WeekStartDate = new DateTime(2013, 9, 16),
                    Task = context.Tasks.Single(t=>t.Name == "Upskilling - HTML5, AngularJs, JavaScript"),
                    Entries = new Collection<Entry>()
                        {

                            new Entry()
                                {
                                    EntryDate = new DateTime(2013, 9, 16),
                                    Hours = 3.0m,
                                    Notes = "Gone through AngularJs Pluralsight Videos"
                                }
                        },
                    User_Id = userId
                });

            entryHeaders.Add(new EntryHeader
                {
                    WeekStartDate = new DateTime(2013, 9, 16),
                    Task = context.Tasks.Single(t=>t.Name == "DTL-CR3A - Split Purchase Orders"),
                    Entries = new Collection<Entry>()
                        {

                            new Entry()
                                {
                                    EntryDate = new DateTime(2013, 9, 16),
                                    Hours = 3.0m,
                                    Notes = "Deploy to UAT and UAT development support"
                                }
                        },
                    User_Id = userId
                });

            foreach (EntryHeader entryHeader in entryHeaders)
            {
                context.EntryHeaders.Add(entryHeader);
            }
            Console.WriteLine("done");

            InitializeSimpleMembershipProvider();
            base.Seed(context);
        }
    }
}
