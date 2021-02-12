using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;


namespace ChatTCPFormExample
{
    public partial class Form1 : Form
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.Write("Введите свое имя: ");
            userName = "@#$";
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = userName;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
               // Console.WriteLine("Добро пожаловать, {0}", userName);

                textBox2.Text = ("Добро пожаловать, {0}", userName) + "\r\n" + textBox2.Text;
                // SendMessage();
            }
            catch (Exception ex)
            {

                
                Console.WriteLine(ex.Message);
            }
            finally
            {
               // Disconnect();
            }
        }








        // отправка сообщений
        void SendMessage(string message)
        {
            //Console.WriteLine("Введите сообщение: ");

            //while (true)
            //{
               // string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                textBox2.Text = message + "\r\n" + textBox2.Text;

            //}
        }
        // получение сообщений
        void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    //Console.WriteLine(message);//вывод сообщения
                    //textBox2.Text = message + "\r\n" + textBox2.Text;


                    Invoke(new MethodInvoker(() =>
                    {
                        //string time = DateTime.Now.ToShortTimeString();
                        addData(/*time + " " +*/ message + "\r\n" + textBox2.Text);
                    }));



                    if (message == "PleaseTurnOffTheClient")
                    {
                       

                        //textBox2.Text = message + " Подключение прервано!" + "\r\n" + textBox2.Text;

                        Invoke(new MethodInvoker(() =>
                        {
                            addData(message + " Подключение прервано!" + "\r\n" + textBox2.Text);
                        }));




                        Disconnect();
                        break;
                    }





                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано

                    //Invoke(new MethodInvoker(() =>
                    //{
                    //    addData("Подключение прервано!" + "\r\n" + textBox2.Text);
                    //}));


                    //Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
           // Environment.Exit(0); //завершение процесса
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text);
        }



        object locker = new object();
        public void addData(string data)
        {
            lock (locker)
            {

                try
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        textBox2.Text = time + " " + data + "\r\n" + textBox2.Text;
                    }));

                }
                catch
                {
                    //Form1.allDead = false;

                    ////if (server != null)
                    //server.DisconnectListener();
                }



            }

        }








        }
}
