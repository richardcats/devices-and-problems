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
        public ProblemRepository()
        {

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

        public void AddComment(Problem selectedProblem, Comment newComment)
        {
            throw new NotImplementedException();
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            throw new NotImplementedException();
        }

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
        

        public ObservableCollection<string> GetComboboxItemsByComboboxType(ComboboxType type)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<int> GetComboboxMonthsByYear(int selectedYear)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<int> GetComboboxYears()
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem)
        {
            throw new NotImplementedException();
        }

        public void RemoveComment(Comment selectedComment, Problem selectedProblem)
        {
            throw new NotImplementedException();
        }

        public List<Device> SelectDevicesByProblemId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Problem> SelectProblemsByDeviceId(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            throw new NotImplementedException();
        }
    }

    public enum ComboboxType
    {
        Afdeling, DeviceType, DeviceTypeAll, Status, StatusAll, Medewerker, PrioriteitErnst, Month, Year
    };
}
