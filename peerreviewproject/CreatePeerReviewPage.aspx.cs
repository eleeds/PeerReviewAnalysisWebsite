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
        int check;
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
            GetQuestionSetsForCourse();
        }

        protected void SubmitBttn_Click(object sender, EventArgs e)
        {

            GetAnswers();

            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                int recentID = 0;
                int setNumber;
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
                    setNumber = 1;
                }
                else if (CurrentQuestionSet_listbox.SelectedIndex == -1)
                {
                    question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.Items[CurrentQuestionSet_listbox.Items.Count - 1]);
                    setNumber = Convert.ToInt32(CurrentQuestionSet_listbox.Items[CurrentQuestionSet_listbox.Items.Count - 1].Value);
                }
                else
                {
                    question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.SelectedValue);
                    setNumber = Convert.ToInt32(CurrentQuestionSet_listbox.SelectedValue);
                }
                question_sqlCmd.ExecuteNonQuery();
                setDueDate(sqlCon, setNumber);

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
            GetQuestionSetsForCourse();
            CurrentQuestionSet_listbox.DataBind();
            GridView1.DataBind();
        }


        protected void Button1_Click1(object sender, EventArgs e)
        {
            NewSetToDataTable();

        }

        private void NewSetToDataTable()
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

            DataView dv = SetList_Datatable.DefaultView;
            dv.Sort = "questionSet";
            SetList_Datatable = dv.ToTable();
            ViewState["data"] = SetList_Datatable;
            CurrentQuestionSet_listbox.DataSource = SetList_Datatable;
            CurrentQuestionSet_listbox.DataBind();
            CurrentQuestionSet_listbox.SelectedIndex = CurrentQuestionSet_listbox.Items.Count - 1;
        }
        private void GetQuestionSetsForCourse()
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

        private void DateFormat(string year)
        {
            year = year.Substring(0, year.IndexOf(' '));             //special format for calendar year-month-day
            string month = year.Substring(0, year.IndexOf('/'));
            year = year.Substring(year.IndexOf('/') + 1);
            string day = year.Substring(0, year.IndexOf('/'));
            year = year.Substring(year.IndexOf('/') + 1);
            if (day.Length == 1)
                day = "0" + day;
            if (month.Length == 1)
                month = "0" + month;
            DueDateTB.Text = year + "-" + month + "-" + day;
        }
        private void GetAnswers()
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

        private void AnswerTextBoxes()
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

        private void clearText()
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
            //int[] arr = CurrentQuestionSet_listbox.Items.;
            DataView dv = SetList_Datatable.DefaultView;
            //dv.Sort = "questionSet";
        }

        private void setDueDate(SqlConnection sqlCon, int setNumber)
        {
            string AlreadyExistQuery = "SELECT COUNT(1) FROM SetDueDates_table WHERE courseID=@courseID AND questionSet=@questionSet";
            SqlCommand checkCMD = new SqlCommand(AlreadyExistQuery, sqlCon);
            checkCMD.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
            checkCMD.Parameters.AddWithValue("@questionSet", setNumber);
            int exist = Convert.ToInt32(checkCMD.ExecuteScalar());
            if (exist > 0)                  //if a due date already exists for that question set, ignore inserting new one
            {
                return;
            }
            string SetDueDateTime = DueDateTB.Text + " 11:59:59 PM";
            string InsertDueDateQuery = "INSERT INTO SetDueDates_table ([courseID], [dueDate], [questionSet]) VALUES (@courseID, @dueDate, @questionSet)";
            SqlCommand cmd = new SqlCommand(InsertDueDateQuery, sqlCon);
            cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
            cmd.Parameters.AddWithValue("@dueDate", SetDueDateTime);
            cmd.Parameters.AddWithValue("@questionSet", setNumber);
            cmd.ExecuteNonQuery();
        }

        protected void CurrentQuestionSet_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel2.Enabled = true;
            dueDatelbl.Text = "Current due date for Set# " + CurrentQuestionSet_listbox.SelectedValue.ToString();
            getDate();
        }

        private void getDate()
        {
            string query = "SELECT dueDate FROM SetDueDates_table WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                    if (cmd.ExecuteScalar() != null)
                    { 
                       string year = cmd.ExecuteScalar().ToString();            //fix new set from duplicates
                       DateFormat(year);
                    }
                
                sqlCon.Close();
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string deletequery = "DELETE FROM Response_table WHERE reviewQuestionID=@reviewQuestionID";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))     //manually deleting from response table since cascade won't work
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(deletequery, sqlCon);
                cmd.Parameters.AddWithValue("@reviewQuestionID", e.Keys[0]);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }    
            //grabs recently deleted set number
            check = Convert.ToInt32(e.Values[4]);
        }
        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            GridView1.DataBind();                   //if all questions belonging to the set are deleted then the set is removed from the SetDueDate table
            if (GridView1.Rows.Count == 0)
            {
                string query = "DELETE FROM SetDueDates_table WHERE courseID=@courseID AND questionSet=@questionSet";
                using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
                 {
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                    cmd.Parameters.AddWithValue("@questionSet", check);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                 }
            }
        }

        protected void DueDateTB_TextChanged(object sender, EventArgs e)        //update due date for selected set
        {
            string query = "UPDATE SetDueDates_table SET dueDate = @dueDate WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@dueDate", DueDateTB.Text + " 11:59:59 PM");
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

    }
}