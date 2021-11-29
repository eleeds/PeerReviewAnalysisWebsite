using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace peerreviewproject
{
    public class StudentTo_Class
    {
        private int courseID;
        private int userID;
        public string sqlConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public int course_ID
        {
            get { return courseID; }
            set {  courseID= value; }
        }
        public int userID_
        {
            get { return userID; }
            set { userID = value; }
        }

        public StudentTo_Class(string class_ID, string email, string team)
        {
            courseID = Convert.ToInt32(class_ID);

            if (!IsUserInClassAlready(class_ID, email))     //skips student if their already in the class
            {
                courseInfo(courseID, email);
                createTeam(courseID, Convert.ToInt32(team));
                studentToGroup(class_ID, email, Convert.ToInt32(team));
            }
        }

        public StudentTo_Class(string userID, string courseID)
        {
            RemoveFromClass(Convert.ToInt32(userID), Convert.ToInt32(courseID));
        }
        public void courseInfo(int ID, string email)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                                    //query to grab student's userID
                                    //query to place student into class

                sqlCon.Open();
                string StudentToCourse_Query = "INSERT INTO Course_access_table ([userID], [courseID], [permissionType]) VALUES(@userID, @courseID, N'Student')";
                string StudentID_Query = "Select ID FROM User_table WHERE email =@email";
                SqlCommand ToCourseCMD = new SqlCommand(StudentToCourse_Query, sqlCon);
                SqlCommand StudentIDCMD = new SqlCommand(StudentID_Query, sqlCon);

                StudentIDCMD.Parameters.AddWithValue("@email", email);
                int userID = Convert.ToInt32(StudentIDCMD.ExecuteScalar());

                ToCourseCMD.Parameters.AddWithValue("@userID", userID);
                ToCourseCMD.Parameters.AddWithValue("@courseID", ID);
                ToCourseCMD.ExecuteNonQuery();
                sqlCon.Close();
                                    
            }
        }
        public bool IsUserInClassAlready(string classID, string email)
        {
                using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
                {
                    sqlCon.Open();
                    string inClass_query = "SELECT COUNT(1) FROM Course_access_table WHERE userID=@user AND courseID=@courseID";
                    string user_query = "SELECT ID FROM User_table WHERE email=@email";
                    SqlCommand gradID_sqlCmd = new SqlCommand(user_query, sqlCon);
                    gradID_sqlCmd.Parameters.AddWithValue("@email", email);
                    int ID = Convert.ToInt32(gradID_sqlCmd.ExecuteScalar());
                    SqlCommand userExist_sqlCmd = new SqlCommand(inClass_query, sqlCon);
                    userExist_sqlCmd.Parameters.AddWithValue("@user", ID);
                    userExist_sqlCmd.Parameters.AddWithValue("courseID", classID);
                    int check = Convert.ToInt32(userExist_sqlCmd.ExecuteScalar());
                    sqlCon.Close();

                    if (check == 1)
                    {
                        return true;
                    }
                    return false;
                }
            
        }
        public void studentToGroup(string classID, string email, int name)      //student assigned to team in class
        {

            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string toGroup_Query = "INSERT INTO UserTeam_table ([userID], [teamID], [courseID]) VALUES(@userID, @teamID, @courseID)";
                string StudentID_Query = "Select ID FROM User_table WHERE email =@email";
                string teamID_Query = "Select teamID FROM teams_table WHERE name=@name";

                SqlCommand team = new SqlCommand(teamID_Query, sqlCon);
                team.Parameters.AddWithValue("@name", name);
                int teamID = Convert.ToInt32(team.ExecuteScalar());

                SqlCommand StudentIDCMD = new SqlCommand(StudentID_Query, sqlCon);
                StudentIDCMD.Parameters.AddWithValue("@email", email);
                int userID = Convert.ToInt32(StudentIDCMD.ExecuteScalar());

                SqlCommand toGroup = new SqlCommand(toGroup_Query, sqlCon);
                toGroup.Parameters.AddWithValue("@courseID", classID);
                toGroup.Parameters.AddWithValue("@userID", userID);
                toGroup.Parameters.AddWithValue("@teamID", teamID);
                toGroup.ExecuteNonQuery();

                sqlCon.Close();

            }
          

        }
        public void createTeam(int courseID, int team)          //teams created in class before student is added
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string DoesTeamExist_Query = "Select COUNT(teamID) FROM teams_table WHERE courseID=@courseID AND name=@name";
                SqlCommand teamCheck = new SqlCommand(DoesTeamExist_Query, sqlCon);
                teamCheck.Parameters.AddWithValue("@courseID", courseID);
                teamCheck.Parameters.AddWithValue("@name", team);
                int check = Convert.ToInt32(teamCheck.ExecuteScalar());
                if (check == 0)
                {
                    string insertTeam = "INSERT INTO [dbo].[teams_table] ([name], [courseID]) VALUES (@name, @courseID)";
                    SqlCommand newTeam = new SqlCommand(insertTeam, sqlCon);
                    newTeam.Parameters.AddWithValue("@name", team);
                    newTeam.Parameters.AddWithValue("@courseID", courseID);
                    newTeam.ExecuteNonQuery();
                }
                
                sqlCon.Close();

            }
        }

        public void RemoveFromClass(int userID, int courseID)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("_removeStudentFromClass", sqlCon);         
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@courseID", courseID);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.ExecuteNonQuery();

                RemoveFromGroup(sqlCon, userID, courseID);
                sqlCon.Close();

            }
        }

        public void RemoveFromGroup(SqlConnection sqlCon, int userID, int courseID)
        {
            SqlCommand cmd = new SqlCommand("_deleteStudentFromTeam", sqlCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@courseID", courseID);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.ExecuteNonQuery();
        }
    }
}