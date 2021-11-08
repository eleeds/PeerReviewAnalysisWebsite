using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace peerreviewproject
{

    public partial class AddUserstoClass : System.Web.UI.Page
    {
        public string fileName;
        public string filePath;
        public DataTable CSV_Datatable = new DataTable();
        string user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CourseAvailable_dropdownlist.DataBind();
            }
                CurrentRoster_lbl.Text = "Current Student Roster for " + CourseAvailable_dropdownlist.SelectedItem.Text;
                StudentToClass_bttn.Text = "Add students to " + CourseAvailable_dropdownlist.SelectedItem.Text;
            
            user = Session["userID"].ToString();

            if (ViewState["data"] != null)
            {
                CSV_Datatable = (DataTable)ViewState["data"];
            }
            else
            {
                GridView1.DataSource = new string[] { };
                GridView1.DataBind();
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
                                Read_from_csv(fileName, filePath);
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            FileUpload1.SaveAs(filePath);
                            Read_from_csv(fileName, filePath);
                        }
                    }
                    else
                    {
                        FileUpload1.SaveAs(filePath);
                        Read_from_csv(fileName, filePath);
                    }
                    Label1.Text = fileName + " was imported successfully!";
                    File.Delete(filePath);                                  //remove import file after reading data
                }
                else Label1.Text = "Please select a CSV file";

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void StudentToClass_bttnClick(object sender, EventArgs e)
        {
            string courseID = CourseAvailable_dropdownlist.SelectedValue;

            if (CSV_Datatable == null || CSV_Datatable.Rows.Count == 0)
            {
                Label2.Text = "Add students first";
                return;
            }
            foreach (DataRow row in CSV_Datatable.Rows)
            {                    
                                                // [0]first name, [1]last name, [2]email, [3]team
                _ = new CreateUser_Class(row[0].ToString(), row[1].ToString(), row[2].ToString());
                _ = new StudentTo_Class(courseID, row[2].ToString(), row[3].ToString());
                //here
            }
            Label2.Text = "New users added";
            CSV_Datatable.Clear();
            GridView1.DataBind();
            ListBox1.DataBind();
        }


        public void Read_from_csv(string fileName, string filePath)
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
            GridView1.DataSource = CSV_Datatable;
            GridView1.DataBind();

        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = CSV_Datatable;
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e) //delete row
        {
            CSV_Datatable = (DataTable)ViewState["data"];
            CSV_Datatable.Rows[e.RowIndex].Delete();
            CSV_Datatable.AcceptChanges();
            ViewState["data"] = CSV_Datatable;
            GridView1.DataSource = CSV_Datatable;
            GridView1.DataBind();

        }


        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) //refresh gridview if edit cancel pressed
        {

            GridView1.EditIndex = -1;
            GridView1.DataSource = CSV_Datatable;
            GridView1.DataBind();

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)  //for updating cells to temp_table
        {
           // string FirstN = e.OldValues[1].ToString();
            CSV_Datatable.Rows[e.RowIndex]["First Name"] = e.NewValues[0].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Last Name"] = e.NewValues[1].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Email"] = e.NewValues[2].ToString().Trim();
            CSV_Datatable.Rows[e.RowIndex]["Team"] = e.NewValues[3].ToString().Trim();
            CSV_Datatable.AcceptChanges();
           
            
            ViewState["data"] = CSV_Datatable;
            GridView1.EditIndex = -1;
            GridView1.DataSource = CSV_Datatable;
            GridView1.DataBind();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)    //adds confirm message before remove
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton Remove = e.Row.FindControl("LinkButton1") as LinkButton;
                Remove.Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to remove {0} {1} ?')", e.Row.Cells[1].Text, e.Row.Cells[2].Text));
            }
        }

        protected void Dropdownchange(object sender, EventArgs e)
        {
            if (ListBox1.Items.Count == 0)
            {
                ListBox1.Items.Add("No students");
            }
        }


        protected void AddStudent_bttnclick(object sender, EventArgs e)
        { 
            TextBox[] boxes = { FirstnameTB, LastnameTB, EmailTB, TeamTB };

            if (!TextBox_Check(boxes))
            {
                CSV_Datatable.Rows.Add(FirstnameTB.Text, LastnameTB.Text, EmailTB.Text, TeamTB.Text);
                ViewState["data"] = CSV_Datatable;
                GridView1.DataSource = CSV_Datatable;
                GridView1.DataBind();

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


    }
}