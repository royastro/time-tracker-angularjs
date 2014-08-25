using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TimeTracker.Model;

namespace TimeTracker.DataAccess
{
    public class Context : DbContext
    {
        public Context() : base("TimeTracker")
        {
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<EntryHeader> EntryHeaders { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryHeader>()
                        .Property(h => h.Id)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Entry>()
                        .HasRequired(h => h.EntryHeader)
                        .WithMany(e => e.Entries)
                        .WillCascadeOnDelete();

            modelBuilder.Entity<UserProfile>()
                        .HasKey(u=>u.User_Id)
                        .Property(u => u.User_Id)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
