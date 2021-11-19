using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherProject.Models;

namespace TeacherProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/List
        //Displays a full list of students in the browser
        public ActionResult List()
        {
            //Help convert DB data into list in the browser
            StudentDataController Controller = new StudentDataController();
            IEnumerable<Student> Students = Controller.ListStudents();
            return View(Students);
        }

        //Get: Student/Show/{id}
        //Displays an HTML page of the information for a single student.
        [HttpGet]
        [Route("Article/Show/{id}")]
        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();
            Student SelectedStudent = Controller.FindStudent(id);
            return View(SelectedStudent); 
        }
    }
}