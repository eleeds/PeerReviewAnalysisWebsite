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
    public partial class CreatePeerReviewPage : System.Web.UI.Page
    {
        
        string sqlConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        DataTable SetList_Datatable = new DataTable();
        string answers = "Feedback";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
                 set();
        }

        protected void SubmitBttn_Click(object sender, EventArgs e)
        {

            GetAnswers();

            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                int recentID;
                sqlCon.Open();
                string createQuestion_query = "INSERT INTO questions_table ([courseID], [question], [type], [correctResponses], [questionName], [questionSet])" +
                    " VALUES (@courseID, @questionText, @type, @answers, @name, @set)";
                string ifnew_query = "SELECT MAX(reviewQuestionID) FROM questions_table";
                SqlCommand getID = new SqlCommand(ifnew_query, sqlCon);
                try
                {
                    recentID = Convert.ToInt32(getID.ExecuteScalar());      //grabs recent number, if list is empty return 0
                }
                catch
                {
                    recentID = 0;
                }
                
                SqlCommand question_sqlCmd = new SqlCommand(createQuestion_query, sqlCon);

                question_sqlCmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                question_sqlCmd.Parameters.AddWithValue("@questionText", questDescriptionTB.Text.Trim());
                question_sqlCmd.Parameters.AddWithValue("@type", type_radiobttn.SelectedValue);
                question_sqlCmd.Parameters.AddWithValue("@answers", answers);
                question_sqlCmd.Parameters.AddWithValue("@name", name_tb.Text);

                if (CurrentQuestionSet_listbox.Items.Count == 0)
                {  
                    question_sqlCmd.Parameters.AddWithValue("@set", 1);         //work on duplicates
                }
                else if (CurrentQuestionSet_listbox.SelectedIndex == -1)
                {
                    question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.Items[CurrentQuestionSet_listbox.Items.Count - 1]);
                }
                else question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.SelectedValue);

                question_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                GridView1.DataBind();
                CurrentQuestionSet_listbox.DataBind();
                
            }
            clearText();
        }

        protected void type_radiobttn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnswerTextBoxes();
        }

        protected void Course_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            set();
            CurrentQuestionSet_listbox.DataBind();
            GridView1.DataBind();
        }


        protected void Button1_Click1(object sender, EventArgs e)
        {

            SetList_Datatable = (DataTable)ViewState["data"];
            if (CurrentQuestionSet_listbox.Items.Count == 0)
            {
                SetList_Datatable.Rows.Add(1);
            }
            else 
            {
                for (int i = 0; i < CurrentQuestionSet_listbox.Items.Count - 1; i++)
                {
                    if (Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1 != Convert.ToInt32(CurrentQuestionSet_listbox.Items[i + 1].Value))
                    {
                        SetList_Datatable.Rows.Add(Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1);
                        break;
                    }
                }
                SetList_Datatable.Rows.Add(CurrentQuestionSet_listbox.Items.Count + 1);
            }

            ViewState["data"] = SetList_Datatable;
            CurrentQuestionSet_listbox.DataSource = SetList_Datatable;

            CurrentQuestionSet_listbox.DataBind();
            CurrentQuestionSet_listbox.SelectedIndex = CurrentQuestionSet_listbox.Items.Count - 1;
        }

        public void set()
        {
            string query = "select DISTINCT questionSet from questions_table WHERE courseID=@courseID";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(SetList_Datatable);
                sqlCon.Close();
                da.Dispose();
            }
            ViewState["data"] = SetList_Datatable;
            CurrentQuestionSet_listbox.DataMember = "questionSet";
            CurrentQuestionSet_listbox.DataValueField = "questionSet";
            CurrentQuestionSet_listbox.DataSource = SetList_Datatable;
            CurrentQuestionSet_listbox.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            questDescriptionTB.Text = GridView1.Rows[e.NewEditIndex].Cells[2].Text;
            type_radiobttn.SelectedValue = GridView1.Rows[e.NewEditIndex].Cells[3].Text;
            name_tb.Text = GridView1.Rows[e.NewEditIndex].Cells[5].Text;
            AnswerTextBoxes();
            SubmitBttn.Visible = false;
        }


        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
            GetAnswers();
            e.NewValues[0] = questDescriptionTB.Text.Trim();
            e.NewValues[1] = type_radiobttn.SelectedValue;
            e.NewValues[2] = answers;            
            e.NewValues[3] = name_tb.Text.Trim();

            clearText();
        }

        public void GetAnswers()
        {
            if (type_radiobttn.SelectedIndex == 1)
            {
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text; //rating
            }
            else if (type_radiobttn.SelectedIndex == 2)
            {
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text + "," + tb5.Text; //rating
            }
        }

        public void AnswerTextBoxes()
        {
            if (type_radiobttn.SelectedIndex == 0)
            {
                Panel1.Visible = false;
            }
            else if (type_radiobttn.SelectedIndex == 1) //1-4
            {
                tb1.Text = "Poor";
                tb2.Text = "Satisfactory";
                tb3.Text = "Good";
                tb4.Text = "Excellent";
                tb5.Visible = false;
                score5lbl.Visible = false;
                Panel1.Visible = true;
            }
            else                                       //1-5
            {
                tb1.Text = "Poor";
                tb2.Text = "Needs Improvement";
                tb3.Text = "Average";
                tb4.Text = "Good";
                tb5.Text = "Excellent";
                tb5.Visible = true;
                score5lbl.Visible = true;
                Panel1.Visible = true;
            }
        }

        public void clearText()
        {
            name_tb.Text = "";
            questDescriptionTB.Text = "";
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

            SubmitBttn.Visible = true;

            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 4)
            {
                e.Row.Cells[2].Enabled = false;
                e.Row.Cells[3].Enabled = false;
                e.Row.Cells[4].Enabled = false;
                e.Row.Cells[5].Enabled = false;
            }

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SubmitBttn.Visible = true;
            clearText();
        }
    }
}