using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace peerreviewproject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            
        }

     

        protected void loginButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query = "SELECT COUNT(1) FROM User_table WHERE email=@email AND password=@password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
               
                sqlCmd.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@password", passwordBox.Text.Trim());
                string permission = "SELECT ID, type FROM User_table WHERE email=@email AND password=@password";
                string[] arr = new string[2];
                SqlCommand sqlCmd2 = new SqlCommand(permission, sqlCon);
                sqlCmd2.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                sqlCmd2.Parameters.AddWithValue("@password", passwordBox.Text.Trim());
                //string check = sqlCmd2.ExecuteScalar().ToString();
                SqlDataReader reader = sqlCmd2.ExecuteReader();
                while (reader.Read())
                {
                    arr[0] = reader["ID"].ToString();
                    arr[1] = reader["type"].ToString();
                }
                reader.Close();
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                if (count == 1 && arr[1] == "Student")
                {
                    Session["email"] = emailBox.Text.Trim();
                    Session["password"] = passwordBox.Text.Trim();

                    Response.Redirect("StudentMain.aspx");
                }
                else if (count == 1 && arr[1] == "Professor")
                {
                    Session["email"] = emailBox.Text.Trim();
                    Session["userID"] = arr[0];

                    Response.Redirect("TeacherMain.aspx");
                }
                else
                {
                    lblError.Visible = true;
                }
            }
        }
    }
}