using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peerreviewproject
{
    public partial class ForgotPass : System.Web.UI.Page 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count > 0)
            {
                Label2.Visible = true;
                Session.Abandon();
            }
        }

        protected void UsersubmitBttn_Click(object sender, EventArgs e)
        {
            string email = UsernameTB.Text.Trim();
            
            PasswordManagement_Class PassReset = new PasswordManagement_Class();
            if (PassReset.DoesUserExist(email))
            {
                PassReset.ResetPass(email);
            }
            Label2.Text = "Email sent to user with instructions";
            Label2.Visible = true;
            Session.Abandon();
            return;
        }

    }
}
