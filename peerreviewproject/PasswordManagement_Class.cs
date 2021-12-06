using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace peerreviewproject
{
    public class PasswordManagement_Class : CreateUser_Class
    {
        //using sqlconnection from CreateUser Class

        public void ResetPass(string email)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                                            //creates random password and and encrypts it based off of USI2021 passphrase
                sqlCon.Open();  
                string reset_query = "UPDATE User_table SET password = ENCRYPTBYPASSPHRASE(N'USI2021', @password), tempPass = 1 WHERE email=@email";
                string pass = Membership.GeneratePassword(10, 2);               //will delete
                SqlCommand resetPass_sqlCmd = new SqlCommand(reset_query, sqlCon);
                resetPass_sqlCmd.Parameters.AddWithValue("@password", pass);
                resetPass_sqlCmd.Parameters.AddWithValue("@email", email);
                resetPass_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                
            }
        }
        public void ChangePass(string email, string newPass)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();              //users password is encrypted based off of USI2021 passphrase
                string reset_query = "UPDATE User_table SET password = ENCRYPTBYPASSPHRASE(N'USI2021', @password), tempPass = 0 WHERE email=@email";
                SqlCommand resetPass_sqlCmd = new SqlCommand(reset_query, sqlCon);
                resetPass_sqlCmd.Parameters.AddWithValue("@password", newPass);
                resetPass_sqlCmd.Parameters.AddWithValue("@email", email);
                resetPass_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }
        }
    }
    
}