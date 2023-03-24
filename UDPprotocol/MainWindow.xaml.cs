using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace UDPprotocol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int kol = 0;

        
        
        public MainWindow()
        {
            InitializeComponent();
            
        }
       
        private void ButtonServer_Click(object sender, RoutedEventArgs e)
        {
            
            WindowServer windowServer = new WindowServer();
            windowServer.Show();
            ButtonServer.IsEnabled = false;
            windowServer.Closed += WindowServer_Closed;
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            if(kol<3)
            {
                WindowClient windowClient = new WindowClient();
                windowClient.Show();
                windowClient.Closed += WindowClient_Closed;
                kol++;
                //MessageBox.Show(kol.ToString());
                // Создаем таймер с интервалом в 1 секунду
                
            }
          
           
        }

        private void WindowClient_Closed(object sender, EventArgs e)
        {
            kol--;
            //MessageBox.Show(kol.ToString());
        }

        private void WindowServer_Closed(object sender, EventArgs e)
        {
            ButtonServer.IsEnabled = true;
        }
    }
}
