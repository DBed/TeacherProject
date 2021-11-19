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
    public class StudentDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();
        /// <summary>
        /// Sends a call to the school database and collects the full name, ID and Student number of the students in the DB.
        /// </summary>
        /// <returns>Full list of student elements in XML format</returns>
        [HttpGet]
        public List<Student> ListStudents()
        {
            //Create and open connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Students";

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create list template into which students can be inserted
            List<Student> Students = new List<Student> { };

            //Go through each row of the query result
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentNum = ResultSet["studentnumber"].ToString();
                string StudentFName = ResultSet["studentfname"].ToString();
                string StudentLName = ResultSet["studentlname"].ToString();

                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentNum = StudentNum;
                NewStudent.StudentFName = StudentFName;
                NewStudent.StudentLName = StudentLName;

                //Add student to the list
                Students.Add(NewStudent);
            }

            //Close connection between DB and server
            Conn.Close();

            //Return complete student list
            return Students;
        }
        /// <summary>
        /// Accepts an integer "id" and returns information about a single student.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns>First name, Last name, Student number and ID for an indiviual student</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{StudentId}")]
        public Student FindStudent(int StudentId)
        {
            //Create connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Students where studentid=" + StudentId;

            //Collect query result in a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create a variable in which to store the current student
            Student SelectedStudent = new Student();

            //Go through each row of the query result
            while (ResultSet.Read())
            {
                int Id = Convert.ToInt32(ResultSet["studentid"]);
                string StudentNum =   ResultSet["studentnumber"].ToString();
                string StudentFName = ResultSet["studentfname"].ToString();
                string StudentLName = ResultSet["studentlname"].ToString();

                SelectedStudent.StudentId = Id;
                SelectedStudent.StudentNum = StudentNum;
                SelectedStudent.StudentFName = StudentFName;
                SelectedStudent.StudentLName = StudentLName;
            }

            //Close connection between DB and server
            Conn.Close();

            //return selected student
            return SelectedStudent;

        }
    }
}
