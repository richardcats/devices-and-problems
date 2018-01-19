using System;
using System.Data.SQLite;

namespace DevicesEnStoringen
{
    public class Employee
    {
        DatabaseConnection conn = new DatabaseConnection();
        public string EmailAddress { get; }

        public Employee(string emailaddress)
        {
            EmailAddress = emailaddress;
        }

        public bool CheckLoginDetails(string emailaddress, string password)
        {
            conn.OpenConnection();
            SQLiteCommand sqlCmd = conn.ReturnSQLiteCommand("SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailaddress AND Wachtwoord=@Password");
            sqlCmd.CommandType = System.Data.CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@Emailaddress", emailaddress);
            sqlCmd.Parameters.AddWithValue("@Password", password);
            bool loginDetailsCorrect = Convert.ToBoolean(sqlCmd.ExecuteScalar());
            conn.CloseConnection();
            return loginDetailsCorrect;
        }

        public string FirstNameOfCurrentEmployee()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM Medewerker WHERE Emailadres='" + EmailAddress + "'");
            dr.Read();
            string firstName = dr["Voornaam"].ToString();
            conn.CloseConnection();
            return firstName;
        }

        public int IDOfCurrentEmployee()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM Medewerker WHERE Emailadres='" + EmailAddress + "'");
            dr.Read();
            string id = dr["MedewerkerID"].ToString();
            conn.CloseConnection();
            return Convert.ToInt32(id);
        }

        public string AccountTypeOfCurrentEmployee()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM Medewerker INNER JOIN AccountType ON Medewerker.AccountTypeID = AccountType.AccountTypeID WHERE Emailadres='" + EmailAddress + "'");
            dr.Read();
            string accountTypeName = dr["Naam"].ToString();
            conn.CloseConnection();
            return accountTypeName;
        }
    }
}
