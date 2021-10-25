using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace peerreviewproject
{
    public class StudentTo_Class
    {
        
        
        public void courseInfo()
        {
            int ID = 0;
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query2 = "INSERT INTO Course_access_table ([userID], [courseID], [permissionType]) VALUES(@userID, @courseID, N'Student')";
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
    }
}