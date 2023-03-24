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
        int kolConnet = 0;
        private readonly UdpClient udpServer;
        private readonly IPEndPoint ipEndPoint;
        public WindowServer()
        {
            InitializeComponent();
            udpServer = new UdpClient(8080);
            ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            //messagesTextBox2.Text += ipEndPoint.Address;
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
                        messagesTextBox2.Text += ("Received request: {0} from {1}\n", request, data.RemoteEndPoint.ToString());
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
                case "борщ":
                    {
                        return "\nИнгредиенты:\r\n\r\n500 гр. мяса (говядина или свинина)\r\n2-3 крупных свёклы\r\n2-3 морковки\r\n1 большой лук\r\n4-5 картофелин\r\n1-2 перца болгарского\r\n2-3 зубчика чеснока\r\n2-3 ст. ложки томатной пасты\r\n2-3 литра воды\r\nсоль и перец по вкусу\r\n1-2 лавровых листа\r\nзелень (укроп, петрушка)";
                    }
                   
                case "оливье":
                    return "\nИнгредиенты:\r\n\r\n4-5 картофелин\r\n3-4 морковки\r\n4 яйца\r\n1 крупный лук\r\n1 банка консервированных горошка\r\n1 банка консервированных огурчиков\r\n200 гр. ветчины или вареной колбасы\r\n200 гр. майонеза\r\nсоль и перец по вкусу";
                case "салат цезарь":
                    return "\nВот список ингредиентов для классического салата Цезарь:\r\n\r\n2 головки салата ромэн\r\n2-3 куринных грудок\r\n1 батон (или 4-5 ломтей белого хлеба)\r\n1/2 стакана тертого пармезана\r\n1/2 стакана гренок (сухариков)\r\n1 яйцо\r\n3 зубчика чеснока\r\n1/2 лимона\r\n2 столовые ложки оливкового масла\r\nсоль и перец по вкусу\r\nДля соуса Цезарь:\r\n\r\n1/2 стакана майонеза\r\n2 столовые ложки грейпфрутового сока (или лимонного сока)\r\n1 зубчик чеснока\r\n1/4 чайной ложки Дижонской горчицы\r\n1/4 чайной ложки вустерского соуса\r\n1/4 чайной ложки сахара\r\nсоль и перец по вкусу";
                case "суп гороховый шут":
                    return "\nВот список ингредиентов для супа из гороха:\r\n\r\n2 стакана сухого гороха\r\n1 луковица\r\n2 средние моркови\r\n2 стебля сельдерея\r\n2-3 зубчика чеснока\r\n2 литра куриного или овощного бульона\r\n1 лавровый лист\r\n1/2 чайной ложки молотого кумина\r\n1/2 чайной ложки молотой паприки\r\n1/4 чайной ложки молотого кориандра\r\n2 столовые ложки оливкового масла\r\nсоль и перец по вкусу\r\nОпционально:\r\n\r\nсвежая зелень для подачи";
                case "картошка жареная":
                    return "\nВот ингредиенты для жареной картошки: \n\nКартошка и сковородка";
                default:
                    return "Part not found";
            }
        }
    }
}
