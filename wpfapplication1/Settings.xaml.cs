using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WootAlert
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public bool audioAlerts;
        
        public SettingsWindow(bool audioAlert,bool windowsStart, double wootRefresh, double wootOffRefresh)
        {
            
            InitializeComponent();
            audioAlerts = audioAlert;
            checkBox1.IsChecked = audioAlert;
            checkBox2.IsChecked = windowsStart;
            slider1.Value = wootRefresh;
            slider2.Value = wootOffRefresh;
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            //Close();
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (audioAlerts)
            { audioAlerts = false; }
            else
            { audioAlerts = true; }
        }

        



        
       
    }
}
