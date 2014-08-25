using System;

namespace TimeTracker.DataTransferObjects
{
    public class dtoEntry
    {
        public int Id { get; set; }
        public int EntryHeader_Id { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal Hours { get; set; }
        public string Notes { get; set; }
    }
}
