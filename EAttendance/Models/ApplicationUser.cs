using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EAttendance.Models
{
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression("^[a-z- .A-Z]*$", ErrorMessage = "Only Alphabets allowed")]
        public string FirstName { get; set; }

        [RegularExpression("^[a-z- .A-Z]*$", ErrorMessage = "Only Alphabets allowed")]
        public string LastName { get; set; }
        public string SchoolName { get; set; }
        
        
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
