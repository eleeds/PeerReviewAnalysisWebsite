using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class AdminPage : System.Web.UI.Page
    {
        string sqlconn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }
            if (!this.IsPostBack)
            {
                this.SearchUsers();
            }

        }

        private void SearchUsers()
        {

            using (SqlConnection sqlCon = new SqlConnection(sqlconn))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                string sql = "SELECT { fn CONCAT(firstName, { fn CONCAT(' ', lastName) }) } AS Name, type as Type, email as Email FROM User_table WHERE type != N'Admin'  ORDER BY type";
                if (!string.IsNullOrEmpty(txtSearch.Text.Trim()) && txtSearch.Text.Trim() != "Search box")
                {
                    sql = "SELECT { fn CONCAT(firstName, { fn CONCAT(' ', lastName) }) } AS Name, type as Type, email as Email FROM User_table WHERE type != N'Admin'" +
                        " AND (firstName LIKE @search + '%') OR (lastName LIKE @search + '%') ORDER BY type";
                    cmd.Parameters.AddWithValue("@search", txtSearch.Text.Trim());
                }
                cmd.CommandText = sql;
                cmd.Connection = sqlCon;

                DataTable userDataTable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(userDataTable);
                UserGridview.DataSource = userDataTable;
                UserGridview.DataBind();

                sqlCon.Close();
            }

        }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            TextBox[] boxes = { FirstNameTB, LastNameTB, EmailTB};
            if (TextBox_Check(boxes))
            {
                Label1.Visible = false;
                return;
            }

            CreateUser_Class newProfessor = new CreateUser_Class();
            if (newProfessor.newAccount(FirstNameTB.Text, LastNameTB.Text, EmailTB.Text, "Admin"))
            {
                SearchUsers();
                Label1.Visible = true;
            }
            else 
                Response.Write("<script>alert('Email already exists in database');</script>");
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconn))
            {
                sqlCon.Open();
                string nextQuestionQuery = "DELETE FROM Course_table FROM Course_table inner join Course_access_table on Course_table.courseID = Course_access_table.courseID Where userID = @userID";
                SqlCommand sqlCmd = new SqlCommand(nextQuestionQuery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@userID", e.Keys[0]);
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }
        }

        protected void Search(object sender, EventArgs e)
        {
            this.SearchUsers();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            UserGridview.PageIndex = e.NewPageIndex;
            this.SearchUsers();
        }

        protected void UserGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconn))
            {
                sqlCon.Open();
                string userIDquery = "SELECT ID FROM User_table WHERE CONCAT(firstName, CONCAT(' ', lastName)) = @name";
                SqlCommand sqlCmd = new SqlCommand(userIDquery, sqlCon);
                sqlCmd.Parameters.AddWithValue("@name", e.Values[0]);
                int user = Convert.ToInt32(sqlCmd.ExecuteScalar());


                string deleteQuery = "DELETE FROM User_table WHERE CONCAT(firstName, CONCAT(' ', lastName)) = @name";
                SqlCommand sqlCmd2 = new SqlCommand(deleteQuery, sqlCon);
                sqlCmd2.Parameters.AddWithValue("@name", e.Values[0]);
                sqlCmd2.ExecuteNonQuery();

                string deleteFromCourseQuery = "DELETE FROM Course_table FROM Course_table inner join Course_access_table on Course_table.courseID = Course_access_table.courseID Where userID = @userID";
                SqlCommand sqlCmd3 = new SqlCommand(deleteFromCourseQuery, sqlCon);
                sqlCmd3.Parameters.AddWithValue("@userID", user);
                sqlCmd3.ExecuteNonQuery();
                sqlCon.Close();
                SearchUsers();
            }
        }

        protected void HomeBttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }

        protected void UserGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton Remove = e.Row.FindControl("LinkButton1") as LinkButton;
                Remove.Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to remove {0}?')", e.Row.Cells[1].Text));
            }
        }

        public bool TextBox_Check(TextBox[] boxes)
        {
            int count = 0;

            foreach (TextBox tb in boxes)
            {
                if (tb.Text == string.Empty)
                {
                    tb.BorderColor = System.Drawing.Color.Red;
                    count++;
                }
                else tb.BorderColor = System.Drawing.Color.Empty;
            }
            if (count == 0)
                return false;
            else return true;
        }
    }
}