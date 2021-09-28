using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAttendance.Models
{
    public class Staff : BaseEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Staff Id")]
        public int StaffId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Z a-z.-]*$", ErrorMessage = "Alphabets only.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "SurName")]
        [RegularExpression(@"^[A-Z a-z.-]*$", ErrorMessage = "Alphabets only.")]
        public string Surname { get; set; }

        [Display(Name = "Other Names")]
        [RegularExpression(@"^[A-Z a-z.-]*$", ErrorMessage = "Alphabets only.")]
        public string Othernames { get; set; }
        public string Department { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public string StaffNo { get; set; }

        [Display(Name = "Active Staffs")]
        public bool ActiveStaff { get; set; }

        [Display(Name = "Phone No.")]
        [Phone]
        public string PhoneNo { get; set; }

        [Display(Name = "Upload Passport")]
        public string ImageUrl { get; set; }

        public string Sex { get; set; }

        public string Qualification { get; set; }

        public string FullName
        {
            get { return Surname + ", " + FirstName + " " + Othernames; }
        }
        
       
        public ICollection<InstructorCourse> InstructorCourse { get; set; }
      
    }


}
