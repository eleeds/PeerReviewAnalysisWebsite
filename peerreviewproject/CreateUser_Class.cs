using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace peerreviewproject
{
    public class CreateUser_Class
    {
        private string FirstName;
        private string LastName;
        private string Email;
        private string UserType;

        public string FirstN
        {
            get { return FirstName; }
            set { FirstName = value; }
        }
        public string LastN
        {
            get { return LastName; }
            set { LastName = value; }
        }
        public string Email_
        {
            get { return Email; }
            set { Email = value; }
        }
        public string PermissionType
        {
            get { return UserType; }
            set { UserType = value; }
        }

        public bool DoesUserExist(string userEmail)
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query = "SELECT COUNT(1) FROM User_table WHERE email=@email";

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@email", userEmail); 
                

                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
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
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\SHAI1\PEER_REVIEW.MDF;
                        Integrated Security=True;
                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                sqlCon.Open();
                string query = "INSERT INTO User_table ([firstName], [lastName], [email], [password], [type]) " +
                    "VALUES (@fname, @lname, @email, ENCRYPTBYPASSPHRASE(N'USI2021', @password), @type)";

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@fname", firstName);
                sqlCmd.Parameters.AddWithValue("@lname", lastName);
                sqlCmd.Parameters.AddWithValue("@password", Membership.GeneratePassword(10,2));
                sqlCmd.Parameters.AddWithValue("@email", userEmail);
                sqlCmd.Parameters.AddWithValue("@type", permissionType);
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }

        } //can combine these two constructors as one once admin page created

        public CreateUser_Class(string fName, string lName, string userEmail) //professor uses this constructor
        {
            if (!DoesUserExist(userEmail)) //check for user in database
            {
                FirstName = fName;
                LastName = lName;
                Email_ = userEmail;
                PermissionType = "Student";
                UsertoDataBase(FirstName, LastName, userEmail, PermissionType);

            }
          
        }
        public CreateUser_Class(string fName, string lName, string userEmail, string type) //Admin uses this constructor
        {
            if (!DoesUserExist(userEmail))
            {
                FirstName = fName;
                LastName = lName;
                Email_ = userEmail;
                PermissionType = "Professor";
                UsertoDataBase(FirstName, LastName, userEmail, PermissionType);

            }
        }
    }
}