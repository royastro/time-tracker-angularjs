using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Model;

namespace TimeTracker.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private Context context;
        private GenericRepository<EntryHeader> entryRepository;
        private GenericRepository<Task> taskRepository;
        private GenericRepository<UserProfile> userProfileRepository;

        //TODO: Inject Context
        public UnitOfWork()
        {
            this.context = new Context();
        }

        public GenericRepository<EntryHeader> GetEntryRepository()
        {
            if (this.entryRepository == null)
            {
                this.entryRepository = new GenericRepository<EntryHeader>(context);
            }
            return entryRepository;
        }

        public GenericRepository<Task> GetTaskRepository()
        {
            if (this.taskRepository == null)
            {
                this.taskRepository = new GenericRepository<Task>(context);
            }
            return taskRepository;
        }

        public GenericRepository<UserProfile> GetUserProfileRepository()
        {
            if (this.userProfileRepository == null)
            {
                this.userProfileRepository = new GenericRepository<UserProfile>(context);
            }
            return userProfileRepository;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
