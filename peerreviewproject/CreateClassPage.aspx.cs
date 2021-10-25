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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateCourse_btnclick(object sender, EventArgs e)
        {
            //prof id = 500
            CreateCourse_Class stan = new CreateCourse_Class(500, CourseDepartTB.Text, CourseNumberTB.Text, CourseNameTB.Text);
            

        }

    }
}