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
using System.Threading;
using System.Timers;
using System.Windows.Threading;

namespace UDPprotocol
{
    /// <summary>
    /// Interaction logic for WindowClient.xaml
    /// </summary>
    public partial class WindowClient : Window
    {
        private DispatcherTimer _timer;
        private double _inactiveTime;
        private UdpClient udpClient;
        private IPAddress serverAddress;
        private int serverPort;
        public WindowClient()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);

            // Устанавливаем обработчик события для таймера
            _timer.Tick += Timer_Tick;

            // Запускаем таймер
            _timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Проверяем, активно ли окно
            if (!IsActive)
            {
                // Если окно неактивно, то увеличиваем счетчик времени неактивности
                _inactiveTime += _timer.Interval.TotalSeconds;

                // Проверяем, превышает ли время неактивности заданный порог
                if (_inactiveTime >= 600)
                {
                    // Если время неактивности превышает порог, то закрываем окно
                    Close();
                }
            }
            else
            {
                // Если окно активно, то обнуляем счетчик времени неактивности
                _inactiveTime = 0;
            }
        }
        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string partName = messagesSendTextBox.Text;

                // Захватываем семафор, чтобы убедиться, что количество одновременных подключений не превышает 3
                //serverSemaphore.WaitOne();

                // Отправляем запрос на сервер
                byte[] requestData = Encoding.UTF8.GetBytes(partName);
                await udpClient.SendAsync(requestData, requestData.Length, new IPEndPoint(serverAddress, serverPort));

                // Получаем ответ от сервера
                UdpReceiveResult response = await udpClient.ReceiveAsync();
                string responseString = Encoding.UTF8.GetString(response.Buffer);

                Dispatcher.Invoke(() => messagesTextBox.Text += responseString + "\n");
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    messagesTextBox.Text += ($"Error occurred: {ex.Message}" + "\n");
                });
            }
       
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {

            udpClient = new UdpClient();
            serverAddress = IPAddress.Parse("127.0.0.1");
            serverPort = 8080;

            // Ждем доступного слота на сервере
            

            Dispatcher.Invoke(() =>
            {
                messagesTextBox.Text += "Connected" + "\n";
            });


        }

        private void ComboBoxRec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = ComboBoxRec.SelectedItem.ToString();
            string wordToRemove = "System.Windows.Controls.Label: ";
            string newStr = str.Replace(wordToRemove, "");
            messagesSendTextBox.Text = newStr;
           
        }
    }
}
