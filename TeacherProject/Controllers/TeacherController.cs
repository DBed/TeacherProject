using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherProject.Models;
using System.Diagnostics;

namespace TeacherProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        //Creates an HTML page displaying the information for all Teachers in the database. 

        [HttpGet]
        public ActionResult List(string SearchKey)
        {
            //Help convert DB data into list in the browser
            TeacherDataController Controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = Controller.ListTeachers(SearchKey); 
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

        //GET: Teacher/Add
        [HttpGet]
        [Route("Teacher/Add")]
        public ActionResult Add()
        {
            return View();
        }

        //POST: Teacher/Create
        [HttpPost]
        [Route("Teacher/Create")]
        public ActionResult Create(string FirstName, string LastName, int EmployeeNumber, DateTime HireDate, double Salary)
        {
            //Posts message in Visual Studio console if the request goes through
            Debug.WriteLine("Attempt to create teacher with First Name: " + FirstName
                + " and Last Name: " + LastName + " With a salary of: " + Salary + " and an Employee Number of: " + EmployeeNumber);

            //Create new teacher profile
            TeacherDataController Controller = new TeacherDataController();

            //Elements to be incorporated in teacher profile and added to database
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFName = FirstName;
            NewTeacher.TeacherLName = LastName;
            NewTeacher.EmployeeNum = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;


            Controller.AddTeacher(NewTeacher);

            //Return to list of teachers
            return RedirectToAction("List");
        }

        //GET: DeleteConfirmation/{id}
        public ActionResult DeleteConfirmation(int id)
        {
            //Fetch teacher info (id) before delete confirmation goes through 
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            //pull up info page for single teacher
            return View(SelectedTeacher);
        }
        //POST: Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController Controller = new TeacherDataController();
            Controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }
    }
}