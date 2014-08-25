using TimeTracker.Model;

namespace TimeTracker.DataAccess
{
    public interface IUnitOfWork
    {
        void Save();
        GenericRepository<EntryHeader> GetEntryRepository();
        GenericRepository<Task> GetTaskRepository();
        GenericRepository<UserProfile> GetUserProfileRepository();
    }
}
