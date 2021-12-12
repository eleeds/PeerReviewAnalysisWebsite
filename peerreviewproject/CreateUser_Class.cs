using System;
using System.Data.SqlClient;
using System.Web.Security;

namespace peerreviewproject
{
    public class CreateUser_Class
    {
        private string FirstName;
        private string LastName;
        private string Email;
        private string UserType;
        public string sqlconnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                            Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //EmailClass emailUser = new EmailClass();

        public bool DoesUserExist(string userEmail)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();
                string userExist_query = "SELECT COUNT(1) FROM User_table WHERE email=@email";
                SqlCommand userExist_sqlCmd = new SqlCommand(userExist_query, sqlCon);
                userExist_sqlCmd.Parameters.AddWithValue("@email", userEmail);
                int count = Convert.ToInt32(userExist_sqlCmd.ExecuteScalar());
                sqlCon.Close();

                if (count == 1)
                {
                    return true;
                }
                return false;
            }
        }
        public void UsertoDataBase(string firstName, string lastName, string userEmail, string permissionType)
        {
            using (SqlConnection sqlCon = new SqlConnection(sqlconnection))
            {
                sqlCon.Open();
                string createUser_query = "INSERT INTO User_table ([firstName], [lastName], [email], [password], [type]) " +
                    "VALUES (@fname, @lname, @email, ENCRYPTBYPASSPHRASE('USI2021', @password), @type)";

                SqlCommand createUser_sqlCmd = new SqlCommand(createUser_query, sqlCon);
                string pass = Membership.GeneratePassword(20, 10);
                createUser_sqlCmd.Parameters.AddWithValue("@fname", firstName);
                createUser_sqlCmd.Parameters.AddWithValue("@lname", lastName);
                createUser_sqlCmd.Parameters.AddWithValue("@password", pass);
                createUser_sqlCmd.Parameters.AddWithValue("@email", userEmail);
                createUser_sqlCmd.Parameters.AddWithValue("@type", permissionType);
                createUser_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }

        }

        public bool newAccount (string fName, string lName, string userEmail, string type)
        {
            if (!DoesUserExist(userEmail))
            {
                FirstName = fName;
                LastName = lName;
                Email = userEmail;
                if (type == "Admin")
                {
                    UserType = "Professor";
                }
                else
                {
                    UserType = "Student";
                }
                UsertoDataBase(FirstName, LastName, userEmail, UserType);
                return true;
            }
            return false;
        }

        public CreateUser_Class()
        {

        }
    }
}