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
                string query = "SELECT COUNT(1) FROM User_table WHERE email=@email AND CONVERT(VARCHAR(50), DECRYPTBYPASSPHRASE(N'USI2021', password))=@password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
               
                sqlCmd.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@password", passwordBox.Text.Trim());
                string permission = "SELECT ID, type, tempPass FROM User_table WHERE email=@email AND CONVERT(VARCHAR(50), DECRYPTBYPASSPHRASE(N'USI2021', password))=@password";
                string[] UserInfo_Array = new string[3];                               //grabs user's permission type if login information correct
                SqlCommand sqlCmd2 = new SqlCommand(permission, sqlCon);
                sqlCmd2.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                sqlCmd2.Parameters.AddWithValue("@password", passwordBox.Text.Trim());
                SqlDataReader reader = sqlCmd2.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo_Array[0] = reader["ID"].ToString();
                    UserInfo_Array[1] = reader["type"].ToString();
                    UserInfo_Array[2] = reader["tempPass"].ToString();
                }
                reader.Close();
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                if (count == 1 && UserInfo_Array[2].ToString() == "True")      //if tempPass = true, user currently has temp password.
                {                                                   //they are sent to the change pass page
                    Session["email"] = emailBox.Text.Trim();
                    Session["userID"] = UserInfo_Array[0];
                    Session["type"] = UserInfo_Array[1];

                    Response.Redirect("ChangePass.aspx");
                }

                if (count == 1 && UserInfo_Array[1] == "Student")
                {
                    Session["email"] = emailBox.Text.Trim();
                    Session["password"] = passwordBox.Text.Trim();

                    Response.Redirect("StudentMain.aspx");
                }
                else if (count == 1 && UserInfo_Array[1] == "Professor")
                {
                    Session["email"] = emailBox.Text.Trim();
                    Session["userID"] = UserInfo_Array[0];

                    Response.Redirect("TeacherMain.aspx");
                }
                else
                {
                    lblError.Visible = true;
                }
            }
        }

        protected void ForgotBttn_click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPass.aspx");   
        }
    }
}