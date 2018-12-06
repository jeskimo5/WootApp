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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1(Image detailImage)
        {
            InitializeComponent();
            image1.Source = detailImage.Source;
        }

        private void image2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        public void CloseWindow()
        {
            this.Close();
        }

      
    }
}
