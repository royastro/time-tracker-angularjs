using System;
using System.Collections.Generic;

namespace TimeTracker.DataTransferObjects
{
    public class dtoEntryHeader
    {
        public int Task_Id;
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public dtoTask Task { get; set; }
        public ICollection<dtoEntry> Entries { get; set; }
        public decimal TotalHours { get; set; }
        public bool IsTotal { get; set; }
        public int User_Id { get; set; }
    }
}
