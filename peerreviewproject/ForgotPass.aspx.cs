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

        }

        protected void UsersubmitBttn_Click(object sender, EventArgs e)
        {
            string email = UsernameTB.Text.Trim();
            
            PasswordManagement_Class PassReset = new PasswordManagement_Class();
            if (PassReset.DoesUserExist(email))
            {
                PassReset.ResetPass(email);
            }
            return;
        }

        protected void emailUserNewPass(object sender, EventArgs e)
        {
          /*  // static void Email(string htmlString) for email class
            // {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("shai114@hotmail.com");
                message.To.Add(new MailAddress("shai114@hotmail.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "did i work?";
                smtp.Port = 587;
                smtp.Host = "smtp.live.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
<<<<<<< HEAD
                smtp.Credentials = new NetworkCredential("shai114@hotmail.com", "password");
=======
                smtp.Credentials = new NetworkCredential("*****@hotmail.com", "*******");
>>>>>>> 1c2c7d4eff7f1fe0579e73c7fdf620f814a12624
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
            // }
          */
        }
    }
}
