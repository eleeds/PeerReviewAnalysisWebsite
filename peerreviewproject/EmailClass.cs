using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;


namespace peerreviewproject
{
    public class EmailClass
    {
        private string Pass;
        private string Email;
        private string Subject;
        private string Professor;
        private string Course;

        public string letter;
        public EmailClass(string type, string userEmail, string pass, string professor, string course)   //constructor for resetting pass
        {
            Email = userEmail;
            if (type == "newAccount")
            {
                Professor = professor;
                Course = course;
                Message(type);
            }
            else if (type == "forgotPass")
            {
                Pass = HttpContext.Current.Server.UrlEncode(pass);
                Message(type);
            }
            else if (type == "newCourse")
            {
                Professor = professor;
                Course = course;
                Message(type);
            }

            EmailUser();
        }

        private void Message(string type)
        {

            if (type == "forgotPass")  //reset pass
            {
                Subject = "DEA Peer Review Forgot Pass";
                letter = "<br/> Follow the provided link to reset your pass. <br/>" 
                + "<a href='https://localhost:44313/ChangePass.aspx/?user=" + Email + "&token=" + Pass+ "'>Reset Password</a>";
            }
            else if (type == "newAccount") //new account
            {
                Subject = "New Account for DEA Peer Review";
                letter = "<br/> Professor " + Professor + " has added you to course " + Course + ". Please follow the Link" +
                    "to sign in and view your surveys. <br/>" 
                + "<a href='https://localhost:44313/ChangePass.aspx/?user=" + Email + "&token=" + Pass + "'>DEA Peer Review</a>";

            }
            else if(type == "newCourse") //student added to new class
            {
                Subject = "DEA Peer Review New Class";
                letter = "<br/> Professor " + Professor + " has added you to course " + Course + ". Please sign in to view new surveys" +
                         "to sign in and view your surveys. <br/>"
                         + "<a href='https://localhost:44313/LoginPage.aspx" + Email + "&token=" + Pass + "'>DEA Peer Review</a>";

            }
        }

        private void EmailUser()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("usipeerreview@gmail.com");
                message.To.Add(new MailAddress("shai114@hotmail.com"));
                message.Subject = Subject;
                message.Body = letter;
                message.IsBodyHtml = true;   
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";   
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("usipeerreview@gmail.com", "nyzysanrzdnvrpyn");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                //smtp.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }





    }
}