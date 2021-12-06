using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class ChangePass : System.Web.UI.Page
    {
        PasswordManagement_Class changePass = new PasswordManagement_Class();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session.IsNewSession && Request.QueryString.Count != 0)
            {
                string user = Request.QueryString["USER"].ToString();
                string token = HttpContext.Current.Server.UrlDecode(Request.QueryString["TOKEN"]);
                string[] check = changePass.GetUser(user, token);
                if (check[0] != null)
                {
                    Session["userID"] = check[0];
                    Session["email"] = user;
                    Session["type"] = check[1];
                }
                else
                {
                    Session["error"] = "0";
                    Response.Redirect("https://localhost:44313/ForgotPass");
                    return;
                }
            }
            else
            {
                Session["error"] = "0";
                Response.Redirect("https://localhost:44313/ForgotPass");
            }
        }

        protected void SubmitBttn_click(object sender, EventArgs e)
        {
            if (passTB1.Text.Equals(PassTB2.Text) && passTB1.Text.Length > 7)
            {
                
                changePass.ChangePass(Session["email"].ToString(), passTB1.Text);
                Label1.Text = "Password changed successfully. Directing to user dashboard.";
                Label1.Visible = true;

                Session["email"] = Session["email"];
                if (Session["type"].ToString() == "Student")
                {
                    Response.AddHeader("REFRESH", "5;URL=StudentMain.aspx"); //creates 5 sec delay before student redirection
                }
                else
                {
                    Response.AddHeader("REFRESH", "5;URL=TeacherMain.aspx"); //creates 5 sec delay before teacher redirection
                }

            }
            else if (passTB1.Text.Length < 7)
            {
                Label1.Text = "Password needs to be at least 8 characters. Try again.";
                Label1.Visible = true;
                return;
            }
            else
            {
                Label1.Text = "Passwords don't match. Try again";
                Label1.Visible = true;
                
            }
        }
    }
}