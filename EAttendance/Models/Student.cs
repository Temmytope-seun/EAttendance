using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EAttendance.Models
{
    public class Student : BaseEntity
    {
        [Key]
        [Required]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }


        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName + " " + MiddleName;
            }
        }


        [Display(Name = "Matric No.")]
        public string MatricNo { get; set; }

        public int FingerPrintID { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string Department { get; set; }


        public string ImageUrl { get; set; }
        public string Gender { get; set; }

        
      
        [Display(Name = "Active Students")]
        public bool ActiveStudents { get; set; }

       
    }
}
