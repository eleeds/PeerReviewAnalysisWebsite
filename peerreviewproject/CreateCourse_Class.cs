using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace peerreviewproject
{                                                   
    public class CreateCourse_Class
    {
        private int CourseProfessor;
        private string CourseDepartment;
        private string CourseNumber;
        private string CourseName;
        private string CourseSemester;
        private int CourseYear;
        public string sqlConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public int CourseProf
        {
            get { return CourseProfessor; }
            set { CourseProfessor = value; }
        }
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

        public string CourseSemester_
        {
            get { return CourseSemester; }
            set { CourseSemester = value; }
        }
        public int CourseYear_
        {
            get { return CourseYear; }
            set { CourseYear = value; }
        }


        public CreateCourse_Class(int ProfessorID, string Dept, string CNumber, string CName, string semester, int year) 
        {
            CourseProf = ProfessorID;
            Coursedept = Dept;
            CourseNum = CNumber;
            CourseName_ = CName;
            CourseSemester_ = semester;
            CourseYear_ = year;
           if(!DoesClassExist(Coursedept,CourseNum,CourseName_))
            {
                CreateCourse();
                CourseAccess();
            }
            

        }
        public void CreateCourse()
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string createCourse_query = "INSERT INTO Course_table ([courseDepartment], [courseNumber], [courseName], [courseSemester], [year]) VALUES(@department, @courseNum, @courseName, @courseSemester, @year)";
                SqlCommand sqlCmd = new SqlCommand(createCourse_query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@department", Coursedept);
                sqlCmd.Parameters.AddWithValue("@courseNum", CourseNum);
                sqlCmd.Parameters.AddWithValue("@courseName", CourseName_);
                sqlCmd.Parameters.AddWithValue("@courseSemester", CourseSemester_);
                sqlCmd.Parameters.AddWithValue("@year", CourseYear_);
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        public void CourseAccess()
        {
            
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();                      
                string userCourseAccess_query = "INSERT INTO Course_access_table ([userID], [courseID], [permissionType]) VALUES(@userID, @courseID, N'Professor')";
                string courseID_query = "Select courseID FROM Course_table WHERE courseDepartment =@department AND courseNumber =@courseNum AND courseName =@courseName";
                SqlCommand courseAccess_sqlCmd = new SqlCommand(userCourseAccess_query, sqlCon);
                SqlCommand courseID_sqlCmd = new SqlCommand(courseID_query, sqlCon);
                courseID_sqlCmd.Parameters.AddWithValue("@department", Coursedept);
                courseID_sqlCmd.Parameters.AddWithValue("@courseNum", CourseNum);
                courseID_sqlCmd.Parameters.AddWithValue("@courseName", CourseName_);

                int ID = Convert.ToInt32(courseID_sqlCmd.ExecuteScalar());

                courseAccess_sqlCmd.Parameters.AddWithValue("@courseID", ID);
                courseAccess_sqlCmd.Parameters.AddWithValue("@userID", CourseProf);
                courseAccess_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();


            }
        }

        public bool DoesClassExist(string dept, string num, string name)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string classExist_query = "SELECT COUNT(1) FROM Course_table WHERE courseDepartment = @department AND courseNumber = @courseNum and courseName = @courseName";
                SqlCommand classExist_sqlCmd = new SqlCommand(classExist_query, sqlCon);
                classExist_sqlCmd.Parameters.AddWithValue("@department", dept);
                classExist_sqlCmd.Parameters.AddWithValue("@courseNum", num);
                classExist_sqlCmd.Parameters.AddWithValue("@courseName", name);

                int count = Convert.ToInt32(classExist_sqlCmd.ExecuteScalar());
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