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
                string reset_query = "UPDATE User_table SET password = ENCRYPTBYPASSPHRASE(), tempPass = 1 WHERE email=@email";
                string pass = Membership.GeneratePassword(20, 2);               
                SqlCommand resetPass_sqlCmd = new SqlCommand(reset_query, sqlCon);
                resetPass_sqlCmd.Parameters.AddWithValue("@password", pass);
                resetPass_sqlCmd.Parameters.AddWithValue("@email", email);
                resetPass_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

                EmailClass emailResetPass = new EmailClass("forgotPass", email, pass, "", ""); //course and professor aren't needed

            }
        }
        public void ChangePass(string email, string newPass)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();              //users password is encrypted based off of USI2021 passphrase
                string reset_query = "UPDATE User_table SET password = ENCRYPTBYPASSPHRASE(), tempPass = 0 WHERE email=@email";
                SqlCommand resetPass_sqlCmd = new SqlCommand(reset_query, sqlCon);
                resetPass_sqlCmd.Parameters.AddWithValue("@password", newPass);
                resetPass_sqlCmd.Parameters.AddWithValue("@email", email);
                resetPass_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }
        }

        public int doesUserExist(string email, string pass)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();
                string doesUserExist = "SELECT COUNT(1) FROM User_table WHERE email=@email AND DECRYPTBYPASSPHRASE()=@password";
                SqlCommand userExistCommand = new SqlCommand(doesUserExist, sqlCon);
                userExistCommand.Parameters.AddWithValue("@email", email);
                userExistCommand.Parameters.AddWithValue("@password", pass);
                int count = Convert.ToInt32(userExistCommand.ExecuteScalar());
                sqlCon.Close();
                return count;
            }
        }

        public string[] GetUser(string email, string pass)
        {
            string[] userInfo = new string[2];

            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();
                string getUser = "SELECT ID, type FROM User_table WHERE email=@email AND CONVERT(NVARCHAR(150), DECRYPTBYPASSPHRASE())=@password";
                SqlCommand userQuery = new SqlCommand(getUser, sqlCon);
                userQuery.Parameters.AddWithValue("@email", email);
                userQuery.Parameters.AddWithValue("@password", pass);
                SqlDataReader sqlReader = userQuery.ExecuteReader();

                while (sqlReader.Read())
                {
                    userInfo[0] = sqlReader["ID"].ToString();
                    userInfo[1] = sqlReader["type"].ToString();
                }
                sqlCon.Close();
                return userInfo;
            }

        }
    }

}
