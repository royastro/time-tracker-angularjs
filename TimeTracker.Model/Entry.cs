using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Model
{
    public class Entry
    {
        public int Id { get; set; }
        public int EntryHeader_Id { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal Hours { get; set; }
        public string Notes { get; set; }
        [ForeignKey("EntryHeader_Id")]
        public virtual EntryHeader EntryHeader { get; set; }
    }
}
