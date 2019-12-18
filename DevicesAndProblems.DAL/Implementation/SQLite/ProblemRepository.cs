using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.SQLite
{
    public class ProblemRepository : Implementation.SQLiteDataAccess, IProblemRepository
    {
        public List<Problem> GetAll()
        {
            //TO DO: waarom DatumAfhandeling/ClosureDate, MedewerkerGeregistreerd/RaisedByEmployeeId, MedewerkerBehandeld/HandledByEmployeeId meegeven? wordt niet in de lijst getoont
            string sql = "SELECT Storing.Id AS Id, RaisedByEmployeeId, " +
                "Description, Date(DateRaised) AS DateRaised, Date(ClosureDate) AS ClosureDate, " +
                "Priority, Severity, Status, HandledByEmployeeId, Medewerker.* " +
                "FROM Storing " +
                "LEFT JOIN Medewerker ON Storing.RaisedByEmployeeId = Medewerker.MedewerkerID";

            //TO DO: dit nog nodig?
            /*   if (!DBNull.Value.Equals(reader["HandledByEmployeeId"]))
                   problem.HandledBy = Convert.ToInt32(reader["HandledByEmployeeId"]) - 1;

               if (!DBNull.Value.Equals(reader["DatumAfhandeling"]))
                   problem.ClosureDate = Convert.ToDateTime(reader["DatumAfhandeling"]).Date;*/

            return GetAll<Problem>(sql, null).ToList();
        }

        public List<Problem> GetById(int problemId)
        {
            throw new NotImplementedException();
        }

        public List<Problem> GetProblemsByDeviceId(int deviceId)
        {
            throw new NotImplementedException();
        }

        public void Add(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            string sql = "INSERT INTO Storing(Description, RaisedByEmployeeId, HandledByEmployeeId, Priority, Severity, Status, DateRaised) " +
                "VALUES(@Description, @RaisedByEmployeeId, @HandledByEmployeeId, @Priority, @Severity, @Status, date('now'))";

            Add<Problem>(sql, newProblem);

            //TO DO: maak hier een aparte call van?
            /*using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                string query2 = "SELECT last_insert_rowid() AS LastID;";
                SQLiteCommand command2 = new SQLiteCommand(query2, connection);

                SQLiteDataReader dr = command2.ExecuteReader();
                dr.Read();

                foreach (Device device in DevicesOfCurrentProblem)
                {
                    command.CommandText = "INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + Convert.ToInt32(dr["LastID"]) + "','" + device.Id + "')";
                    command.ExecuteNonQuery();
                }
            }*/
        }

        public void Update(Problem problem, ObservableCollection<Device> devicesOfProblem, int problemId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Problem problem) // TO DO: pass int as parameter instead?
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<string> GetComboboxItemsByComboboxType(ComboboxType type)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<int> GetComboboxMonthsByYear(int year)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<int> GetComboboxYears()
        {
            throw new NotImplementedException();
        }


    }

    public enum ComboboxType
    {
        Department, DeviceType, DeviceTypeAll, Status, StatusAll, Employee, PrioritySeverity, Month, Year
    };
}
