using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace peerreviewproject
{
    public partial class StudentGroupsPage : System.Web.UI.Page
    {
        public string sqlConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }
        }


        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            RosterGridView.DataBind();
        }

        protected void AddTeamBttn_click(object sender, EventArgs e)
        {

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (GridView1.Rows[i].Cells[2].Text == TextBox1.Text)
                {
                    Label1.Text = "Team # already exists";
                    Label1.Visible = true;
                    return;
                }
            }

            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("_insertTeam", sqlCon);         //numbers only
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@courseID", DropDownList1.SelectedValue);
                cmd.Parameters.AddWithValue("@name", Convert.ToInt32(TextBox1.Text));
                cmd.ExecuteNonQuery();
                sqlCon.Close();

            }
            GridView1.DataBind();
            RosterGridView.DataBind();
            TextBox1.Text = string.Empty;
            Label1.Text = "Team created";
            Label1.Visible = true;
        }

        protected void RosterGridView_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {
            bool flag = false;
            int i = 0;
            try
            {                                           //alert if letters are entered in team slot
                Convert.ToInt32(e.NewValues[0]);
            }
            catch
            {
                e.Cancel = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message Box", "<script language = 'javascript'>alert('Numbers Only')</script>");
                return;
            }

            while (i < GridView1.Rows.Count && !flag)
            {
                if (GridView1.Rows[i].Cells[1].Text == e.NewValues[0].ToString())
                {
                    flag = true;
                }
                i++;
            }

            if (!flag)
            {
                e.Cancel = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message Box", "<script language = 'javascript'>alert('Create Team # first before adding Student')</script>");
                return;
            }
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                string query = "UPDATE UserTeam_table SET teamID = a.teamID FROM UserTeam_table CROSS JOIN (SELECT teamID FROM teams_table WHERE (name = @name)) AS a WHERE (UserTeam_table.userID = @userID)";

                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@userID", Convert.ToInt32(e.Keys[0]));
                cmd.Parameters.AddWithValue("@name", Convert.ToInt32(e.NewValues[0]));
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                RosterGridView.DataBind();

            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                string query = "UPDATE teams_table SET name = @name WHERE (courseID = @courseID) AND (name = @old)";

                sqlCon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@old", Convert.ToInt32(e.OldValues[0]));
                cmd.Parameters.AddWithValue("@name", Convert.ToInt32(e.NewValues[0]));
                cmd.Parameters.AddWithValue("@courseID", DropDownList1.SelectedValue);
                cmd.ExecuteNonQuery();
                sqlCon.Close();

                RosterGridView.DataBind();

            }

        }

        protected void RosterGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowState.ToString().Contains("Edit"))
            {
                return;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(e.Row.Cells[1].Text) % 2 == 0)
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }

            }
            Label1.Visible = false;
            RosterGridView.Caption = "Course Roster - " + RosterGridView.Rows.Count.ToString() + " Total Students";
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            for (int i = 0; i < RosterGridView.Rows.Count; i++)
            {

                if (RosterGridView.Rows[i].Cells[1].Text == e.Values[0].ToString())
                {
                    Label1.Text = "Remove students from team before deleting";
                    Label1.Visible = true;
                    e.Cancel = true;
                    return;
                }
            }
            Label1.Text = "Team removed";
            Label1.Visible = true;
        }

        protected void HomeBttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }
    }
}