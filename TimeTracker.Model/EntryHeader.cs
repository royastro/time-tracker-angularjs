using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Model
{
    public class EntryHeader
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public int Task_Id { get; set; }
        [ForeignKey("Task_Id")]
        public virtual Task Task { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public int User_Id { get; set; }
    }
}
