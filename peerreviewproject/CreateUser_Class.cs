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
                    "VALUES (@fname, @lname, @email, ENCRYPTBYPASSPHRASE(N'USI2021', @password), @type)";

                SqlCommand createUser_sqlCmd = new SqlCommand(createUser_query, sqlCon);

                createUser_sqlCmd.Parameters.AddWithValue("@fname", firstName);
                createUser_sqlCmd.Parameters.AddWithValue("@lname", lastName);
                createUser_sqlCmd.Parameters.AddWithValue("@password", Membership.GeneratePassword(10,2));
                createUser_sqlCmd.Parameters.AddWithValue("@email", userEmail);
                createUser_sqlCmd.Parameters.AddWithValue("@type", permissionType);
                createUser_sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

            }

        } //can combine these two constructors as one once admin page created

        public CreateUser_Class(string fName, string lName, string userEmail) //professor uses this constructor Student account
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
        public CreateUser_Class(string fName, string lName, string userEmail, string type) //Admin uses this constructor to create Professor account
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

        public CreateUser_Class()
        {
            
        }
    }
}