using System;
using System.Collections.Generic;
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

        }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            _ = new CreateUser_Class(FirstNameTB.Text, LastNameTB.Text, EmailTB.Text, "Admin");
        }

        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            GridView1.DataBind();
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
    }
}