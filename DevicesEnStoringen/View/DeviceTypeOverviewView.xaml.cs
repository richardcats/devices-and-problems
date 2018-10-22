using DevicesEnStoringen.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Model;
using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.View;
using System.ComponentModel;

namespace DevicesEnStoringen
{
    public partial class DeviceTypeOverviewView : UserControl
    {
        public DeviceTypeOverviewView()
        {
            InitializeComponent();
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            /*var _itemSourceList = new CollectionViewSource() { Source = DeviceTypes };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // create and apply the filter
            var searchFilter = new Predicate<object>(item => ((DeviceType)item).DeviceTypeName.ToLower().Contains(txtZoek.Text.ToLower()));
            Itemlist.Filter = searchFilter;

            dgDeviceTypes.ItemsSource = Itemlist;*/
        }
    }
}
