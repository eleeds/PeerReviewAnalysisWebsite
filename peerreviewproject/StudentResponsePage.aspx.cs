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
        string sqlconn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userID"] = Session["userID"];
            if (!IsPostBack)                         //loads first question from the question set
            {
                    GetNextQuestion();
                    QuestionInfo(questionArray);
                    MemberNamelbl.Text = "Review for " + StudentGridview.Rows[teamMember].Cells[0].Text;
                    Questionlbl.Text = "Question# " + (currentquestion + 1).ToString();
                    Session["currentQuestion"] = currentquestion;
                    Session["reviewQuestionID"] = questionArray[0];
            }
            
        }

        private void GetNextQuestion()
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconn))
            {
                sqlCon.Open();
                string getReviewIDsQuery = "SELECT reviewQuestionID FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDsQuery, sqlCon);
                sqlCmd_IDs.Parameters.AddWithValue("@courseID", Session["courseID"]);
                sqlCmd_IDs.Parameters.AddWithValue("@questionSet", Session["questionSet"]);
                List<int> reviewIDs = new List<int>();
                SqlDataReader IDreader = sqlCmd_IDs.ExecuteReader();
                while (IDreader.Read())                        //gets questionIDs. will be greater than 1
                {
                    reviewIDs.Add(Convert.ToInt32(IDreader["reviewQuestionID"]));
                }
                IDreader.Close();

                for (int i = 0; i < reviewIDs.Count; i++)               //just in case student doesn't finish survey in one sitting.
                {                                                       //it will find the student's last submitted entry and continue from there
                    if (!CheckIfQuestionIsDone(sqlCon, reviewIDs[i]))
                    {
                        currentquestion = reviewIDs.IndexOf(reviewIDs[i]);
                        Session["currentQuestion"] = currentquestion;
                        break;
                    }
                    if (i == reviewIDs[reviewIDs.Count - 1])
                    {
                        currentquestion = reviewIDs.IndexOf(i);
                        CurrentTeamMember();
                        i = 0;
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
            else                    //type = response
            {
                feedbackTxtbox.Visible = true;
            }
            Errorlbl.Text = question[2];
            TextBox1.Text = question[2];
            
        }

        protected void SubmitBttnClick(object sender, EventArgs e)
        {
            if(!AnswerProvided())
            {
                return;
            }

            teamMember = Convert.ToInt32(Session["teamMember"]);
            GetNextQuestion();
            SubmitResponse();

            ClearSelections();
            currentquestion = Convert.ToInt32(Session["currentQuestion"]);
            currentquestion++;

            CurrentTeamMember();
            
        }

        private void CurrentTeamMember()
        {
            if (currentquestion < Convert.ToInt32(Session["questionCount"]))    //means reviewer needs to answer more questions for current student
            {
                Session["currentQuestion"] = currentquestion;
                GetNextQuestion();
            }
            else if (teamMember + 1 < StudentGridview.Rows.Count)       //means reviewer is moving on to another student to review
            {
                teamMember++;
                Session["teamMember"] = teamMember;
                currentquestion = 0;
                Session["currentQuestion"] = currentquestion;
                GetNextQuestion();
            }
            else                         //reviewer has finished, return to main student page
            {
                Session["userID"] = 2551;
                Response.Redirect("StudentMain.aspx");
            }
            Questionlbl.Text = "Question# " + (currentquestion + 1).ToString();
            MemberNamelbl.Text = "Review for " + StudentGridview.Rows[teamMember].Cells[0].Text;

            Session["reviewQuestionID"] = questionArray[0];
        }
        private void SubmitResponse()
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconn))
            {
                string response;
                sqlCon.Open();
                string getReviewIDs = "SELECT reviewQuestionID FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";
                SqlCommand sqlCmd_IDs = new SqlCommand(getReviewIDs, sqlCon);
                sqlCmd_IDs.Parameters.AddWithValue("@courseID", Session["courseID"]);
                sqlCmd_IDs.Parameters.AddWithValue("@questionSet", Session["questionSet"]);

                string insertReviewQuery = "INSERT INTO Response_table (userID, teamID, reviewQuestionID, dateComplete, userResponse, studentReviewed, questionSet)" +
                    " VALUES (@userID, @teamID, @reviewQuestionID, @dateComplete, @userResponse, @studentReviewed, @questionSet)";
                if (RadioBttns1to4.Visible)
                {
                    response = RadioBttns1to4.SelectedValue;
                }
                else if (Radiobttns1to5.SelectedIndex != -1)
                {
                    response = Radiobttns1to5.SelectedValue;
                }
                else response = feedbackTxtbox.Text;

                SqlCommand sqlInsert = new SqlCommand(insertReviewQuery, sqlCon);
                sqlInsert.Parameters.AddWithValue("@userID", Session["userID"]);
               // sqlInsert.Parameters.AddWithValue("@teamID", Convert.ToInt32(StudentGridview.DataKeys[1].Values[1]));
                sqlInsert.Parameters.AddWithValue("@teamID", Convert.ToInt32(StudentGridview.DataKeys[0].Values[1]));
                sqlInsert.Parameters.AddWithValue("@reviewQuestionID", Session["reviewQuestionID"]);
                //sqlInsert.Parameters.AddWithValue("@dateComplete", DateTime.Today);
                sqlInsert.Parameters.AddWithValue("@dateComplete", DateTime.Today.ToShortDateString());
                sqlInsert.Parameters.AddWithValue("@userResponse", response);
                sqlInsert.Parameters.AddWithValue("@studentReviewed", StudentGridview.DataKeys[teamMember].Value);
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
            else if (feedbackTxtbox.Visible && feedbackTxtbox.Text.Length < 5)
            {
                Errorlbl.Visible = true;
                Errorlbl.Text = "Response isn't long enough. Try Again";
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
            feedbackTxtbox.Visible = false;
            feedbackTxtbox.Text = "";
        }

        private bool CheckIfQuestionIsDone(SqlConnection sqlCon, int reviewQuestion)
        {
            if (StudentGridview.Rows.Count == 0)
            {
                StudentGridview.DataBind();
            }
            
            string checkCompletionQuery = "SELECT COUNT(1) FROM Response_table WHERE studentReviewed=@studentReviewed AND questionSet=@questionSet " +
                "AND teamID=@teamID AND userID=@userID AND reviewQuestionID=@reviewQuestionID" ;
            SqlCommand QuestionDoneCheckSQL = new SqlCommand(checkCompletionQuery, sqlCon);
            QuestionDoneCheckSQL.Parameters.AddWithValue("@studentReviewed", Convert.ToInt32(StudentGridview.DataKeys[teamMember].Value));
           // QuestionDoneCheckSQL.Parameters.AddWithValue("@studentReviewed", StudentGridview.DataKeys[teamMember].Value);
            QuestionDoneCheckSQL.Parameters.AddWithValue("@teamID", Convert.ToInt32(StudentGridview.DataKeys[0].Values[1]));
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

    }
}