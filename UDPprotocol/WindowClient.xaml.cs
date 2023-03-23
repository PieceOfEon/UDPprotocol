using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace UDPprotocol
{
    /// <summary>
    /// Interaction logic for WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        private UdpClient udpClient;
        private IPAddress serverAddress;
        private int serverPort;
        public WindowClient()
        {
            InitializeComponent();

            
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string partName = messagesSendTextBox.Text;

                // Отправляем запрос на сервер
                byte[] requestData = Encoding.UTF8.GetBytes(partName);
                await udpClient.SendAsync(requestData, requestData.Length, new IPEndPoint(serverAddress, serverPort));

                // Получаем ответ от сервера
                UdpReceiveResult response = await udpClient.ReceiveAsync();
                //MessageBox.Show(response.ToString());
                string responseString = Encoding.UTF8.GetString(response.Buffer);
                
                Dispatcher.Invoke(() => messagesTextBox.Text+= responseString +"\n" );
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    messagesTextBox.Text += ($"Error occurred: {ex.Message}" + "\n" );
                });
                
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            udpClient = new UdpClient();
            serverAddress = IPAddress.Parse("127.0.0.1");
            serverPort = 8080;
            Dispatcher.Invoke(() =>
            {
                messagesTextBox.Text += "Connected" + "\n";
            });
           
        }
    }
}
