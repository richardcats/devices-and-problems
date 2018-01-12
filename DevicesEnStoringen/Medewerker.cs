using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen
{
    public class Medewerker
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        string emailadres;

        public Medewerker(string emailadres)
        {
            this.emailadres = emailadres;
        }

        public bool ControleerInlogGegevens(string emailadres, string wachtwoord)
        {
            conn.OpenConnection();
            SQLiteCommand sqlCmd = conn.ReturnSQLiteCommand("SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailadres AND Wachtwoord=@Wachtwoord");
            sqlCmd.CommandType = System.Data.CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@Emailadres", emailadres);
            sqlCmd.Parameters.AddWithValue("@Wachtwoord", wachtwoord);
            bool inloggegevensCorrect = Convert.ToBoolean(sqlCmd.ExecuteScalar());
            conn.CloseConnection();
            return inloggegevensCorrect;
        }

        public string naamHuidigeMedewerkerIngelogd()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM Medewerker WHERE Emailadres='" + emailadres + "'");
            dr.Read();
            string voornaam = dr["Voornaam"].ToString();
            conn.CloseConnection();
            return voornaam;
        }

        public int idHuidigeMedewerkerIngelogd()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM Medewerker WHERE Emailadres='" + emailadres + "'");
            dr.Read();
            string id = dr["MedewerkerID"].ToString();
            conn.CloseConnection();
            return Convert.ToInt32(id);
        }

        public string accountTypeHuidigeMedewerkerIngelogd()
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM Medewerker INNER JOIN AccountType ON Medewerker.AccountTypeID = AccountType.AccountTypeID WHERE Emailadres='" + emailadres + "'");
            dr.Read();
            string naam = dr["Naam"].ToString();
            conn.CloseConnection();
            return naam;
        }
    }
}
