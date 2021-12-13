using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class CourseReviews : System.Web.UI.Page
    {

        bool Flag = false;                  //for keeping track of scores because of paging settings seperating results gridview
        Dictionary<string, Double> TypeandScores = new Dictionary<string, Double>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }


            if (IsPostBack)
            {             
                GroupMembersGridview.DataBind();
            }


        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            if (Session["course"] != null)      //if first time page loading, select index where professor clicked
            {
                CourseDropDown.SelectedIndex = CourseDropDown.Items.IndexOf(CourseDropDown.Items.FindByText(Session["course"].ToString()));
            }
        }

        protected void ResultsGridview_DataBound(object sender, EventArgs e)
        {

            if (Flag == true && ResultsGridview.AllowPaging)
            {
                ResultsGridview.AllowPaging = false;
                ResultsGridview.DataBind();
            }
            else if (ResultsGridview.AllowPaging)
            {
                for (int i = 0; i < ResultsGridview.Rows.Count; i++)    //removes comments from results gridview
                {
                    if (ResultsGridview.DataKeys[i].Values[1].ToString() == "Comment Response")     
                    {
                        ResultsGridview.Rows[i].Visible = false;
                    }

                }
                return;
            }
            else 
            {
                return;
            }
            DataTable RatingsDatatable = new DataTable();
            RatingsDatatable.Columns.Add(" ");
            RatingsDatatable.Columns.Add("Current Rating");
            RatingsDatatable.Columns.Add("Max Score");
            DataTable CommentsDatatable = new DataTable();
            CommentsDatatable.Columns.Add("Feedback");
            CommentsDatatable.Columns.Add("Set");
            CommentsDatatable.Columns.Add("Date");
            //CommentsDatatable.Columns.Add("LeftBy");
            List<string> names = new List<string>();

            if (ResultsGridview.Rows.Count > 0)
            {
                bool isRating;

                for (int j = 0; j < ResultsGridview.Rows.Count; j++)
                {
                    if (ResultsGridview.DataKeys[j].Values[1].ToString() == "Comment Response")
                    {
                        ResultsGridview.Rows[j].Visible = false;
                    }
                    if (!TypeandScores.ContainsKey(ResultsGridview.Rows[j].Cells[4].Text))      //adds rating and score to dictionary
                    {
                        isRating = double.TryParse(ResultsGridview.Rows[j].Cells[2].Text, out double convertedNumber);
                        if (isRating)
                        {
                            TypeandScores.Add(ResultsGridview.Rows[j].Cells[4].Text, convertedNumber);
                        }
                        else
                        {
                            if (ResultsGridview.Rows[j].Cells[2].Text != "&nbsp;" && ResultsGridview.DataKeys[j].Values[1].ToString() == "Comment Response")
                            {
                                CommentsDatatable.Rows.Add(ResultsGridview.Rows[j].Cells[2].Text, ResultsGridview.Rows[j].Cells[5].Text, ResultsGridview.Rows[j].Cells[7].Text);
                                names.Add(ResultsGridview.Rows[j].Cells[0].Text);
                            }
                        }
                    }
                    else
                    TypeandScores[ResultsGridview.Rows[j].Cells[4].Text] = (Convert.ToDouble(TypeandScores[ResultsGridview.Rows[j].Cells[4].Text]) + Convert.ToDouble(ResultsGridview.Rows[j].Cells[2].Text)) / 2;
                }                                                           //if a score already exists in dictionary, update and divide for average
                
                int count = 0;
                foreach (var key in TypeandScores)
                {
                    if (ResultsGridview.DataKeys[count].Values[1].ToString() == "1-4 Score Rating")
                        RatingsDatatable.Rows.Add(key.Key.ToString(), Convert.ToDecimal(key.Value), 4); //max score = 4
                    else if (ResultsGridview.DataKeys[count].Values[1].ToString() == "1-5 Score Rating")
                        RatingsDatatable.Rows.Add(key.Key.ToString(), Convert.ToDecimal(key.Value), 5);  //max score = 5
                    else
                    {
                        CommentsDatatable.Rows.Add(key.Value);
                    }
                    count++;
                }


                RatingGridview.DataSource = RatingsDatatable;
                RatingGridview.DataBind();
                CommentsGridview.DataSource = CommentsDatatable;
                CommentsGridview.DataBind();
                ResultsGridview.Caption = "Reviews for " + GroupMembersGridview.SelectedRow.Cells[1].Text;
                if (CommentsGridview.Rows.Count > 0)
                {
                    for(int i = 0; i < CommentsGridview.Rows.Count; i++)
                    {
                        if (names[i] != "")
                        {
                            CommentsGridview.Rows[i].ToolTip = "Comment by " + names[i];
                        }
                    }
                }

                if (RatingGridview.Rows.Count > 0 && CommentsGridview.Rows.Count < 1)
                {
                    CommentsGridview.EmptyDataText = "No reviews for student";
                    CommentsGridview.DataBind();
                }
                else if (RatingGridview.Rows.Count < 1 && CommentsGridview.Rows.Count > 1)
                {
                    RatingGridview.EmptyDataText = "No reviews for student";
                    RatingGridview.DataBind();
                }

            }
            else if (GroupMembersGridview.SelectedIndex == -1)
            {
                RatingGridview.DataBind();
                CommentsGridview.DataBind();
                RatingGridview.EmptyDataText = "No members to review";
                ResultsGridview.EmptyDataText = "No members to review";
                CommentsGridview.EmptyDataText = "No members to review";
            }
            else
            {
                RatingGridview.DataBind();
                CommentsGridview.DataBind();
                ResultsGridview.Caption = "Reviews for " + GroupMembersGridview.SelectedRow.Cells[1].Text;
                RatingGridview.EmptyDataText = "No scores for student";
                ResultsGridview.EmptyDataText = "No reviews for student";
                CommentsGridview.EmptyDataText = "No reviews for student";
            }
            Flag = false;
            ResultsGridview.AllowPaging = true;
            ResultsGridview.PageSize = 8;
        }


        protected void CourseDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.DataBind();
            Flag = true;
            if (GridView1.Rows.Count > 0)
                GridView1.SelectedIndex = 0;
            GroupMembersGridview.DataBind();

            if (GroupMembersGridview.Rows.Count > 0)
            {
                GroupMembersGridview.SelectedIndex = 0;
                ResultsGridview.DataBind();
                RatingGridview.DataBind();
                CommentsGridview.DataBind();
                GroupMembersGridview.Visible = true;
            }
            else
            {
                ResultsGridview.Visible = false;
                GroupMembersGridview.Visible = false;
            }

            ClassSurveyGridView.Visible = false;
        }

        protected void CourseSurveyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupMembersGridview.SelectedIndex = -1;
            ResultsGridview.Visible = false;
            GroupMembersGridview.Visible = false;
            RatingGridview.Visible = false;
            CommentsGridview.Visible = false;
            ClassSurveyGridView.Visible = true;
            GetSurveyResults();
        }

        private void GetSurveyResults()
        {
            DataTable VotesDataTable = new DataTable(); //column 1 = questions description. columns 2-5 = multiple choice answers (1-4) or (1-5) 
            VotesDataTable.Columns.Add("Question");     //column 6 = total votes casted for question
            VotesDataTable.Columns.Add();
            VotesDataTable.Columns.Add();
            VotesDataTable.Columns.Add();
            VotesDataTable.Columns.Add();
            VotesDataTable.Columns.Add();
            VotesDataTable.Columns.Add();

            TallyVotes(VotesDataTable);
            RemoveUnusedColumns(VotesDataTable);

            ClassSurveyGridView.DataSource = VotesDataTable;
            ClassSurveyGridView.DataBind();
            ClassSurveyGridView.Caption = "Class Survey for Set " + CourseSurveyListBox.SelectedValue;
            if(ClassSurveyGridView.Rows.Count != 0)
                ClassSurveyGridView.HeaderRow.Visible = false;

        }

        private void TallyVotes(DataTable VotesDataTable)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                sqlCon.Open();

                string GetReviewQuestionsquery = "SELECT reviewQuestionID, question FROM questions_table WHERE courseID=@courseID AND questionSet=@questionSet";


                Dictionary<int, string> QuestionsDictionary = new Dictionary<int, string>();
                SqlCommand GetsurveyQuestionsQueryCommand = new SqlCommand(GetReviewQuestionsquery, sqlCon);
                GetsurveyQuestionsQueryCommand.Parameters.AddWithValue("@courseID", CourseDropDown.SelectedValue);
                GetsurveyQuestionsQueryCommand.Parameters.AddWithValue("@questionSet", CourseSurveyListBox.SelectedValue);
                SqlDataReader SurveyQuestionsReader = GetsurveyQuestionsQueryCommand.ExecuteReader();
                while (SurveyQuestionsReader.Read())
                {
                    QuestionsDictionary.Add(Convert.ToInt32(SurveyQuestionsReader[0]), SurveyQuestionsReader[1].ToString());

                }
                SurveyQuestionsReader.Close();     //question ids now in list


                for (int i = 0; i < QuestionsDictionary.Count; i++)
                {
                    string submitsExist = "SELECT COUNT(userID) AS number FROM Response_table AS Response_table_1 WHERE (questionSet = @questionSet)" +
                    " AND (reviewQuestionID = @review)";
                    SqlCommand check = new SqlCommand(submitsExist, sqlCon);
                    check.Parameters.AddWithValue("@questionSet", CourseSurveyListBox.SelectedValue);
                    check.Parameters.AddWithValue("@review", QuestionsDictionary.ElementAt(i).Key);
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count == 0)
                    {
                        sqlCon.Close();
                        ClassSurveyGridView.EmptyDataText = "No submissions yet";
                        return;
                    }
                  
                    string GetVotesQuery = "SELECT FORMAT(CONVERT (float, COUNT(Response_table.userResponse) / (numberOfSubmits.number * .1 * 10)), 'P') AS Votes," +
                    "Response_table.userResponse, numberOfSubmits.number FROM questions_table INNER JOIN Response_table ON questions_table.reviewQuestionID = Response_table.reviewQuestionID " +
                    "CROSS JOIN (SELECT COUNT(userID) AS number FROM Response_table AS Response_table_1 WHERE (questionSet = @questionSet)" +
                    " AND (reviewQuestionID = @review)) AS numberOfSubmits WHERE (questions_table.questionSet = @questionSet) " +
                    "AND (Response_table.reviewQuestionID = @review) GROUP BY questions_table.questionName, numberOfSubmits.number, questions_table.question, Response_table.userResponse";
                    SqlCommand GetVotesCommand = new SqlCommand(GetVotesQuery, sqlCon);
                    GetVotesCommand.Parameters.AddWithValue("@questionSet", CourseSurveyListBox.SelectedValue);
                    GetVotesCommand.Parameters.AddWithValue("@review", QuestionsDictionary.ElementAt(i).Key);
                    List<string> StudentVotesList = new List<string>();
                    List<string> NumberOfVotes = new List<string>();
                    SqlDataReader StudentVotesReader = GetVotesCommand.ExecuteReader();

                    while (StudentVotesReader.Read())
                    {
                        StudentVotesList.Add(StudentVotesReader[0].ToString() + " " + StudentVotesReader[1].ToString());
                        NumberOfVotes.Add("(Based on " + StudentVotesReader[2].ToString() + " submits)");
                    }
                    
                    StudentVotesReader.Close();
                    VotesDataTable.Rows.Add(QuestionsDictionary.ElementAt(i).Value);
                    for (int k = 0; k < StudentVotesList.Count; k++)
                    {
                        VotesDataTable.Rows[i][k + 1] = StudentVotesList[k];
                        VotesDataTable.Rows[i][VotesDataTable.Columns.Count - 1] = NumberOfVotes[k];
                    }

                }
                sqlCon.Close();
            }
        }

        private void RemoveUnusedColumns(DataTable VotesDataTable)      //if the all the cells in a column are empty, remove it
        {
            for (int i = 0; i < VotesDataTable.Columns.Count; i++)
            {
                int count = 0;
                for (int h = 0; h < VotesDataTable.Rows.Count; h++)
                {
                    if (VotesDataTable.Rows[h][i].ToString() == "")
                    {
                        count++;
                    }
                }
                if (count == VotesDataTable.Rows.Count)
                {
                    VotesDataTable.Columns.RemoveAt(i);
                    --i;
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupMembersGridview.DataBind();

            if (GroupMembersGridview.Rows.Count > 0)
            {
                GroupMembersGridview.SelectedIndex = 0;
            }
            else
            {
                GroupMembersGridview.SelectedIndex = -1;
            }

            ClassSurveyGridView.Visible = false;
            ResultsGridview.Visible = true;
            GroupMembersGridview.Visible = true;
            RatingGridview.Visible = true;
            CommentsGridview.Visible = true;
            CourseSurveyListBox.SelectedIndex = -1;
            Flag = true;
        }

        protected void ClassSurveyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtQuestion = new TextBox();
                txtQuestion.ReadOnly = true;
                txtQuestion.Columns = 3;
                txtQuestion.TextMode = TextBoxMode.MultiLine;
                txtQuestion.ID = "txtQuestion";
                txtQuestion.Text = (e.Row.DataItem as DataRowView).Row["Question"].ToString();
                txtQuestion.Width = 300;
                e.Row.Cells[0].Controls.Add(txtQuestion);
            }
        }

        protected void GroupMembersGridview_SelectedIndexChanged(object sender, EventArgs e)
        {
            Flag = true;
        }

        protected void ResultsGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Flag = true;
        }

        protected void HomeBttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }
    }
}