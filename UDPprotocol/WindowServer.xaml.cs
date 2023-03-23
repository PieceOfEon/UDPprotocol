using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    /// Interaction logic for WindowServer.xaml
    /// </summary>
    public partial class WindowServer : Window
    {
        private readonly UdpClient udpServer;
        private readonly IPEndPoint ipEndPoint;
        public WindowServer()
        {
            InitializeComponent();
            udpServer = new UdpClient(8080);
            ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            Task.Factory.StartNew(async() =>
            {

                Dispatcher.Invoke(() =>
                {
                    messagesTextBox2.Text += ("Server started on port 8080") + "\n";
                });
               

                while (true)
                {
                    // Получаем данные от клиента
                    var data = await udpServer.ReceiveAsync();

                    // Преобразуем полученные данные в строку
                    string request = Encoding.UTF8.GetString(data.Buffer);
                    Dispatcher.Invoke(() =>
                    {
                        messagesTextBox2.Text += ("Received request: {0} from {1}\n", request, ipEndPoint.ToString());
                    });
                    

                    // Получаем ответ на запрос клиента
                    string response = GetPrice(request);
                    //MessageBox.Show(response);
                    // Отправляем ответ клиенту
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await udpServer.SendAsync(responseData, responseData.Length, data.RemoteEndPoint);
                    //MessageBox.Show(response);
                    Dispatcher.Invoke(() =>
                    {
                        messagesTextBox2.Text += ("Sent response: {0}", response) + "\n";
                    });
                   
                }
            });

            
        }
        private string GetPrice(string partName)
        {
            // Здесь можно реализовать логику получения цены на запчасть
            switch (partName.ToLower())
            {
                case "cpu":
                    {
                        return "Price for CPU is $300";
                    }
                   
                case "gpu":
                    return "Price for GPU is $500";
                case "ram":
                    return "Price for RAM is $100";
                default:
                    return "Part not found";
            }
        }
    }
}
