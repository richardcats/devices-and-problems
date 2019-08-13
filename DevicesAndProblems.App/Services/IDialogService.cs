namespace DevicesAndProblems.App.Services
{
    public interface IDialogService
    {
        void ShowEditDialog(ViewType viewType);
        void ShowAddDialog(ViewType viewType);
        void CloseDialog();
        bool ShowRemoveWarningMessageBox(string type, int id);
        void CanNotRemoveMessageBox(string deletableType, string preventingTypes);
        void ShowEmptyFieldMessageBox();

    }
}
