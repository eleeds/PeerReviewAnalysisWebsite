using System;
using System.Collections.Generic;
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitBttn_Click(object sender, EventArgs e)
        {
            string answers = "Feedback";
            if (type_radiobttn.SelectedIndex == 1)
            {
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text; //rating
            }
            else if(type_radiobttn.SelectedIndex == 2)
            {
                answers = tb1.Text + "," + tb2.Text + "," + tb3.Text + "," + tb4.Text + "," + tb5.Text; //rating
            }
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
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
                if (CurrentQuestionSet_listbox.SelectedIndex == -1)
                {
                    //question_sqlCmd.Parameters.AddWithValue("@set", recentID + 1);

                }
                // else question_sqlCmd.Parameters.AddWithValue("@set", CurrentQuestionSet_listbox.SelectedValue);

                if (CurrentQuestionSet_listbox.Items.Count == 0 || check == 1)
                {
                    question_sqlCmd.Parameters.AddWithValue("@set", 1);
                }
                else if (CurrentQuestionSet_listbox.SelectedIndex == -1)
                {
                    question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.Items[CurrentQuestionSet_listbox.Items.Count - 1]);
                }
                else question_sqlCmd.Parameters.AddWithValue("set", CurrentQuestionSet_listbox.SelectedValue);

                question_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

                CurrentQuestionSet_listbox.DataBind();
                
            }
            name_tb.Text = "";
            questDescriptionTB.Text = "";
        }

        protected void type_radiobttn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (type_radiobttn.SelectedIndex == 0) 
            {
                Panel1.Visible = false;
            }
            else if(type_radiobttn.SelectedIndex == 1) //1-4
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

        protected void Course_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentQuestionSet_listbox.DataBind();
            GridView1.DataBind();
        }


        protected void Button1_Click1(object sender, EventArgs e)
        {
            //CurrentQuestionSet_listbox.Items.Add(1.ToString());
            Label1.Text = ("This will be set 1");
            check = 1;
        }
    }
}