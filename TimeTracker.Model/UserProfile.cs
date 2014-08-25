using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Model
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
