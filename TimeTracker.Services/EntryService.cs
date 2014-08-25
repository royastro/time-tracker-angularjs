using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using TimeTracker.DataAccess;
using TimeTracker.DataTransferObjects;
using TimeTracker.Model;

namespace TimeTracker.Services
{
    public class EntryService : IEntryService
    {
        //TODO: Perform validation and return error messages

        private IUnitOfWork unitOfWork;
        private EntityMapper mapper = new EntityMapper();

        public EntryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper.SetupEntityToDataTransferObjectMappings();
            mapper.SetupDataTranferObjectMappingsToEntity();
        }

        public IList<dtoEntryHeader> GetEntries(DateTime startDate, DateTime endDate, int userId)
        {
            var entries = unitOfWork.GetEntryRepository()
                                    .Get(e => e.WeekStartDate >= startDate 
                                              && e.WeekStartDate <= endDate 
                                              && e.User_Id == userId
                                        ).ToList();

            List<dtoEntryHeader> result = Mapper.Map<List<EntryHeader>, List<dtoEntryHeader>>(entries);
            DateTime tempDate;

            // fill dates with blank entries
            foreach (dtoEntryHeader header in result)
            {
                List<dtoEntry> blankEntries = new List<dtoEntry>();
                tempDate = startDate;
                while (tempDate <= endDate)
                {
                    var entry = header.Entries.SingleOrDefault(e => e.EntryDate == tempDate);
                    if (entry == null)
                    {
                        blankEntries.Add(new dtoEntry()
                        {
                            EntryDate = tempDate,
                        });
                    }
                    tempDate = tempDate.AddDays(1);
                }

                foreach (var blankEntry in blankEntries)
                {
                    header.Entries.Add(blankEntry);
                }
                header.Entries = header.Entries.OrderBy(e => e.EntryDate).ToList();
                // calculate total line hours
                header.TotalHours = header.Entries.Sum(e => e.Hours);
            }

            // create total hours row
            dtoEntryHeader totalHeader = new dtoEntryHeader()
            {
                IsTotal = true,
                Entries = new Collection<dtoEntry>()
            };

            Decimal grandTotal = 0;
            tempDate = startDate;
            while (tempDate <= endDate)
            {
                List<decimal> listTempTotals = new List<decimal>();
                result.ForEach(p => listTempTotals.Add(p.Entries.Where(e => e.EntryDate == tempDate).Sum(s => s.Hours)));
                tempDate = tempDate.AddDays(1);

                var totalEntry = new dtoEntry
                {
                    EntryDate = tempDate,
                    Hours = listTempTotals.Sum()
                };
                totalHeader.Entries.Add(totalEntry);
                grandTotal += totalEntry.Hours;
            }
            totalHeader.Entries.Add(new dtoEntry(){ Hours = grandTotal });
            result.Add(totalHeader);

            return result;
        }

        public string CreateEntryHeader(dtoEntryHeader entryHeader)
        {
            string output = "";

            try
            {
                if (!HeaderAlreadyExist(entryHeader))
                {
                    var newEntryHeader = Mapper.Map<dtoEntryHeader, EntryHeader>(entryHeader);

                    unitOfWork.GetEntryRepository().Insert(newEntryHeader);
                    unitOfWork.Save();
                }
                else
                {
                    output = "The selected task already exists in for the selected date/week.";
                }
            }
            catch (Exception exc)
            {
                output = exc.Message;
            }
            return output;
        }

        private bool HeaderAlreadyExist(dtoEntryHeader entryHeader)
        {
            var result = unitOfWork.GetEntryRepository()
                                   .Get(e => 
                                       e.WeekStartDate == entryHeader.WeekStartDate 
                                       && e.Task_Id == entryHeader.Task_Id
                                       && e.User_Id == entryHeader.User_Id)
                                   .FirstOrDefault();
            if (result == null)
                return false;

            return true;
        }

        public bool DeleteEntryHeader(int entryHeaderId)
        {
            try
            {
                unitOfWork.GetEntryRepository().Delete(entryHeaderId);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateEntry(dtoEntry entry)
        {
            try
            {
                var newEntry = Mapper.Map<dtoEntry, Entry>(entry);

                var entryHeader = unitOfWork.GetEntryRepository().GetById(newEntry.EntryHeader_Id);
                entryHeader.Entries.Add(newEntry);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdateEntry(dtoEntry entry)
        {
            try
            {
                var entryHeader = unitOfWork.GetEntryRepository().GetById(entry.EntryHeader_Id);
                var item = entryHeader.Entries.SingleOrDefault(e => e.Id == entry.Id);

                if (item != null)
                {
                    item.Hours = entry.Hours;
                    item.Notes = entry.Notes;
                }

                unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public List<dtoGridColumn> GetGridColumns(DateTime startDate)
        {
            var columns = new List<dtoGridColumn>();
            for (int ctr = 1; ctr <= 7; ctr++)
            {
                columns.Add(new dtoGridColumn() { Date = startDate, Day = startDate.DayOfWeek.ToString().Substring(0, 3) });
                startDate = startDate.AddDays(1);
            }

            return columns;
        }

        public List<string> GetTaskList(int userId)
        {
            var tasks = unitOfWork.GetTaskRepository()
                            .Get()
                            .Where(t=>t.User_Id == userId)
                            .Select(t=>t.Name)
                            .ToList();
            return tasks;
        } 

        public dtoTask GetTask(string taskName, int userId)
        {
            var task = unitOfWork.GetTaskRepository()
                                 .Get()
                                 .FirstOrDefault(t => t.Name == taskName && t.User_Id == userId);
            if (task != null)
                return Mapper.Map<Task, dtoTask>(task);
            
            return null;
        }
    }
}
