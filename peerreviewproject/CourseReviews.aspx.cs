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


        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userID"] = Session["userID"];
            if (IsPostBack)
            {
                if (GroupMembersGridview.SelectedIndex == -1)
                {
                    Panel1.Visible = false;
                }
                GroupMembersGridview.DataBind();
            }

            if (!IsPostBack)
            {
                //TemplateField tfield = new TemplateField();
                //tfield.HeaderText = "Country";
                //ClassSurveyGridView.Columns.Add(tfield);
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
            if (ResultsGridview.Rows.Count > 0)
            {
                DataTable RatingsDatatable = new DataTable();
                RatingsDatatable.Columns.Add(" ");
                RatingsDatatable.Columns.Add("Current Rating");
                RatingsDatatable.Columns.Add("Max Score");

                DataTable CommentsDatatable = new DataTable();
                CommentsDatatable.Columns.Add("Feedback");
                CommentsDatatable.Columns.Add("Set");
                CommentsDatatable.Columns.Add("Date");

                Dictionary<string, Double> TypeandScores = new Dictionary<string, Double>();     //type of question and the rating given for the question

                for (int i = 0; i < ResultsGridview.Rows.Count; i++)
                {
                    if (ResultsGridview.DataKeys[i].Values[1].ToString() == "Comment Response")
                    {
                        ResultsGridview.Rows[i].Visible = false;
                    }
                    if (!TypeandScores.ContainsKey(ResultsGridview.Rows[i].Cells[4].Text))      //adds rating and score to dictionary
                    {
                        try
                        {
                            TypeandScores.Add(ResultsGridview.Rows[i].Cells[4].Text, Convert.ToDouble(ResultsGridview.Rows[i].Cells[2].Text)); //review is a number
                        }
                        catch           
                        {
                            CommentsDatatable.Rows.Add(ResultsGridview.Rows[i].Cells[2].Text, ResultsGridview.Rows[i].Cells[5].Text, ResultsGridview.Rows[i].Cells[7].Text);    //review is a string/comment
                        }
                    }
                    else
                        TypeandScores[ResultsGridview.Rows[i].Cells[4].Text] = (Convert.ToDouble(TypeandScores[ResultsGridview.Rows[i].Cells[4].Text]) + Convert.ToDouble(ResultsGridview.Rows[i].Cells[2].Text)) / 2;
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
                Panel1.Visible = true;

                CommentsGridview.DataSource = CommentsDatatable;
                CommentsGridview.DataBind();
                ResultsGridview.Caption = "Reviews for " + GroupMembersGridview.SelectedRow.Cells[1].Text;
            }
            else
            {
                Panel1.Visible = false;
                ResultsGridview.Caption = "";
            }
            if (GroupListbox.SelectedIndex != -1)
            {
                ResultsGridview.EmptyDataText = "No reviews for student";
                CommentsGridview.EmptyDataText = "No reviews for student";
            }

        }

        protected void GroupListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            CourseSurveyListBox.SelectedIndex = -1;
        }


        protected void CourseDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupListbox.DataBind();
            if (GroupListbox.Items.Count > 0)
                GroupListbox.SelectedIndex = 0;
            GroupMembersGridview.DataBind();
            if (GroupMembersGridview.Rows.Count > 0)
                GroupMembersGridview.SelectedIndex = 0;
            Panel1.Visible = false;
            ClassSurveyGridView.Visible = false;
        }

        protected void CourseSurveyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupListbox.SelectedIndex = -1;
            GroupMembersGridview.SelectedIndex = -1;
            ResultsGridview.Visible = false;
            Panel1.Visible = false;
            GroupMembersGridview.Visible = false;
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
            ClassSurveyGridView.HeaderRow.Visible = false;
            //ClassSurveyGridView.Columns[0].it
            
        }

        private void TallyVotes(DataTable VotesDataTable)
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
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
                        VotesDataTable.Rows[i][VotesDataTable.Columns.Count - 1] =  NumberOfVotes[k];
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
            CourseSurveyListBox.SelectedIndex = -1;
        }

        protected void ClassSurveyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCountry = new TextBox();
                txtCountry.ReadOnly = true;
                txtCountry.Columns = 3;
                txtCountry.TextMode = TextBoxMode.MultiLine;
                txtCountry.ID = "txtCountry";
                txtCountry.Text = (e.Row.DataItem as DataRowView).Row["Question"].ToString();
                txtCountry.Width = 300;
                e.Row.Cells[0].Controls.Add(txtCountry);
            }
        }
    }
}