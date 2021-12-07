using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherProject.Models;

namespace TeacherProject.Controllers
{
    public class SchoolClassController : Controller
    {
        // GET: SchoolClass
        //Displays each of the classes listed in the database
        [HttpGet]
        public ActionResult List()
        {
            //Help convert DB data into list in the browser
            SchoolClassDataController Controller = new SchoolClassDataController();
            IEnumerable<SchoolClass> SchoolClasses = Controller.ListSchoolClasses();
            return View(SchoolClasses);
        }

        //Get: SchoolClass/Show/{id}
        //Displays an HTML page of  a single class and it's elements.
        [HttpGet]
        [Route("SchoolClass/Show/{id}")]
        public ActionResult Show(int id)
        {
            SchoolClassDataController Controller = new SchoolClassDataController();
            SchoolClass SelectedSchoolClass = Controller.FindSchoolClass(id);
            return View(SelectedSchoolClass);
        }

        //GET: SchoolClass/New
        [HttpGet]
        [Route("SchoolClass/New")]

        public ActionResult New()
        {
            return View();
        }
    }
}