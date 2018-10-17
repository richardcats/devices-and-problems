﻿using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Messages;
using DevicesEnStoringen.Services;
using DevicesEnStoringen.Utility;
using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace DevicesEnStoringen.ViewModel
{
    public class DeviceTypeDetailViewModel : INotifyPropertyChanged
    {
        private DeviceTypeDataService deviceTypeDataService;
        private DialogService dialogService = new DialogService();

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private SolidColorBrush blackText;
        public SolidColorBrush BlackText
        {
            get
            {
                return blackText;
            }
            set
            {
                blackText = value;
                RaisePropertyChanged("BlackText");
            }
        }


        private DeviceType selectedDeviceType;
        public DeviceType SelectedDeviceType
        {
            get
            {
                return selectedDeviceType;
            }
            set
            {
                selectedDeviceType = value;
                RaisePropertyChanged("SelectedDeviceType");
            }
        }

        public bool markRedIfFieldEmptyName;
        public bool MarkRedIfFieldEmptyName
        {
            get
            {
                return markRedIfFieldEmptyName;
            }
            set
            {
                markRedIfFieldEmptyName = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyName");
            }
        }

        public DeviceType SelectedDeviceTypeCopy { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }
        
        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveWithoutCloseCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceTypeDetailViewModel()
        {
            LoadCommands();
            deviceTypeDataService = new DeviceTypeDataService();
            Messenger.Default.Register<string>(this, OnNewDeviceType);
            Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);

        }

        // When showing the add new device-type window, set the title, make all TextBlocks black and make sure all fields are empty
        private void OnNewDeviceType(string obj)
        {
            Title = "Device-type registreren";

            MarkTextBlocksBlack();

            SelectedDeviceTypeCopy = new DeviceType()
            {
                DeviceTypeName = "",
                Description = ""
            };
        }

        // When showing the edit device-type window, set the title, make all TextBlocks black and set the selected devicetype
        private void OnDeviceTypeReceived(DeviceType deviceType)
        {
            Title = "Device-type bewerken";

            MarkTextBlocksBlack();

            SelectedDeviceType = deviceType;
            SelectedDeviceTypeCopy = SelectedDeviceType.Copy(); // Creates a deep copy in case the user wants to cancel the change
            DevicesOfCurrentDeviceType = deviceTypeDataService.GetDevicesOfDeviceType(SelectedDeviceType.DeviceTypeId).ToObservableCollection();
            
        }

        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddDeviceType, CanAddDeviceType);
            SaveCommand = new CustomCommand(SaveDeviceType, CanSaveDeviceType);
            SaveWithoutCloseCommand = new CustomCommand(SaveDeviceTypeWithoutClose, CanSaveDeviceTypeWithoutClose);
            DeleteCommand = new CustomCommand(DeleteDeviceType, CanDeleteDeviceType);
            CancelCommand = new CustomCommand(Cancel, CanCancel);
        }

        private bool CanAddDeviceType(object obj)
        {
            return true;
        }

        private void AddDeviceType(object obj)
        {

            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the device-type in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedDeviceType = SelectedDeviceTypeCopy;
                deviceTypeDataService.AddDeviceType(SelectedDeviceTypeCopy);
                Messenger.Default.Send(new UpdateListMessage(true));
            }
        }

        private void SaveDeviceTypeWithoutClose(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the device-type in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedDeviceType = SelectedDeviceTypeCopy.Copy(); // Creates a deep copy so that CanSaveDeviceTypeWithoutClose knows when a change is taking place in one of the fields again
                deviceTypeDataService.UpdateDeviceType(SelectedDeviceTypeCopy, SelectedDeviceTypeCopy.DeviceTypeId);
                Messenger.Default.Send(new UpdateListMessage(false));
            }
        }

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
        private bool CanSaveDeviceTypeWithoutClose(object obj)
        {
            if (SelectedDeviceType != null && SelectedDeviceTypeCopy != null)
            {
                if (SelectedDeviceTypeCopy.DeviceTypeName != SelectedDeviceType.DeviceTypeName || SelectedDeviceTypeCopy.Description != SelectedDeviceType.Description)
                    return true;
            }
            return false;
        }

        private void SaveDeviceType(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the device-type in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedDeviceType = SelectedDeviceTypeCopy;
                deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, SelectedDeviceType.DeviceTypeId);
                Messenger.Default.Send(new UpdateListMessage(true));
            }
        }

        private bool CanSaveDeviceType(object obj)
        {
            return true;
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Messenger.Default.Send(new UpdateListMessage(true));
        }

        private bool CanDeleteDeviceType(object obj)
        {
            return true;
        }

        private void DeleteDeviceType(object obj)
        {
            // Prevent problems by having the user first remove the coupled devices
            if (selectedDeviceType.DeviceAmount > 0)
            {
                dialogService.CanNotRemoveMessageBox("device-type", "devices");
                return;
            }

            // The user first receives a message before the device-type is permanently removed from the database
            if (dialogService.ShowRemoveWarningMessageBox("Device-type", selectedDeviceType.DeviceTypeId))
            {
                deviceTypeDataService.DeleteDeviceType(SelectedDeviceType);
                Messenger.Default.Send(new UpdateListMessage(true));
            }
        }


        public bool CheckIfFieldsNotEmpty()
        {
            MarkTextBlocksBlack();
            bool noEmptyFields = true;
            if (SelectedDeviceTypeCopy.DeviceTypeName.Length == 0)
            {
                MarkRedIfFieldEmptyName = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }

            if (noEmptyFields)
                return true;

            return false;
        }

        public void MarkTextBlocksBlack()
        {
            MarkRedIfFieldEmptyName = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
