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

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            PasswordManagement_Class Security = new PasswordManagement_Class();
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();

                string permission = "SELECT ID, type, tempPass FROM User_table WHERE email=@email AND CONVERT(VARCHAR(50), DECRYPTBYPASSPHRASE('USI2021', password))=@password";
                string[] UserInfo_Array = new string[3];                               //grabs user's permission type if login information correct
                SqlCommand UserTypePassCheck_sqlCMD = new SqlCommand(permission, sqlCon);
                UserTypePassCheck_sqlCMD.Parameters.AddWithValue("@email", emailBox.Text.Trim());
                UserTypePassCheck_sqlCMD.Parameters.AddWithValue("@password", passwordBox.Text.Trim());
                SqlDataReader reader = UserTypePassCheck_sqlCMD.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo_Array[0] = reader["ID"].ToString();
                    UserInfo_Array[1] = reader["type"].ToString();
                    UserInfo_Array[2] = reader["tempPass"].ToString();
                }
                reader.Close();

                if (UserInfo_Array[0] != null)
                {
                    if (UserInfo_Array[1] == "Student" && UserInfo_Array[2] != "True")
                    {
                        Session["userID"] = UserInfo_Array[0];
                        Session["email"] = emailBox.Text.Trim();

                        Response.Redirect("StudentMain.aspx");
                    }
                    else if (UserInfo_Array[1] != "Student" && UserInfo_Array[2] != "True")
                    {
                        Session["email"] = emailBox.Text.Trim();
                        Session["userID"] = UserInfo_Array[0];
                        Session["type"] = UserInfo_Array[1];

                        Response.Redirect("TeacherMain.aspx");
                    }
                    else
                    {
                        lblError.Visible = true;
                    }
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