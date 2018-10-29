using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
{
    public interface IDialogService
    {
        void ShowEditDialog();
        void ShowAddDialog();
        void CloseDialog();
        bool ShowRemoveWarningMessageBox(string type, int id);
        void CanNotRemoveMessageBox(string deletableType, string preventingTypes);
        void ShowEmptyFieldMessageBox();

    }
}
