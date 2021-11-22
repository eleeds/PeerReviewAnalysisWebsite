using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace peerreviewproject
{
    public partial class StudentMain : System.Web.UI.Page
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {

           // Session["userID"] = 2550;          //for testing
            lblStudentDetails.Text = "User ID: " + Session["email"];
            Check();
             
        }

      
        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }

        private void Check()            //check to see which reviews are completed already
        {
            for(int i = 0; i < StudentReviewsGridview.Rows.Count; i++)
            {
                if (SetComplete(i))
                {
                    StudentReviewsGridview.Rows[i].Cells[7].Text = "Complete";
                    StudentReviewsGridview.Rows[i].Cells[0].Visible = true;
                    StudentReviewsGridview.Rows[i].Cells[0].Enabled = false;
                }
               
            }
        }
        private bool SetComplete(int rowIndex)      //if true applys complete status to gridview
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string teamMembers = "SELECT COUNT(1) FROM UserTeam_table WHERE teamID=@teamID AND @userID NOT IN (userID)"; //amount of group members
                SqlCommand sqlCmd_members = new SqlCommand(teamMembers, sqlCon);
                sqlCmd_members.Parameters.AddWithValue("@teamID", StudentReviewsGridview.DataKeys[rowIndex].Values[1]);
                sqlCmd_members.Parameters.AddWithValue("@userID", Session["userID"]);
                int membersCount = Convert.ToInt32(sqlCmd_members.ExecuteScalar());

                string getReviewIDsQuery = "SELECT COUNT(reviewQuestionID) FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDsQuery, sqlCon);                                          //questions count in set
                sqlCmd_IDs.Parameters.AddWithValue("@courseID", StudentReviewsGridview.DataKeys[rowIndex].Value);
                sqlCmd_IDs.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                int questionsCount = Convert.ToInt32(sqlCmd_IDs.ExecuteScalar());

                string userReponses = "SELECT COUNT(1) FROM Response_table WHERE userID=@userID AND questionSet=@questionSet";
                SqlCommand sql_reponses = new SqlCommand(userReponses, sqlCon);                                             //questions completed by student
                sql_reponses.Parameters.AddWithValue("@userID", Session["userID"]);
                sql_reponses.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                int responseCount = Convert.ToInt32(sql_reponses.ExecuteScalar());
                sqlCon.Close();
                if (responseCount == membersCount * questionsCount)
                {
                    return true;
                }
                else return false;
            }
        }

       

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {        
            Session["userID"] = Session["userID"];
            Session["courseID"] = StudentReviewsGridview.SelectedValue;
            Session["questionSet"] = StudentReviewsGridview.SelectedRow.Cells[3].Text;
            Session["teamID"] = StudentReviewsGridview.DataKeys[StudentReviewsGridview.SelectedIndex].Values[1];
            Response.Redirect("StudentResponsePage.aspx");
        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            Check();
        }
    }
}