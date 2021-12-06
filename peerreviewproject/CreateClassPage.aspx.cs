using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class CreateClassPage : System.Web.UI.Page
    {
        int user;
                public string sqlConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
                int count = CurrentCourseGridView.Rows.Count;
                CurrentCourseGridView.DataBind();
                if (count == CurrentCourseGridView.Rows.Count)
                {
                    WarningLabel.Text = "Course already exists";
                    WarningLabel.ForeColor = System.Drawing.Color.Red;
                    WarningLabel.Visible = true;
                    return;
                }
                if (AnotherProfessorTextBox.Text.Length > 5)
                {
                    if (FindProfessor(NewCourse))
                    {
                        WarningLabel.Text = "Course Created and Professor added";
                        WarningLabel.ForeColor = System.Drawing.Color.Green;
                        WarningLabel.Visible = true;
                        Clear();
                    }
                    else
                    {
                        WarningLabel.Text = "Course Created but Professor wasn't found";
                        WarningLabel.ForeColor = System.Drawing.Color.Green;
                        WarningLabel.Visible = true;
                        Clear();
                    }
                }
                else
                {
                    WarningLabel.Text = "Course Created";
                    WarningLabel.ForeColor = System.Drawing.Color.Green;
                    WarningLabel.Visible = true;
                    Clear();
                }
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
            AnotherProfessorTextBox.Text = "";
        }

        private bool FindProfessor(CreateCourse_Class AddtoCourse)                      //finds added professor by email if it exists in user table
        {
            using (SqlConnection sqlCon = new SqlConnection(AddtoCourse.sqlConnection))
            {
                sqlCon.Open();
                string CoProfessor_query = "SELECT ID FROM User_table WHERE email=@email AND type != 'Student'";
                SqlCommand sqlCmd = new SqlCommand(CoProfessor_query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@email", AnotherProfessorTextBox.Text.Trim());
                int ID = Convert.ToInt32(sqlCmd.ExecuteScalar());
                sqlCon.Close();
                if (ID != 0)
                {
                    AddtoCourse.CourseProf = ID;
                    AddtoCourse.CourseAccess();
                    return true;
                }
                return false;
            }
        }

        protected void CurrentCourseGridView_DataBound(object sender, EventArgs e)      //underlines course if it contains multiple professors
        {
            int i = 0;
            foreach (DataKey key in CurrentCourseGridView.DataKeys)
            {
                if (ProfessorCount(key))
                {
                    CurrentCourseGridView.Rows[i].Cells[0].Font.Underline = true;
                }
                i++;
            }
        }

        private bool ProfessorCount(DataKey key)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlConnection))
            {
                sqlCon.Open();
                string numberOfProfessors = "SELECT COUNT(1) FROM Course_access_table WHERE courseID=@courseID AND permissionType != 'Student'";
                SqlCommand sqlCmd = new SqlCommand(numberOfProfessors, sqlCon);
                sqlCmd.Parameters.AddWithValue("@courseID", key.Value);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                sqlCon.Close();
                if (count > 1)
                    return true;
            }
            return false;
        }

    }
}