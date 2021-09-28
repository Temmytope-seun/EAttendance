using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EAttendance.Models
{
    public class BaseEntity
    {
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Created User")]
        public string UserCreated { get; set; }

        [Display(Name = " Date Modified")]
        public DateTime? DateModified { get; set; }

        [Display(Name = "Modified User")]
        public string UserModified { get; set; }

       
    }
}
