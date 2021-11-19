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
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Sends a call to the school database and collects the full name, ID and class taught of each teacher in the DB.
        /// </summary>
        /// <returns>Full list of teacher elements in XML format</returns>
        [HttpGet]
        public List<Teacher> ListTeachers()
        {
            //Create and open connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Teachers";

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create list template into which Teachers can be inserted
            List<Teacher> Teachers = new List<Teacher> { };

            //Go through each row of the query result
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                
                //Add each teacher to the list
                Teachers.Add(NewTeacher);
            }

            //close DB<->server connection
            Conn.Close();
            //return complete teacher list
            return Teachers;
        }
        /// <summary>
        /// Accepts an integer "id" and returns information about a single teacher.
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <returns>First name, Last name, TeacherId and Class taught by a single teacher.</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{TeacherId}")]
        public Teacher FindTeacher(int TeacherId)
        {
            //Create connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            //Trying to pull multiple "classes taught" from each teacher here but not sure what I'm missing. 
            //I was thinking of making a foreach loop inside the while loop to make a list of the classes, however I'm not sure how to do that exactly. 
            cmd.CommandText = "Select * from Teachers INNER JOIN Classes on teachers.teacherid = classes.teacherid where teachers.teacherid=" + TeacherId;

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create a variable in which to store the current teacher
            Teacher SelectedTeacher = new Teacher();

            //go through each row of the query result
            while (ResultSet.Read())
            {
                int Id = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string TaughtClassName = ResultSet["classname"].ToString();
                string TaughtClassCode = ResultSet["classcode"].ToString();

                SelectedTeacher.TeacherId = Id;
                SelectedTeacher.TeacherFName = TeacherFName;
                SelectedTeacher.TeacherLName = TeacherLName;
                SelectedTeacher.TaughtClassCode = TaughtClassCode;
                SelectedTeacher.TaughtClassName = TaughtClassName;
            }
            //close connection between DB and server
            Conn.Close();
            
            //return the selected teacher
            return SelectedTeacher;
        }
    }
}
