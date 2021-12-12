using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace peerreviewproject
{

    public partial class AddUserstoClass : System.Web.UI.Page
    {
        public string fileName;
        public string filePath;
        public DataTable CSV_Datatable = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count == 0)
            {
                Response.Redirect("LoginPage.aspx");
            }

            if (!IsPostBack)
            {
                CourseAvailable_dropdownlist.DataBind();
            }
            CurrentRoster_lbl.Text = "Current Student Roster for " + CourseAvailable_dropdownlist.SelectedItem.Text;
            StudentToClass_bttn.Text = "Add students to " + CourseAvailable_dropdownlist.SelectedItem.Text;


            if (ViewState["data"] != null)
            {
                CSV_Datatable = (DataTable)ViewState["data"];
            }
            else
            {
                StudentsToAddGridview.DataSource = new string[] { };
                StudentsToAddGridview.DataBind();
                CSV_Datatable.Columns.AddRange(new DataColumn[4]
                {new DataColumn("First Name", typeof(string))
                ,new DataColumn("Last Name", typeof(string))
                ,new DataColumn("Email", typeof(string))
                ,new DataColumn("Team", typeof(string))});

            }

        }

        protected void Import_bttn_click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile && Path.GetExtension(FileUpload1.PostedFile.FileName) == ".csv")
                {
                    int check = 0;
                    fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string randomName = DateTime.Now.ToFileTime().ToString();
                    string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string folder = "~/CSV_uploads/";

                    filePath = Server.MapPath(folder + fileName);
                    string[] filenames = Directory.GetFiles(Server.MapPath("~/CSV_uploads"));

                    if (filenames.Length > 0)
                    {
                        foreach (string filename in filenames)
                        {
                            if (filePath == filename)
                            {
                                check = 1;
                                Read_from_csv(filePath);
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            FileUpload1.SaveAs(filePath);
                            Read_from_csv(filePath);
                        }
                    }
                    else
                    {
                        FileUpload1.SaveAs(filePath);
                        Read_from_csv(filePath);
                    }
                    SuccessLabel.Text = fileName + " was imported successfully!";
                    File.Delete(filePath);                                  //remove import file after reading data
                    
                }
                else WarningLabel.Text = "Please select a CSV file";

            }

            catch 
            {
                Response.Write("<script>alert('Incorrect format. Try again.')</script>");
            }
        }

        protected void StudentToClass_bttnClick(object sender, EventArgs e)
        {
            string courseID = CourseAvailable_dropdownlist.SelectedValue;

            if (CSV_Datatable == null || CSV_Datatable.Rows.Count == 0)
            {
                SuccessLabel.Text = "Add students first";
                return;
            }
            foreach (DataRow row in CSV_Datatable.Rows)
            {
                // [0]first name, [1]last name, [2]email, [3]team
                CreateUser_Class newStudent = new CreateUser_Class();
                newStudent.newAccount(row[0].ToString(), row[1].ToString(), row[2].ToString(), "Professor");
                _ = new StudentTo_Class(courseID, row[2].ToString(), row[3].ToString(), Convert.ToInt32(Session["userID"]));

            }
            SuccessLabel.Text = "New users added";
            CSV_Datatable.Clear();
            StudentsToAddGridview.DataBind();
            CurrentRosterGridView.DataBind();
        }


        public void Read_from_csv(string filePath)
        {
            string csvreader = File.ReadAllText(filePath);

            foreach (string row in csvreader.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row) && !row.Contains("First"))
                {
                    CSV_Datatable.Rows.Add();
                    int i = 0;

                    foreach (string cell in row.Split(','))
                    {
                        CSV_Datatable.Rows[CSV_Datatable.Rows.Count - 1][i] = cell.Trim();
                        i++;
                    }

                }
            }
            ViewState["data"] = CSV_Datatable;
            StudentsToAddGridview.DataSource = CSV_Datatable;
            StudentsToAddGridview.DataBind();

        }


        protected void StudentsToAddGridview_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StudentsToAddGridview.EditIndex = e.NewEditIndex;
            StudentsToAddGridview.DataSource = CSV_Datatable;
            StudentsToAddGridview.DataBind();
        }

        protected void StudentsToAddGridview_RowDeleting(object sender, GridViewDeleteEventArgs e) //delete row
        {
            CSV_Datatable = (DataTable)ViewState["data"];
            CSV_Datatable.Rows[e.RowIndex].Delete();
            CSV_Datatable.AcceptChanges();
            ViewState["data"] = CSV_Datatable;
            StudentsToAddGridview.DataSource = CSV_Datatable;
            StudentsToAddGridview.DataBind();

        }


        protected void StudentToAddGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) //refresh gridview if edit cancel pressed
        {

            StudentsToAddGridview.EditIndex = -1;
            StudentsToAddGridview.DataSource = CSV_Datatable;
            StudentsToAddGridview.DataBind();

        }

        protected void StudentsToAddGridview_RowUpdating(object sender, GridViewUpdateEventArgs e)  //for updating cells to temp_table
        {

            CSV_Datatable.Rows[e.RowIndex]["First Name"] = e.NewValues[0].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Last Name"] = e.NewValues[1].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Email"] = e.NewValues[2].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Team"] = e.NewValues[3].ToString().Trim();
            CSV_Datatable.AcceptChanges();


            ViewState["data"] = CSV_Datatable;
            StudentsToAddGridview.EditIndex = -1;
            StudentsToAddGridview.DataSource = CSV_Datatable;
            StudentsToAddGridview.DataBind();


        }

        protected void StudentsToAddGridview_RowDataBound(object sender, GridViewRowEventArgs e)    //adds confirm message before remove
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton Remove = e.Row.FindControl("LinkButton1") as LinkButton;
                Remove.Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to remove {0} {1} ?')", e.Row.Cells[1].Text, e.Row.Cells[2].Text));
            }
        }

        protected void Dropdownchange(object sender, EventArgs e)
        {
            SuccessLabel.Visible = false;
        }


        protected void AddStudent_bttnclick(object sender, EventArgs e)
        {
            TextBox[] boxes = { FirstnameTB, LastnameTB, EmailTB, TeamTB };

            if (!TextBox_Check(boxes))
            {
                CSV_Datatable.Rows.Add(FirstnameTB.Text, LastnameTB.Text, EmailTB.Text, TeamTB.Text);
                ViewState["data"] = CSV_Datatable;
                StudentsToAddGridview.DataSource = CSV_Datatable;
                StudentsToAddGridview.DataBind();

            }
        }

        public bool TextBox_Check(TextBox[] boxes)
        {
            int count = 0;

            foreach (TextBox tb in boxes)
            {
                if (tb.Text == string.Empty)
                {
                    tb.BorderColor = System.Drawing.Color.Red;
                    count++;
                }
                else tb.BorderColor = System.Drawing.Color.Empty;
            }
            if (count == 0)
                return false;
            else return true;
        }

        protected void CurrentRosterGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _ = new StudentTo_Class(e.Keys[0].ToString(), CourseAvailable_dropdownlist.SelectedValue);

        }

        protected void CurrentRosterGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton Remove = e.Row.FindControl("LinkButton2") as LinkButton;
                Remove.Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to remove {0} ?')", e.Row.Cells[0].Text));
            }
            CurrentRosterGridView.Caption = CurrentRoster_lbl.Text;
        }

        protected void CurrentRosterGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionStringClass.connection))
            {
                string[] firstLastNames = e.NewValues[0].ToString().Split(' ');
                sqlCon.Open();
                string query = "UPDATE User_table SET firstName =@firstName, lastName =@lastName WHERE CONCAT(firstName, CONCAT(' ', lastName)) =@oldValues";
                SqlCommand NameEdit = new SqlCommand(query, sqlCon);
                NameEdit.Parameters.AddWithValue("@firstName", firstLastNames[0]);
                NameEdit.Parameters.AddWithValue("@lastName", firstLastNames[1]);
                NameEdit.Parameters.AddWithValue("@oldValues", e.OldValues[0]);
                NameEdit.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        protected void ExampleCSVFile_Click(object sender, EventArgs e)
        {
            Response.ContentType = "image/jpeg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CSV_Format_example.csv");
            Response.TransmitFile(Server.MapPath("~/CSV_uploads/CSV_Format_example.csv"));
            Response.End();
        }

        protected void HomeBttn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherMain.aspx");
        }
    }
}