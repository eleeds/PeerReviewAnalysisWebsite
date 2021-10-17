using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class StudentMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStudentDetails.Text = "User ID: " + Session["email"];
            
            
        }

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }
    }
}