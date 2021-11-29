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
        string check;
        int SetIndex;
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
            if (Course_listbox.SelectedIndex == -1) //no course yet
            {
                return;
            }
            GetAnswers();

            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                int recentID = 0;
                string setName = "0";
                sqlCon.Open();
                string createQuestion_query = "INSERT INTO questions_table ([courseID], [question], [type], [correctResponses], [questionName], [questionSet], [classSurvey])" +
                    " VALUES (@courseID, @questionText, @type, @answers, @name, @set, @survey)";
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
                question_sqlCmd.Parameters.AddWithValue("@survey", YesCheckBox.Checked.ToString());


                if (CurrentQuestionSet_listbox.Items.Count == 0)
                {
                    question_sqlCmd.Parameters.AddWithValue("@set", 1);         
                    setName = "1";
                }
                else if (CurrentQuestionSet_listbox.SelectedIndex == -1)
                {
                    for (int i = 0; i < CurrentQuestionSet_listbox.Items.Count; i++)
                    {
                        try
                        {
                            int ifNumbercheck = (Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1);    //next set will become last number plus 1
                            if (Convert.ToInt32(CurrentQuestionSet_listbox.Items[i + 1].Value) != ifNumbercheck)
                            {
                               question_sqlCmd.Parameters.AddWithValue("set", (ifNumbercheck + 1).ToString());     //example, last #set is 3, new # set is 4
                               break;
                            }
                            
                        }
                        catch
                        {
                            question_sqlCmd.Parameters.AddWithValue("set", (Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1)).ToString();
                            break;
                        }
                    }
                    
                }
                else
                {
                    question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.SelectedValue);
                    setName = CurrentQuestionSet_listbox.SelectedValue;
                }
                question_sqlCmd.ExecuteNonQuery();
                setDueDate(sqlCon, setName);

                sqlCon.Close();
                QuestionsInSetGridview.DataBind();
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
        }

        protected void NewSetButton_click(object sender, EventArgs e)
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
                    try
                    {
                        if (Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1 != Convert.ToInt32(CurrentQuestionSet_listbox.Items[i + 1].Value))
                        {
                            SetList_Datatable.Rows.Add(Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1);
                            SetIndex = Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1;
                            break;
                        }
                    }
                    catch 
                    {
                        if (CurrentQuestionSet_listbox.Items.Count == 1)
                        {
                            SetList_Datatable.Rows.Add(1);
                        }
                        SetList_Datatable.Rows.Add(Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1);
                        SetIndex = Convert.ToInt32(CurrentQuestionSet_listbox.Items[i].Value) + 1;
                        break;
                    }
                }
                
            }

            DataView dv = SetList_Datatable.DefaultView;
            dv.Sort = "questionSet";
            SetList_Datatable = dv.ToTable();
            ViewState["data"] = SetList_Datatable;
            CurrentQuestionSet_listbox.DataSource = SetList_Datatable;              //places selected index at newly created question set
            CurrentQuestionSet_listbox.DataBind();
            CurrentQuestionSet_listbox.SelectedIndex = CurrentQuestionSet_listbox.Items.IndexOf(CurrentQuestionSet_listbox.Items.FindByValue(SetIndex.ToString()));
            QuestionsInSetGridview.DataBind();
        }

        protected void QuestionsInSetGridview_RowEditing(object sender, GridViewEditEventArgs e)
        {
            questDescriptionTB.Text = QuestionsInSetGridview.Rows[e.NewEditIndex].Cells[2].Text;
            type_radiobttn.SelectedValue = QuestionsInSetGridview.Rows[e.NewEditIndex].Cells[3].Text;
            name_tb.Text = QuestionsInSetGridview.Rows[e.NewEditIndex].Cells[5].Text;
            AnswerTextBoxes();
            SubmitBttn.Visible = false;
        }

        protected void QuestionsInSetGridview_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {           
            GetAnswers();
            e.NewValues[0] = questDescriptionTB.Text.Trim();
            e.NewValues[1] = type_radiobttn.SelectedValue;
            e.NewValues[2] = answers;            
            e.NewValues[3] = name_tb.Text.Trim();
            SetIndex = CurrentQuestionSet_listbox.SelectedIndex;
            clearText();

            if (e.OldValues[4] != e.NewValues[4])               //update question set name in due date table 
            {
                UpdateForDueDate(e.OldValues[4].ToString(), e.NewValues[4].ToString());
            }
        }

        protected void QuestionsInSetGridview_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            GetQuestionSetsForCourse();
            SubmitBttn.Visible = true;
            CurrentQuestionSet_listbox.SelectedIndex = SetIndex;
        }

        protected void QuestionsInSetGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 4)
            {
                e.Row.Cells[2].Enabled = false;
                e.Row.Cells[3].Enabled = false;
                e.Row.Cells[4].Enabled = false;
                e.Row.Cells[5].Enabled = false;
                getClassSurvey();
                if (e.Row.RowIndex != 0 && e.Row.Cells[7].Text != "Class Survey")
                {
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }
               
            }
        }

        protected void QuestionsInSetGridview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SubmitBttn.Visible = true;
            clearText();
            DataView dv = SetList_Datatable.DefaultView;
            //dv.Sort = "questionSet";
        }

        protected void CurrentQuestionSet_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel2.Enabled = true;
            dueDatelbl.Text = "Current due date for Set " + CurrentQuestionSet_listbox.SelectedValue.ToString();
            getDateandShowStudents();
        }

        protected void QuestionsInSetGridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            check = e.Values[4].ToString();
        }
        protected void QuestionsInSetGridview_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            QuestionsInSetGridview.DataBind();                   //if all questions belonging to the set are deleted then the set is removed from the SetDueDate table
            if (QuestionsInSetGridview.Rows.Count == 0)
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
                if (DueDateExistYet(sqlCon, CurrentQuestionSet_listbox.SelectedValue))              //if already in dueDate table, update with new date
                {                                                                                   //date is created if one doesn't exist
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.Parameters.AddWithValue("@dueDate", DueDateTB.Text + " 11:59:59 PM");
                    cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                    cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                    cmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
        }

        protected void NoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NoCheckBox.Checked)
            {
                YesCheckBox.Checked = false;
                UpdateForSurvey(0);
                QuestionsInSetGridview.DataBind();
            }
        }

        protected void YesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (YesCheckBox.Checked)
            {
                NoCheckBox.Checked = false;
                UpdateForSurvey(1);
                QuestionsInSetGridview.DataBind();
            }
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
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text; // 1-4 rating or 1-4 multiple choice
            }
            else if (type_radiobttn.SelectedIndex == 2)
            {
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text + "," + tb5.Text; // 1-5 rating or 1-5 multiple choice
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

        private void setDueDate(SqlConnection sqlCon, string setName)
        {
            DueDateExistYet(sqlCon, setName);
        }

        private bool DueDateExistYet(SqlConnection sqlCon, string setName)
        {
            string AlreadyExistQuery = "SELECT COUNT(1) FROM SetDueDates_table WHERE courseID=@courseID AND questionSet=@questionSet";
            SqlCommand checkCMD = new SqlCommand(AlreadyExistQuery, sqlCon);
            checkCMD.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
            checkCMD.Parameters.AddWithValue("@questionSet", setName);
            int exist = Convert.ToInt32(checkCMD.ExecuteScalar());
            if (exist > 0)                  //if a due date already exists for that question set, ignore inserting new one
            {
                return true;
            }
            else
            {
                string SetDueDateTime = DueDateTB.Text + " 11:59:59 PM";
                string InsertDueDateQuery = "INSERT INTO SetDueDates_table ([courseID], [dueDate], [questionSet]) VALUES (@courseID, @dueDate, @questionSet)";
                SqlCommand cmd = new SqlCommand(InsertDueDateQuery, sqlCon);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@dueDate", SetDueDateTime);
                cmd.Parameters.AddWithValue("@questionSet", setName);
                cmd.ExecuteNonQuery();
                return false;
            } 
        }

        private void getDateandShowStudents()
        {
            string[] info = new string[2];
            string query = "SELECT dueDate, showStudents FROM SetDueDates_table WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    info[0] = reader["dueDate"].ToString();
                    info[1] = reader["showStudents"].ToString();
                }
                reader.Close();
                if (info[0] != null)
                    { 
                       string year = info[0];            
                       DateFormat(year);
                    }
                if (info[1] == "True")
                {
                    YesCheckBoxStudents.Checked = true;
                    NoCheckBoxStudents.Checked = false;
                }
                else
                {
                    YesCheckBoxStudents.Checked = false;
                    NoCheckBoxStudents.Checked = true;
                }
                sqlCon.Close();
            }
        }
        private void UpdateForSurvey(int choice)
        {
            string query = "UPDATE questions_table SET classSurvey = @classSurvey WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@classSurvey", choice);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        } 
        private void getClassSurvey()
        {
            if (QuestionsInSetGridview.Rows.Count != 0)
            {
                if (QuestionsInSetGridview.DataKeys[0].Values[1].ToString() == "False")
                {
                    NoCheckBox.Checked = true;
                    YesCheckBox.Checked = false;
                }
                else
                {
                    NoCheckBox.Checked = false;
                    YesCheckBox.Checked = true;
                }
            }
        }
        private void UpdateForDueDate(string Oldvalue, string Newvalue)
        {
            string query = "UPDATE SetDueDates_table SET questionSet = @newquestionSet WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@newquestionSet", Newvalue);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", Oldvalue);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }
        private void UpdateForShowStudents(int choice)
        {
            string query = "UPDATE SetDueDates_table SET showStudents = @showStudents WHERE courseID=@courseID AND questionSet=@questionSet";
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@showStudents", choice);
                cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                cmd.Parameters.AddWithValue("@questionSet", CurrentQuestionSet_listbox.SelectedValue);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        protected void NoCheckBoxStudents_CheckedChanged(object sender, EventArgs e)
        {
            if (NoCheckBoxStudents.Checked)
            {
                YesCheckBoxStudents.Checked = false;
                UpdateForShowStudents(0);
                QuestionsInSetGridview.DataBind();
            }
        }

        protected void YesCheckBoxStudents_CheckedChanged(object sender, EventArgs e)
        {
            if (YesCheckBoxStudents.Checked)
            {
                NoCheckBoxStudents.Checked = false;
                UpdateForShowStudents(1);
                QuestionsInSetGridview.DataBind();
            }
        }
    }
}