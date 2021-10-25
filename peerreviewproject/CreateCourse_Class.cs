using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace peerreviewproject
{                                                   //need something to keep track of current users ID
    public class CreateCourse_Class
    {
        private string CourseDepartment;
        private string CourseNumber;
        private string CourseName;

        public string Coursedept
        {
            get { return CourseDepartment; }
            set { CourseDepartment = value; }
        }
        public string CourseNum
        {
            get { return CourseNumber; }
            set { CourseNumber = value; }
        }
        public string CourseName_
        {
            get { return CourseName; }
            set { CourseName = value; }
        }

        public CreateCourse_Class(int ProfessorID, string Dept, string CNumber, string CName) 
        {
            Coursedept = Dept;
            CourseNum = CNumber;
            CourseName_ = CName;
           if(!DoesClassExist(Coursedept,CourseNum,CourseName_))
            {
                CreateClass();
                CourseAccess();
            }
            //maybe add date to course table

        }
        public void CreateClass()
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query = "INSERT INTO Course_table ([courseDepartment], [courseNumber], [courseName]) VALUES(@department, @courseNum, @courseName)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@department", Coursedept);
                sqlCmd.Parameters.AddWithValue("@courseNum", CourseNum);
                sqlCmd.Parameters.AddWithValue("@courseName", CourseName_);
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                

            }
        }

        public void CourseAccess()
        {
            int ID = 0;
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();                      //userID needs to be current professor logged in
                string query2 = "INSERT INTO Course_access_table ([userID], [courseID], [permissionType]) VALUES(500, @courseID, N'Professor')";
                string query3 = "Select courseID FROM Course_table WHERE courseDepartment =@department AND courseNumber =@courseNum AND courseName =@courseName";
                SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
                SqlCommand sqlCmd3 = new SqlCommand(query3, sqlCon);
                sqlCmd3.Parameters.AddWithValue("@department", Coursedept);
                sqlCmd3.Parameters.AddWithValue("@courseNum", CourseNum);
                sqlCmd3.Parameters.AddWithValue("@courseName", CourseName_);
                ID = Convert.ToInt32(sqlCmd3.ExecuteScalar());
                //need to grab courseID from course table
                sqlCmd2.Parameters.AddWithValue("@courseID", ID);
                sqlCmd2.ExecuteNonQuery();
                sqlCon.Close();


            }
        }

        public bool DoesClassExist(string dept, string num, string name)
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query = "SELECT COUNT(1) FROM Course_table WHERE courseDepartment = @department AND courseNumber = @courseNum and courseName = @courseName";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@department", dept);
                sqlCmd.Parameters.AddWithValue("@courseNum", num);
                sqlCmd.Parameters.AddWithValue("@courseName", name);

                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                sqlCon.Close();

                if (count == 1)
                {
                    return true;
                }
                return false;
            }
        }

    }
}