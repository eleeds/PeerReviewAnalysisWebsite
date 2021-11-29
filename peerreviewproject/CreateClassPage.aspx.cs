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
            Years();
        }

        protected void CreateCourse_btnclick(object sender, EventArgs e)
        {
            if(CourseNumberTB.Text == "" || CourseNameTB.Text == "")    
            {
                WarningLabel.Text = "Please complete each section";
                WarningLabel.Visible = true;
                WarningLabel.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                CreateCourse_Class NewCourse = new CreateCourse_Class(user, CollegeDroplist.SelectedItem.Text, CourseNumberTB.Text,
                    CourseNameTB.Text, SemesterDropDown.SelectedValue, Convert.ToInt32(YearDropList.SelectedValue));
                CurrentCourseGridView.DataBind();
                WarningLabel.Text = "Course Created";
                WarningLabel.ForeColor = System.Drawing.Color.Green;
                WarningLabel.Visible = true;
                Clear();
            }
            catch 
            {
                WarningLabel.Text = "Course not created";
                WarningLabel.ForeColor = System.Drawing.Color.Red;
                WarningLabel.Visible = true;
            }

        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }

        private void Years()        //populate dropdown list with years based off of current year
        {
            int year = DateTime.Now.Year;
            for (int i = year; i <= year + 7; i++)
            {
                ListItem li = new ListItem(i.ToString());
                YearDropList.Items.Add(li);
            }
        }

        private void Clear()
        {
            CourseNumberTB.Text = "";
            CourseNameTB.Text = "";
        }
    }
}