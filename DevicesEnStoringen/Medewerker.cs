using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen
{
    class Medewerker
    {
        DatabaseConnectie conn = new DatabaseConnectie();

        public bool ControleerInlogGegevens(string emailadres, string wachtwoord)
        {
            conn.OpenConnection();
            SQLiteCommand sqlCmd = conn.ReturnSQLiteCommand("SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailadres AND Wachtwoord=@Wachtwoord");
            sqlCmd.CommandType = System.Data.CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@Emailadres", emailadres);
            sqlCmd.Parameters.AddWithValue("@Wachtwoord", wachtwoord);
            bool inloggegevensCorrect = Convert.ToBoolean(sqlCmd.ExecuteScalar());

            return inloggegevensCorrect;
        }
    }
}
