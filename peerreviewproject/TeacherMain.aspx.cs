using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class TeacherMain : System.Web.UI.Page
    {
        int user;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblprofessor.Text = "Welcome " + Session["email"];
            user = Convert.ToInt32(Session["userID"]);
        }

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["userID"] = user;
            Response.Redirect("CreateClassPage.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["userID"] = user;
            Response.Redirect("AddUserstoClass.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Session["userID"] = user;
            Response.Redirect("CreatePeerReviewPage.aspx");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Session["userID"] = user;
            Response.Redirect("StudentGroupsPage");
        }
    }
}