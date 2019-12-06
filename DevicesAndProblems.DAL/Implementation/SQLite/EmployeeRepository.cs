using DevicesAndProblems.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.SQLite
{
    public class EmployeeRepository : IEmployeeRepository
    {
        string connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public bool CheckLoginDetails(string emailaddress, string password)
        {
            bool loginDetailsCorrect;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailaddress AND Wachtwoord=@Password";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Emailaddress", emailaddress);
                command.Parameters.AddWithValue("@Password", password);
                loginDetailsCorrect = Convert.ToBoolean(command.ExecuteScalar());
            }
            return loginDetailsCorrect;
        }
        // TODO: Generieker maken
        public string FirstNameOfCurrentEmployee(string emailAddress)
        {
            string firstName;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT * FROM Medewerker WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                firstName = dr["Voornaam"].ToString();
            }
            return firstName;
        }

        public int IDOfCurrentEmployee(string emailAddress)
        {
            int id;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT * FROM Medewerker WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                id = Convert.ToInt32(dr["MedewerkerID"]);
            }
            return id;
        }

        public string AccountTypeOfCurrentEmployee(string emailAddress)
        {
            string accountTypeName;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();

                string query = "SELECT Naam FROM Medewerker INNER JOIN AccountType ON Medewerker.AccountTypeID = AccountType.AccountTypeID WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                accountTypeName = dr["Naam"].ToString();
            }
            return accountTypeName;
        }
    }
}
