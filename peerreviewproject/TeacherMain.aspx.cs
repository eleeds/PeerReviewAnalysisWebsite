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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblprofessor.Text = "Welcome " + Session["email"];

            if (Session["type"].ToString() == "Admin")
            {
                AdminButton.Visible = true;
                AdminButton.Enabled = true;
                Session["type"] = "Admin";
            }
            else 
            {
                Session["type"] = "Professor";
            }
        }

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }

        protected void EditClassesbttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateClassPage.aspx");
        }

        protected void EditStudentsbttn_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("AddUserstoClass.aspx");
        }

        protected void EditReviewsbttn_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("CreatePeerReviewPage.aspx");
        }

        protected void EditGroupsbttn_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("StudentGroupsPage.aspx");
        }

        protected void TeacherCourseGridview_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Session["course"] = TeacherCourseGridview.Rows[TeacherCourseGridview.SelectedIndex].Cells[1].Text;
            Response.Redirect("CourseReviews.aspx");
        }

        protected void ChangePassButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePass.aspx");
        }

        protected void AdminButton_Click(object sender, EventArgs e)
        {
            if (Session["type"].ToString() == "Admin")
                Response.Redirect("AdminPage.aspx");
        }
    }
}