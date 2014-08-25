using System;
using System.Collections.Generic;
using TimeTracker.DataTransferObjects;

namespace TimeTracker.Services
{
    public interface IEntryService
    {
        IList<dtoEntryHeader> GetEntries(DateTime startDate, DateTime endDate, int userId);
        string CreateEntryHeader(dtoEntryHeader entryHeader);
        bool DeleteEntryHeader(int entryHeaderId);
        bool CreateEntry(dtoEntry entry);
        bool UpdateEntry(dtoEntry entry);
        List<dtoGridColumn> GetGridColumns(DateTime startDate);
        List<string> GetTaskList(int userId);
        dtoTask GetTask(string taskName, int userId);
    }
}
