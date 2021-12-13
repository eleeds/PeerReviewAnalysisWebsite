using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class StudentResponsePage : System.Web.UI.Page
    {
        int currentquestion = 0;
        int teamMember = 0;
        string[] questionArray = new string[7];      // 7 because of each column coming back from question query
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }


            if (!IsPostBack)                         //loads first question from the question set
            {
                if (Check())
                {
                    GetNextQuestion();
                    Questionlbl.Text = "Survey Question# " + (currentquestion + 1).ToString() + " out of " + Session["questionCount"];
                    QuestionInfo(questionArray);
                    Session["currentQuestion"] = currentquestion;
                    Session["reviewQuestionID"] = questionArray[0];

                }
                else
                {
                    GetNextQuestion();
                    MemberNamelbl.Text = "Review for " + StudentGridview.Rows[teamMember].Cells[0].Text;
                    Questionlbl.Text = "Question# " + (currentquestion + 1).ToString() + " out of " + Session["questionCount"] + " for current student";
                    Session["currentQuestion"] = currentquestion;
                    Session["reviewQuestionID"] = questionArray[0];

                }
            }

        }

        private bool Check()
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                sqlCon.Open();
                string nextQuestionQuery = "SELECT COUNT(1) FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet AND classSurvey = 1";
                SqlCommand sqlCmd = new SqlCommand(nextQuestionQuery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@courseID", Session["courseID"]);
                sqlCmd.Parameters.AddWithValue("@questionSet", Session["questionSet"]);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                sqlCon.Close();
                if (count > 0)
                {
                    Session["teamID"] = null;        //question set is survey  
                    return true;                    //teamID isn't required

                }
                else return false;          //question set is not a survey
            }
        }

        private void GetNextQuestion()
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                sqlCon.Open();
                string getReviewIDsQuery = "SELECT reviewQuestionID FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDsQuery, sqlCon);
                sqlCmd_IDs.Parameters.AddWithValue("@courseID", Session["courseID"]);
                sqlCmd_IDs.Parameters.AddWithValue("@questionSet", Session["questionSet"]);
                List<int> reviewIDs = new List<int>();
                SqlDataReader IDreader = sqlCmd_IDs.ExecuteReader();
                while (IDreader.Read())                                         //gets questionIDs for set. will be greater than 1
                {
                    reviewIDs.Add(Convert.ToInt32(IDreader["reviewQuestionID"]));
                }
                IDreader.Close();

                for (int i = 0; i < reviewIDs.Count; i++)               //just in case student doesn't finish survey in one sitting.
                {                                                       //it will find the student's last submitted entry and continue from there
                    if (!CheckIfQuestionIsDone(sqlCon, reviewIDs[i]))
                    {
                        currentquestion = i;
                        Session["currentQuestion"] = currentquestion;
                        break;
                    }
                    if (reviewIDs[i] == reviewIDs[reviewIDs.Count - 1] && Session["teamID"] != null)   //reset questions back to 0
                    {
                        ChangeTeamMember();
                        return;
                    }
                }


                string nextQuestionQuery = "SELECT * FROM questions_table WHERE reviewQuestionID=@reviewQuestionID";
                SqlCommand sqlCmd = new SqlCommand(nextQuestionQuery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@reviewQuestionID", reviewIDs[currentquestion]);

                SqlDataReader reader = sqlCmd.ExecuteReader();                  //reads current question information and saves to array
                while (reader.Read())
                {
                    questionArray[0] = reader["reviewQuestionID"].ToString();
                    questionArray[1] = reader["courseID"].ToString();
                    questionArray[2] = reader["question"].ToString();
                    questionArray[3] = reader["type"].ToString();
                    questionArray[4] = reader["correctResponses"].ToString();
                    questionArray[5] = reader["questionName"].ToString();
                    questionArray[6] = reader["questionSet"].ToString();
                }
                reader.Close();
                sqlCon.Close();
                Session["questionCount"] = reviewIDs.Count;
                Session["reviewQuestionID"] = questionArray[0];

                QuestionInfo(questionArray);

            }
        }

        private void QuestionInfo(string[] question)        //displays the radiobuttons or textbox depending on review question type
        {
            int itemNum = 0;
            if (question[3] == "1-5 Score Rating")
            {
                string[] items = question[4].Split(',');
                foreach (string s in items)
                {
                    Radiobttns1to5.Items[itemNum].Text = s;
                    itemNum++;
                }
                Radiobttns1to5.Visible = true;
            }
            else if (question[3] == "1-4 Score Rating")
            {
                string[] items = question[4].Split(',');
                foreach (string s in items)
                {
                    RadioBttns1to4.Items[itemNum].Text = s;
                    itemNum++;
                }
                RadioBttns1to4.Visible = true;

            }
            else if (question[3] == "Yes or No")
            {
                string[] items = question[4].Split(',');
                foreach (string s in items)
                {
                    RadiobttnsYesorNo.Items[itemNum].Text = s;
                    itemNum++;
                }
                RadiobttnsYesorNo.Visible = true;
            }
            else                    //type = response
            {
                feedbackTxtbox.Visible = true;
            }
            Errorlbl.Text = question[2];
            TextBox1.Text = question[2];

        }

        protected void SubmitBttnClick(object sender, EventArgs e)
        {
            if (!AnswerProvided())
            {
                return;
            }
            SubmitResponse();

            if (Session["teamID"] == null)
            {
                currentquestion = Convert.ToInt32(Session["currentQuestion"]);

                if (currentquestion < Convert.ToInt32(Session["questionCount"]) - 1)    //means reviewer needs to answer more questions for current student
                {
                    ClearSelections();
                    GetNextQuestion();
                }
                else                         //reviewer has finished, return to main student page
                {
                    Response.Redirect("StudentMain.aspx");
                }
                Questionlbl.Text = "Survey Question# " + (currentquestion + 1).ToString() + " out of " + Session["questionCount"];
            }
            else
            {
                teamMember = Convert.ToInt32(Session["teamMember"]);
                ClearSelections();
                GetNextQuestion();
                Questionlbl.Text = "Question# " + (currentquestion + 1).ToString() + " out of " + Session["questionCount"] + " for current student";
            }

        }

        private void ChangeTeamMember()
        {

            if (teamMember + 1 < StudentGridview.Rows.Count)       //means reviewer is moving on to another student to review
            {
                teamMember++;
                Session["teamMember"] = teamMember;
                currentquestion = 0;
                Session["currentQuestion"] = currentquestion;
                GetNextQuestion();
            }
            else                         //reviewer has finished, return to main student page
            {
                Response.Redirect("StudentMain.aspx");
            }
            Questionlbl.Text = "Question# " + (currentquestion + 1).ToString() + " out of " + Session["questionCount"] + " for current student";
            MemberNamelbl.Text = "Review for " + StudentGridview.Rows[teamMember].Cells[0].Text;

        }
        private void SubmitResponse()
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                string insertReviewQuery;
                string response;
                sqlCon.Open();
                string getReviewIDs = "SELECT reviewQuestionID FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDs, sqlCon);
                sqlCmd_IDs.Parameters.AddWithValue("@courseID", Session["courseID"]);
                sqlCmd_IDs.Parameters.AddWithValue("@questionSet", Session["questionSet"]);


                if (Session["teamID"] == null)      //inserts as survey. student reviewed and teamID columns equal Null
                {
                    insertReviewQuery = "INSERT INTO Response_table (userID, reviewQuestionID, dateComplete, userResponse, questionSet)" +
                                        " VALUES (@userID, @reviewQuestionID, @dateComplete, @userResponse, @questionSet)";

                    if (RadioBttns1to4.Visible)
                    {
                        response = RadioBttns1to4.SelectedItem.ToString();
                    }
                    else if (Radiobttns1to5.SelectedIndex != -1)
                    {
                        response = Radiobttns1to5.SelectedItem.ToString();
                    }
                    else if (RadiobttnsYesorNo.SelectedIndex != -1)
                    {
                        response = RadiobttnsYesorNo.SelectedItem.ToString();
                    }
                    else response = feedbackTxtbox.Text;

                }
                else 
                {
                                        //inserts as peer review. student review contains a student
                    insertReviewQuery = "INSERT INTO Response_table (userID, teamID, reviewQuestionID, dateComplete, userResponse, studentReviewed, questionSet)" +
                    " VALUES (@userID, @teamID, @reviewQuestionID, @dateComplete, @userResponse, @studentReviewed, @questionSet)";

                    if (RadioBttns1to4.Visible)
                    {
                        response = RadioBttns1to4.SelectedValue.ToString();
                    }
                    else if (Radiobttns1to5.SelectedIndex != -1)
                    {
                        response = Radiobttns1to5.SelectedValue.ToString();
                    }
                    else if (RadiobttnsYesorNo.SelectedIndex != -1)
                    {
                        response = RadiobttnsYesorNo.SelectedItem.ToString();
                    }
                    else response = feedbackTxtbox.Text;
                }
                
                SqlCommand sqlInsert = new SqlCommand(insertReviewQuery, sqlCon);
                sqlInsert.Parameters.AddWithValue("@userID", Session["userID"]);
                sqlInsert.Parameters.AddWithValue("@reviewQuestionID", Session["reviewQuestionID"]);
                sqlInsert.Parameters.AddWithValue("@dateComplete", DateTime.Today.ToShortDateString());
                sqlInsert.Parameters.AddWithValue("@userResponse", response);
                if (Session["teamID"] != null)
                {
                    sqlInsert.Parameters.AddWithValue("@teamID", Session["teamID"]);
                    sqlInsert.Parameters.AddWithValue("@studentReviewed", StudentGridview.DataKeys[Convert.ToInt32(Session["teamMember"])].Value);
                }

                sqlInsert.Parameters.AddWithValue("@questionSet", Session["questionSet"]);
                sqlInsert.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        private bool AnswerProvided()
        {
            if (RadioBttns1to4.Visible && RadioBttns1to4.SelectedIndex == -1)
            {
                Errorlbl.Visible = true;
                Errorlbl.Text = "Please choose a selection";
                return false;
            }
            else if (Radiobttns1to5.Visible && Radiobttns1to5.SelectedIndex == -1)
            {
                Errorlbl.Visible = true;
                Errorlbl.Text = "Please choose a selection";
                return false;
            }
            return true;
        }

        private void ClearSelections()
        {
            Errorlbl.Visible = false;
            RadioBttns1to4.Visible = false;
            RadioBttns1to4.SelectedIndex = -1;
            Radiobttns1to5.Visible = false;
            Radiobttns1to5.SelectedIndex = -1;
            RadiobttnsYesorNo.Visible = false;
            RadiobttnsYesorNo.SelectedIndex = -1;
            feedbackTxtbox.Visible = false;
            feedbackTxtbox.Text = "";
        }

        private bool CheckIfQuestionIsDone(SqlConnection sqlCon, int reviewQuestion)
        {
            string checkCompletionQuery;
            if (StudentGridview.Rows.Count == 0)
            {
                StudentGridview.DataBind();
            }
            if (Session["teamID"] == null)
            {
                checkCompletionQuery = "SELECT COUNT(1) FROM Response_table WHERE questionSet=@questionSet " +
                                        "AND userID=@userID AND reviewQuestionID=@reviewQuestionID";
            }
            else
            {
                checkCompletionQuery = "SELECT COUNT(1) FROM Response_table WHERE studentReviewed=@studentReviewed AND questionSet=@questionSet " +
                   "AND teamID=@teamID AND userID=@userID AND reviewQuestionID=@reviewQuestionID";
            }

            SqlCommand QuestionDoneCheckSQL = new SqlCommand(checkCompletionQuery, sqlCon);
            if (Session["teamID"] != null)
            {
                QuestionDoneCheckSQL.Parameters.AddWithValue("@studentReviewed", Convert.ToInt32(StudentGridview.DataKeys[teamMember].Value));
                QuestionDoneCheckSQL.Parameters.AddWithValue("@teamID", Convert.ToInt32(StudentGridview.DataKeys[0].Values[1]));
            }

            QuestionDoneCheckSQL.Parameters.AddWithValue("@questionSet", Session["questionSet"]);
            QuestionDoneCheckSQL.Parameters.AddWithValue("@userID", Session["userID"]);
            QuestionDoneCheckSQL.Parameters.AddWithValue("@reviewQuestionID", reviewQuestion);

            int count = Convert.ToInt32(QuestionDoneCheckSQL.ExecuteScalar());
            if (count != 0)
            {
                return true;
            }
            else return false;
        }

        protected void HomeBttnClick(object sender, EventArgs e)
        {
            Response.Redirect("StudentMain.aspx");
        }
    }
}