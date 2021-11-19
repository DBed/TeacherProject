using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeacherProject.Models;
using MySql.Data.MySqlClient;

namespace TeacherProject.Controllers
{
    public class SchoolClassDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();
        /// <summary>
        /// Sends a call to the school database and collects the Class name, Class ID and Class code of each class in the DB.
        /// </summary>
        /// <returns>Full list of student elements in XML format</returns>
        [HttpGet]
        public List<SchoolClass> ListSchoolClasses()
        {
            //Create and open connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Classes";

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create list template into which Classes can be inserted
            List<SchoolClass> SchoolClasses = new List<SchoolClass> { };

            //go through each row of the query result
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassName = ResultSet["classname"].ToString();
                string ClassCode = ResultSet["classcode"].ToString();

                //Found the naming got a bit confusing here.  I didn't want to just use 'class' as the name for a controller but when this section came around it felt muddy. 
                SchoolClass NewSchoolClass = new SchoolClass();
                NewSchoolClass.SchoolClassId = ClassId;
                NewSchoolClass.SchoolClassName = ClassName;
                NewSchoolClass.SchoolClassCode = ClassCode;

                //add each class to the list
                SchoolClasses.Add(NewSchoolClass);
            }

            //close DB<->server connection
            Conn.Close();

            //return complete class list
            return SchoolClasses;
        }

        /// <summary>
        /// Accepts an integer "id" and returns information about a single class.
        /// </summary>
        /// <param name="ClassId"></param>
        /// <returns>Class name, class code and class Id of a single class</returns>
        [HttpGet]
        [Route("api/SchoolClassData/FindSchoolClass/{SchoolClassId}")]
        public SchoolClass FindSchoolClass(int SchoolClassId) 
        {
            //Create connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from classes where classid=" + SchoolClassId;

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create a variable in which to store the current class
            SchoolClass SelectedSchoolClass = new SchoolClass();

            //go through each row of the query result
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassName = ResultSet["classname"].ToString();
                string ClassCode = ResultSet["classcode"].ToString();

                SelectedSchoolClass.SchoolClassId = ClassId;
                SelectedSchoolClass.SchoolClassName = ClassName;
                SelectedSchoolClass.SchoolClassCode = ClassCode;
            }

            //close connection between DB and server
            Conn.Close();

            //return the selected class
            return SelectedSchoolClass;
        }

    }
}
