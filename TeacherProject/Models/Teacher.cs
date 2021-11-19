using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherProject.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherFName { get; set; }
        public string TeacherLName { get; set; }
        public string TaughtClassCode { get; set; }
        public string TaughtClassName { get; set; }
    }
}