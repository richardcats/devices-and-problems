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
using System.Windows.Shapes;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for Storing.xaml
    /// </summary>
    public partial class Storing : Window
    {
        bool editMode;
        public Storing(bool editMode)
        {
            InitializeComponent();
            this.editMode = editMode;

            if (editMode)
                Title = "Storing bewerken";
            else
                Title = "Storing registreren";
        }
    }
}
