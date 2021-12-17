using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeacherProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;


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
        [Route("api/TeacherData/ListTeachers/{SearchKey}")]
        //Added search function during a puzzling period of C2 to get my brain working again
        public List<Teacher> ListTeachers(string SearchKey=null) 
        {
            //Debug.WriteLine("Attempting to search using " + SearchKey);
            //Create and open connection to School DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();

            //Set up and define query for DB
            MySqlCommand cmd = Conn.CreateCommand();
            string query = "Select * from Teachers where teacherlname like @key";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%" +SearchKey + "%");
            cmd.Prepare();

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
            string query = "Select * from Teachers LEFT JOIN Classes on teachers.teacherid = classes.teacherid where teachers.teacherid=@teacherid";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@teacherid", TeacherId);

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
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                SelectedTeacher.TeacherId = Id;
                SelectedTeacher.TeacherFName = TeacherFName;
                SelectedTeacher.TeacherLName = TeacherLName;
                SelectedTeacher.TaughtClassCode = TaughtClassCode;
                SelectedTeacher.TaughtClassName = TaughtClassName;
                SelectedTeacher.Salary = Salary;
            }
            //close connection between DB and server
            Conn.Close();
            
            //return the selected teacher
            return SelectedTeacher;
        }

        /// <summary>
        /// Adds a new Teacher to the database
        /// </summary>
        /// <param name="NewTeacher">Teacher Object</param>
        public void AddTeacher(Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //SQL query to actually add info to the database in phpMyAdmin
            string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate,  salary) values(@fname, @lname, @employeenumber, @hiredate, @salary) ";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            //match the SQL Query elements with elements from the form in /Teacher/Add
            cmd.Parameters.AddWithValue("@fname", NewTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@lname", NewTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@employeenumber", "T" + NewTeacher.EmployeeNum.ToString()); //Adjusting Employee Number to the TXXX format on the database end
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.Salary);
            
            cmd.ExecuteNonQuery();
            
            Conn.Close();
        }

        /// <summary>
        /// Changes teacher info based on user input, references an Id
        /// </summary>
        /// <param name="SelectedTeacher"> Teacher info (first name, last name)</param>
        
        public void UpdateTeacher(Teacher SelectedTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            string query = "update teachers set teacherfname=@fname, teacherlname=@lname, salary=@salary where teacherid=@id";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@fname", SelectedTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@lname", SelectedTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@id", SelectedTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@salary", SelectedTeacher.Salary);

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Deletes a teacher from the School DB using 
        /// </summary>
        /// <param name="id">TeacherId</param>
        /// This method works on the pre-existing teachers in the database (Sorry, John Taram)
        //I haven't been able to delete one of my created teachers using the method - created teachers have a blank poage when using the 'Show' View
        public void DeleteTeacher(int id)
        {
            //Create and open connection to school DB
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            //Write a query to target all columns for the teacher with the current ID
            string query = "delete from teachers where teacherid = @id";

            //create command and apply the above query
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);

            //run query in DB
            cmd.ExecuteNonQuery();

            Conn.Close();
        }
    }
}
