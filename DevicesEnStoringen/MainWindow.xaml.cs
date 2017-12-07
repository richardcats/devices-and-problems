using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevicesEnStoringen.DataAccess;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DataCommander dc = new DataCommander();
        List<string> containers = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            dc.ContainersReady += showContainers;
        }

        private void showContainers()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string item in containers)
            {
                sb.AppendLine(item);
            }
            txtContainer.Text = sb.ToString();
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            containers = dc.GetAllContainers();
        }
    }
}
