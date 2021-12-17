using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherProject.Models
{
    public class Teacher
    {
        //elements required for a new teacher
        public int TeacherId { get; set; }
        public string TeacherFName { get; set; }
        public string TeacherLName { get; set; }
        public string TaughtClassCode { get; set; }
        public string TaughtClassName { get; set; }
        public int EmployeeNum { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; } //had this as a double before, changing to a decimal seems to have alleviated a lot of issues pulling and converting data
    }
}