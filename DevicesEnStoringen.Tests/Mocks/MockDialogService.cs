using DevicesEnStoringen.View;
using System.Windows;

namespace DevicesEnStoringen.Services
{
    public class MockDialogService : IDialogService
    {

        public void ShowEditDialog()
        {
        }

        public void ShowAddDialog()
        {
        }

        public void CloseDialog()
        {
        }

        public bool ShowRemoveWarningMessageBox(string type, int id)
        {
            return false;
        }

        public void CanNotRemoveMessageBox(string deletableType, string preventingTypes)
        {
        }

        public void ShowEmptyFieldMessageBox()
        {
        }
    }
}
