using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EAttendance.Models
{
   public class InstructorCourse :BaseEntity
    {
        [Key]
        [Required]
        public int InstructorCourseId { get; set; }
        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }

    public class Course : BaseEntity
    {
        [Key]
        [Required]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CourseUnit { get; set; }

        public string CourseTitle
        {
            get { return CourseName + " (" + CourseCode + ")"; }
        }
    }

    public class Attendance : BaseEntity
    {
        [Key]
        [Required]
        public int AttendanceId { get; set; }
        public int FingerPrintID { get; set; }
        public Student Student { get; set; }
        public string CourseCode { get; set; }
        public Course Course { get; set; }

    }
}
