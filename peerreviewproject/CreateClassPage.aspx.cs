using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class CreateClassPage : System.Web.UI.Page
    {
        int user;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Convert.ToInt32(Session["userID"]);
        }

        protected void CreateCourse_btnclick(object sender, EventArgs e)
        {
            //prof id = 500
            CreateCourse_Class stan = new CreateCourse_Class(user, CourseDepartTB.Text, CourseNumberTB.Text, CourseNameTB.Text);
          //  GridView1.DataBind();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }
    }
}