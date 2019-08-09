using DevicesAndProblems.App.View;
using System.Windows;


namespace DevicesAndProblems.App.Services
{
    public class DialogService : IDialogService
    {
        Window window = null;

        public void ShowEditDialog(DialogType dialogType)
        {
            if (dialogType == DialogType.Problem)
                window = new DeviceTypeDetailView(true);
            else if (dialogType == DialogType.Device)
                window = new DeviceDetailView(true);
            else if (dialogType == DialogType.DeviceType)
                window = new DeviceTypeDetailView(true);

            window.ShowDialog();
        }

        public void ShowAddDialog(DialogType dialogType)
        {
            if (dialogType == DialogType.Problem)
                window = new DeviceTypeDetailView(false);
            else if (dialogType == DialogType.Device)
                window = new DeviceDetailView(false);
            else if (dialogType == DialogType.DeviceType)
                window = new DeviceTypeDetailView(false);

            window.ShowDialog();
        }

        public void CloseDialog()
        {
            if (window != null)
                window.Close();
        }

        public bool ShowRemoveWarningMessageBox(string type, int id)
        {
            return MessageBox.Show($"{type} {id} wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public void CanNotRemoveMessageBox(string deletableType, string preventingTypes)
        {
            MessageBox.Show($"Het is niet mogelijk om dit {deletableType} te verwijderen. Zorg dat er geen {preventingTypes} gekoppeld zijn aan dit {deletableType}.", deletableType, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowEmptyFieldMessageBox()
        {
            MessageBox.Show("Niet alle verplichte velden zijn ingevuld", "Ontbrekende informatie", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public enum DialogType { Problem, Device, DeviceType };
}
