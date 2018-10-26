using System;
using System.Windows.Controls;

namespace DevicesEnStoringen
{
    public partial class DeviceTypeOverviewView : UserControl
    {
        public DeviceTypeOverviewView()
        {
            InitializeComponent();
        }

        // As soon as a change has occurred in the search field, force the DataGrid to update
        private void SearchInputChanged(object sender, EventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
