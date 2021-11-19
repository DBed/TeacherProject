using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherProject.Models;

namespace TeacherProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        //Creates an HTML page displaying the information for all Teachers in the database. 

        [HttpGet]
        public ActionResult List()
        {
            //Help convert DB data into list in the browser
            TeacherDataController Controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = Controller.ListTeachers();
            return View(Teachers);
        }

        [HttpGet]
        //Get: Student/Show/{id}
        //Creates an HTML page displaying the information for a single Teacher.
        [Route("Teacher/Show/{id}")]
        public ActionResult Show(int id)
        {
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeachers = Controller.FindTeacher(id);
            return View(SelectedTeachers);
        }
    }
}