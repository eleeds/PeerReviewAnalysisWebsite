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
                DataTable CommentsDatatable = new DataTable();
                RatingsDatatable.Columns.Add(" ");
                RatingsDatatable.Columns.Add("Current Rating");
                RatingsDatatable.Columns.Add("Max Score");
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
                            TypeandScores.Add(ResultsGridview.Rows[i].Cells[4].Text, Convert.ToDouble(ResultsGridview.Rows[i].Cells[2].Text));
                        }
                        catch           
                        {
                            CommentsDatatable.Rows.Add(ResultsGridview.Rows[i].Cells[2].Text, ResultsGridview.Rows[i].Cells[5].Text, ResultsGridview.Rows[i].Cells[7].Text);
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
            ResultsGridview.Visible = false;
            Panel1.Visible = false;
            ClassSurveyGridView.Visible = true;
        }

    }
}