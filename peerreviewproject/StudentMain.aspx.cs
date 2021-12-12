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
        string groupMembers;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }
            //Session["userID"] = 2550;          //for testing
            lblStudentDetails.Text = "User ID: " + Session["email"];
            Session["type"] = "Student";
        }


        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }

        private void Check()            //check to see which reviews are completed already
        {
            for (int i = 0; i < StudentReviewsGridview.Rows.Count; i++)
            {
                if (SetComplete(i))
                {
                    if (groupMembers == "Exist")
                    {
                        StudentReviewsGridview.Rows[i].Cells[7].Text = "Complete";
                    }
                    else
                    {
                        StudentReviewsGridview.Rows[i].Cells[7].Text = "No group members yet";
                    }
                    StudentReviewsGridview.Rows[i].Cells[0].Visible = true;
                    StudentReviewsGridview.Rows[i].Cells[0].Enabled = false;
                }

            }
        }
        private bool SetComplete(int rowIndex)      //if true applys complete status to gridview
        {
            groupMembers = "Exist";
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                sqlCon.Open();

                string ifSurvey = "SELECT classSurvey FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand ifSurveyQuery = new SqlCommand(ifSurvey, sqlCon);
                ifSurveyQuery.Parameters.AddWithValue("@courseID", StudentReviewsGridview.DataKeys[rowIndex].Value);
                ifSurveyQuery.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                string surveyCheck = ifSurveyQuery.ExecuteScalar().ToString();
                if (surveyCheck == "True")
                {
                    string getReviewIDsQuery = "SELECT COUNT(reviewQuestionID) FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                    SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDsQuery, sqlCon);                                          //questions count in set
                    sqlCmd_IDs.Parameters.AddWithValue("@courseID", StudentReviewsGridview.DataKeys[rowIndex].Value);
                    sqlCmd_IDs.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                    int questionsCount = Convert.ToInt32(sqlCmd_IDs.ExecuteScalar());

                    string userResponses = "SELECT COUNT(1) FROM Response_table WHERE userID=@userID AND questionSet=@questionSet";
                    SqlCommand sql_responses = new SqlCommand(userResponses, sqlCon);                                             //questions completed by student
                    sql_responses.Parameters.AddWithValue("@userID", Session["userID"]);
                    sql_responses.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                    int responseCount = Convert.ToInt32(sql_responses.ExecuteScalar());

                    if (responseCount == questionsCount)
                    {
                        return true;
                    }
                    else return false;
                }
                else
                {
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

                    string userResponses = "SELECT COUNT(1) FROM Response_table WHERE userID=@userID AND questionSet=@questionSet AND teamID=@teamID";
                    SqlCommand sql_responses = new SqlCommand(userResponses, sqlCon);                                             //questions completed by student
                    sql_responses.Parameters.AddWithValue("@userID", Session["userID"]);
                    sql_responses.Parameters.AddWithValue("@questionSet", StudentReviewsGridview.Rows[rowIndex].Cells[3].Text);
                    sql_responses.Parameters.AddWithValue("@teamID", StudentReviewsGridview.DataKeys[rowIndex].Values[1]);
                    int responseCount = Convert.ToInt32(sql_responses.ExecuteScalar());
                    sqlCon.Close();

                    if (membersCount == 0)
                    {
                        groupMembers = "none";
                        return true;
                    }
                    if (responseCount == membersCount * questionsCount)
                    {
                        return true;
                    }
                    else return false;
                }
            }
        }
        protected void StudentReviewsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["courseID"] = StudentReviewsGridview.SelectedValue;
            Session["questionSet"] = StudentReviewsGridview.SelectedRow.Cells[3].Text;
            Session["teamID"] = StudentReviewsGridview.DataKeys[StudentReviewsGridview.SelectedIndex].Values[1];
            Response.Redirect("StudentResponsePage.aspx");
        }

        protected void StudentReviewsGridView_DataBound(object sender, EventArgs e)
        {
            Check();
        }

        protected void ChangePass_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePass.aspx");
        }
    }
}