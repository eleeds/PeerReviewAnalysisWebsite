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
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }

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

                string setName = "0";
                sqlCon.Open();
                string createQuestion_query = "INSERT INTO questions_table ([courseID], [question], [type], [correctResponses], [questionName], [questionSet], [classSurvey])" +
                    " VALUES (@courseID, @questionText, @type, @answers, @name, @set, @survey)";
                string ifnew_query = "SELECT MAX(reviewQuestionID) FROM questions_table";
                SqlCommand getID = new SqlCommand(ifnew_query, sqlCon);

                SqlCommand question_sqlCmd = new SqlCommand(createQuestion_query, sqlCon);

                question_sqlCmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                question_sqlCmd.Parameters.AddWithValue("@questionText", questDescriptionTB.Text.Trim());
                question_sqlCmd.Parameters.AddWithValue("@type", type_radiobttn.SelectedValue);
                question_sqlCmd.Parameters.AddWithValue("@answers", answers);
                question_sqlCmd.Parameters.AddWithValue("@name", name_tb.Text);
                question_sqlCmd.Parameters.AddWithValue("@survey", YesCheckBox.Checked.ToString());

                if (CurrentQuestionSet_listbox.Items.Count == 0 || CurrentQuestionSet_listbox.Items[0].Text != "1")
                {
                    question_sqlCmd.Parameters.AddWithValue("@set", 1);
                    setName = "1";
                }
                else if (CurrentQuestionSet_listbox.SelectedIndex == -1)                        //adds next number after most recent set number. if recent 1, new is 2
                {
                    for (int i = 2; i < CurrentQuestionSet_listbox.Items.Count + 2; i++)
                    {
                        if (CurrentQuestionSet_listbox.Items.FindByValue(i.ToString()) == null)
                        {
                            question_sqlCmd.Parameters.AddWithValue("set", i.ToString());
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
                GetQuestionSetsForCourse();

            }
            clearText();
        }

        protected void Type_radiobttn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnswerTextBoxes();
        }

        protected void Course_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetQuestionSetsForCourse();
            clearDuplicate();
        }

        protected void NewSetButton_click(object sender, EventArgs e)
        {
            NewSetToDataTable();
        }

        private void NewSetToDataTable()
        {

            SetList_Datatable = (DataTable)ViewState["data"];
            if (CurrentQuestionSet_listbox.Items.Count == 0 || CurrentQuestionSet_listbox.Items[0].Text != "1")
            {
                SetList_Datatable.Rows.Add(1);
            }
            else
            {
                for (int i = 0; i < CurrentQuestionSet_listbox.Items.Count; i++)
                {

                    try
                    {       //makes sure list item is a number, if not skip and place number before list item
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
                        else
                        {
                            SetList_Datatable.Rows.Add(i + 2);
                        }
                        SetIndex = i + 1;
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
            QuestionsInSetGridview.DataBind();
            moveAllQuestions(e.NewValues[4].ToString(), e.OldValues[4].ToString());
            GetQuestionSetsForCourse();
            SubmitBttn.Visible = true;
            CurrentQuestionSet_listbox.SelectedIndex = CurrentQuestionSet_listbox.Items.IndexOf(CurrentQuestionSet_listbox.Items.FindByValue(e.NewValues[4].ToString()));
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
                if (e.Row.RowIndex > 0)
                {
                    if (e.Row.RowIndex % 2 == 0)                                                        //hides other set numbers for cleaner look
                        e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#EFF3FB");
                    else
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                }

            }
        }

        protected void QuestionsInSetGridview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SubmitBttn.Visible = true;
            clearText();
        }

        protected void CurrentQuestionSet_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel2.Enabled = true;
            dueDatelbl.Text = "Current due date for Set " + CurrentQuestionSet_listbox.SelectedValue.ToString();
            getDateandShowStudents();
            clearDuplicate();
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
            QuestionsInSetGridview.DataBind();             //if all questions belonging to the set are deleted then the set is removed from the SetDueDate table
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
                GetQuestionSetsForCourse();
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
            else if (type_radiobttn.SelectedIndex == 3)
            {
                answers = tb1.Text + "," + tb2.Text; // Yes or No selection
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
                tb3.Visible = true;
                tb4.Visible = true;
                tb5.Visible = false;
                score3lbl.Visible = true;
                score4lbl.Visible = true;
                score5lbl.Visible = false;
                Panel1.Visible = true;
            }
            else if (type_radiobttn.SelectedIndex == 2)   //1-5
            {
                tb1.Text = "Poor";
                tb2.Text = "Needs Improvement";
                tb3.Text = "Average";
                tb4.Text = "Good";
                tb5.Text = "Excellent";
                tb3.Visible = true;
                tb4.Visible = true;
                tb5.Visible = true;
                score3lbl.Visible = true;
                score4lbl.Visible = true;
                score5lbl.Visible = true;
                Panel1.Visible = true;
            }
            else                                            //1-2
            {
                tb1.Text = "Yes";
                tb2.Text = "No";
                tb3.Visible = false;
                tb4.Visible = false;
                tb5.Visible = false;
                score3lbl.Visible = false;
                score4lbl.Visible = false;
                score5lbl.Visible = false;
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

        protected void Duplicate_Click(object sender, EventArgs e)
        {
            if (DuplicateBttn.Text == "Duplicate Set?")
            {
                DuplicateBttn.Text = "How many duplicates?";
                duplicateTB.Visible = true;
                SubmitDuplicateBttn.Visible = true;
            }
        }

        protected void SubmitDuplicate_Click(object sender, EventArgs e)
        {
            string currentValue = CurrentQuestionSet_listbox.SelectedValue;
            if (duplicateTB.Text != "")
            {
                int count = Convert.ToInt32(duplicateTB.Text);
                string setName = "";
                if (count <= 15)
                {
                    using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
                    {
                        sqlCon.Open();
                        for (int i = 0; i < count; i++)
                        {
                            foreach (GridViewRow rows in QuestionsInSetGridview.Rows)
                            {
                                string InsertCopyQuery = "INSERT INTO questions_table ([courseID], [question], [type], " +
                                    "[correctResponses], [questionName], [questionSet], [classSurvey])" +
                                " VALUES (@courseID, @questionText, @type, @answers, @name, @set, @survey)";
                                SqlCommand DuplicateSQL = new SqlCommand(InsertCopyQuery, sqlCon);
                                DuplicateSQL.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                                DuplicateSQL.Parameters.AddWithValue("@questionText", rows.Cells[2].Text);
                                DuplicateSQL.Parameters.AddWithValue("@type", rows.Cells[3].Text);
                                DuplicateSQL.Parameters.AddWithValue("@answers", rows.Cells[4].Text);
                                DuplicateSQL.Parameters.AddWithValue("@name", rows.Cells[5].Text);
                                DuplicateSQL.Parameters.AddWithValue("@set", rows.Cells[6].Text + " (" + (i + 2).ToString() + ")");
                                DuplicateSQL.Parameters.AddWithValue("@survey", YesCheckBox.Checked.ToString());
                                DuplicateSQL.ExecuteNonQuery();
                                setName = rows.Cells[6].Text;
                            }

                            string SetDueDateTime = Convert.ToDateTime(DueDateTB.Text).AddDays((i + 1) * 7).AddHours(23).AddMinutes(59).AddSeconds(59).ToString();
                            string InsertDueDateQuery = "INSERT INTO SetDueDates_table ([courseID], [dueDate], [questionSet]) VALUES (@courseID, @dueDate, @questionSet)";
                            SqlCommand cmd = new SqlCommand(InsertDueDateQuery, sqlCon);
                            cmd.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                            cmd.Parameters.AddWithValue("@dueDate", SetDueDateTime);
                            cmd.Parameters.AddWithValue("@questionSet", setName + " (" + (i + 2).ToString() + ")");
                            cmd.ExecuteNonQuery();

                        }
                        sqlCon.Close();
                    }
                    clearDuplicate();
                    GetQuestionSetsForCourse();
                }
                else
                {

                }
            }
            CurrentQuestionSet_listbox.SelectedValue = currentValue;
        }
        private void clearDuplicate()
        {
            DuplicateBttn.Text = "Duplicate Set?";
            duplicateTB.Visible = false;
            duplicateTB.Text = "";
            SubmitDuplicateBttn.Visible = false;
        }

        private void moveAllQuestions(string newSet, string oldSet)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string updateOtherQuestions = "UPDATE questions_table SET questionSet=@newSet WHERE (questionSet=@oldSet) AND (courseID=@courseID)";
                SqlCommand updateWholeSetSQL = new SqlCommand(updateOtherQuestions, sqlCon);
                updateWholeSetSQL.Parameters.AddWithValue("@newSet", newSet);
                updateWholeSetSQL.Parameters.AddWithValue("@oldSet", oldSet);
                updateWholeSetSQL.Parameters.AddWithValue("@courseID", Course_listbox.SelectedValue);
                updateWholeSetSQL.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        protected void HomeBttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }

    }
}