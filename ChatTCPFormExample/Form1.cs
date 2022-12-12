//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Threading;
//using System.Net.Sockets;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;


//namespace ChatTCPFormExample
//{
//    public partial class Form1 : Form
//    {
//        static string userName;
//        private const string host = "169.254.5.120";//"127.0.0.1";
//        private const int port = 8888;
//        static TcpClient client;
//        static NetworkStream stream;



//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            //Console.Write("Введите свое имя: ");
//            userName = "@#$";
//            client = new TcpClient();
//            try
//            {
//                client.Connect(host, port); //подключение клиента
//                stream = client.GetStream(); // получаем поток

//                string message = userName;
//                byte[] data = Encoding.UTF8.GetBytes(message);
//                stream.Write(data, 0, data.Length);

//                // запускаем новый поток для получения данных
//                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
//                receiveThread.Start(); //старт потока
//               // Console.WriteLine("Добро пожаловать, {0}", userName);

//                textBox2.Text = ("Добро пожаловать, {0}", userName) + "\r\n" + textBox2.Text;
//                // SendMessage();
//            }
//            catch (Exception ex)
//            {


//                Console.WriteLine(ex.Message);
//            }
//            finally
//            {
//               // Disconnect();
//            }
//        }








//        // отправка сообщений
//        void SendMessage(string message)
//        {
//            //Console.WriteLine("Введите сообщение: ");

//            //while (true)
//            //{
//               // string message = Console.ReadLine();
//                byte[] data = Encoding.UTF8.GetBytes(message);
//                stream.Write(data, 0, data.Length);
//                textBox2.Text = message + "\r\n" + textBox2.Text;

//            //}
//        }
//        // получение сообщений
//        void ReceiveMessage()
//        {
//            while (true)
//            {
//                try
//                {
//                    if (file) continue;

//                    byte[] data = new byte[256]; // буфер для получаемых данных
//                    StringBuilder builder = new StringBuilder();
//                    int bytes = 0;
//                    do
//                    {
//                        bytes = stream.Read(data, 0, data.Length);
//                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
//                    }
//                    while (stream.DataAvailable);

//                    string message = builder.ToString();
//                    //Console.WriteLine(message);//вывод сообщения
//                    //textBox2.Text = message + "\r\n" + textBox2.Text;


//                    Invoke(new MethodInvoker(() =>
//                    {
//                        //string time = DateTime.Now.ToShortTimeString();
//                        addData(/*time + " " +*/ message + "\r\n" + textBox2.Text);
//                    }));



//                    if (message == "PleaseTurnOffTheClient")
//                    {


//                        //textBox2.Text = message + " Подключение прервано!" + "\r\n" + textBox2.Text;

//                        Invoke(new MethodInvoker(() =>
//                        {
//                            addData(message + " Подключение прервано!" + "\r\n" + textBox2.Text);
//                        }));




//                        Disconnect();
//                        break;
//                    }





//                }
//                catch
//                {
//                    Console.WriteLine("Подключение прервано!"); //соединение было прервано

//                    //Invoke(new MethodInvoker(() =>
//                    //{
//                    //    addData("Подключение прервано!" + "\r\n" + textBox2.Text);
//                    //}));


//                    //Console.ReadLine();
//                    Disconnect();
//                    return;
//                }
//            }
//        }

//        static void Disconnect()
//        {
//            if (stream != null)
//                stream.Close();//отключение потока
//            if (client != null)
//                client.Close();//отключение клиента
//           // Environment.Exit(0); //завершение процесса
//        }

//        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            Disconnect();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            SendMessage(textBox1.Text);
//        }



//        object locker = new object();
//        public void addData(string data)
//        {
//            lock (locker)
//            {

//                try
//                {
//                    this.Invoke(new MethodInvoker(() =>
//                    {
//                        string time = DateTime.Now.ToShortTimeString();
//                        textBox2.Text = time + " " + data + "\r\n" + textBox2.Text;
//                    }));

//                }
//                catch
//                {
//                    //Form1.allDead = false;

//                    ////if (server != null)
//                    //server.DisconnectListener();
//                }



//            }

//        }


//        bool file = false;

//        private void button2_Click(object sender, EventArgs e)
//        {
//            if (openFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                string FileName = openFileDialog1.FileName;//FolderForTemples + LastNameOfTemple + ".le";
//                if (File.Exists(FileName))
//                {
//                    file = true;

//                    BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));
//                    byte[] Buffer = Encoding.UTF8.GetBytes("This is a LE file");
//                    stream.Write(Buffer, 0, Buffer.Length);
//                    Thread.Sleep(50);
//                    //Buffer = new byte[256];
//                    while (reader.BaseStream.Position < reader.BaseStream.Length - 256)
//                    {
//                        Buffer = reader.ReadBytes(256);
//                        stream.Write(Buffer, 0, Buffer.Length);
//                        Thread.Sleep(50);
//                    }
//                    int len = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
//                    Buffer = reader.ReadBytes(len);
//                    stream.Write(Buffer, 0, Buffer.Length);

//                    Thread.Sleep(50);

//                    Buffer = Encoding.UTF8.GetBytes("This is the end of file");
//                    stream.WriteAsync(Buffer, 0, Buffer.Length);

//                    Thread.Sleep(50);

//                    reader.Close();

//                    file = false;

//                    ////Считываем ответ
//                    //len = ClientServer.Read(bufferForReading, 0, 256);
//                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) != "LE success")
//                    //{
//                    //    MessageBox.Show("Не удалось загрузить файл:" + FileName);
//                    //    this.Close();
//                    //}
//                    //SetCounter(Counter);
//                    //SetNumber(LastNumber);
//                    //LayerTextBox.Text = GetFixNumber();
//                }
//            }
//        }

//        private void button3_Click(object sender, EventArgs e)
//        {
//            if (openFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                string FileName = openFileDialog1.FileName;//FolderForTemples + LastNameOfTemple + ".le";
//                if (File.Exists(FileName))
//                {
//                    file = true;

//                    //BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));
//                    //byte[] Buffer = Encoding.UTF8.GetBytes("This is a TXT file");
//                    //stream.Write(Buffer, 0, Buffer.Length);
//                    //Thread.Sleep(20);
//                    ////Buffer = new byte[256];
//                    //while (reader.BaseStream.Position < reader.BaseStream.Length - 256)
//                    //{
//                    //    Buffer = reader.ReadBytes(256);
//                    //    stream.Write(Buffer, 0, Buffer.Length);
//                    //    Thread.Sleep(20);
//                    //}
//                    //int len = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
//                    //Buffer = reader.ReadBytes(len);
//                    //stream.Write(Buffer, 0, Buffer.Length);

//                    //Thread.Sleep(20);

//                    //Buffer = Encoding.UTF8.GetBytes("This is the end of file");
//                    //stream.WriteAsync(Buffer, 0, Buffer.Length);

//                    //Thread.Sleep(20);


//                    //reader.Close();



//                    List<string> Data = new List<string>();

//                    //if (openFileDialog1.ShowDialog() == DialogResult.OK)
//                    //{
//                        Data.AddRange(File.ReadAllLines(openFileDialog1.FileName, Encoding.UTF8));
//                    //}


//                    byte[] Buffer = Encoding.UTF8.GetBytes("This is a TXT file");
//                    stream.Write(Buffer, 0, Buffer.Length);
//                    Thread.Sleep(50);

//                    for (int i = 0; i < Data.Count; i++)
//                    {
//                        string dataOut = Data[i] + "*?";

//                        //Buffer = ObjectToByteArray(dataOut);

//                        Buffer = Encoding.UTF8.GetBytes(dataOut);
//                        stream.Write(Buffer, 0, Buffer.Length);
//                        Thread.Sleep(50);

//                    }

//                    Buffer = Encoding.UTF8.GetBytes("This is the end of file");
//                    stream.Write(Buffer, 0, Buffer.Length);

//                    Thread.Sleep(50);


//                    //int len = ClientServer.Read(bufferForReading, 0, 256);
//                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) == "TXT sucsses")
//                    //{
//                    //    MessageBox.Show((string)ByteArrayToObject(bufferForReading, bufferForReading.Length).ToString());
//                    //    //this.Close();
//                    //}
//                    //else
//                    //{
//                    //    MessageBox.Show((string)ByteArrayToObject(bufferForReading, bufferForReading.Length).ToString());
//                    //}






//                    file = false;

//                    ////Считываем ответ
//                    //len = ClientServer.Read(bufferForReading, 0, 256);
//                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) != "LE success")
//                    //{
//                    //    MessageBox.Show("Не удалось загрузить файл:" + FileName);
//                    //    this.Close();
//                    //}
//                    //SetCounter(Counter);
//                    //SetNumber(LastNumber);
//                    //LayerTextBox.Text = GetFixNumber();
//                }
//            }
//        }









//        public static byte[] ObjectToByteArray(Object obj)
//        {
//            BinaryFormatter bf = new BinaryFormatter();
//            using (var ms = new MemoryStream())
//            {
//                bf.Serialize(ms, obj);
//                return ms.ToArray();
//            }
//        }








//    }
//}
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace ChatTCPFormExample
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Ключ API, используется для подтверждения данных при получении 
        /// </summary>
        static string ApiKey = "WS56MH038517R3bve3SbPAOcOp3vLe2F";//"ZyzSsFxcmtoC1LNivMqkWRkbiMqeSv4R";

        /// <summary>
        /// Идентификатор подключения
        /// </summary>
        public string ID;

        /// <summary>
        /// Преамбула из ApiKey и ID в формате string
        /// </summary>
        public string Prefix;

        /// <summary>
        /// Пеамбула из ApiKey и ID в формате byte[]
        /// </summary>
        public byte[] dataPrefix;

        /// <summary>
        /// IP - адрес
        /// </summary>
        private const string host = "169.254.5.120"; //"127.0.0.1";

        /// <summary>
        /// Номер порта
        /// </summary>
        private const int port = 8888;

        /// <summary>
        /// TcpClient для подключения к MG
        /// </summary>
        private static TcpClient client;

        /// <summary>
        /// Доступ к потоку данных
        /// </summary>
        private static NetworkStream stream;

        /// <summary>
        /// Флаг успешного подключения
        /// </summary>
        public bool OK = false;

        public Form1()
        {
            InitializeComponent();

            // Запуск MG
            StartMG();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Подключаемся к сокету MG

            client = new TcpClient();

            try
            {
                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            //Ожидание подключения
            while (!OK)
            {
                Application.DoEvents();
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// Запуск MG с парметрами
        /// </summary>
        public void StartMG()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //Путь к файлу MaxiGraf.exe
            p.StartInfo.FileName = @"E:\Assembly MaxiGraf Last(Current)\LaserEditorCore\bin\x64\Debug\net5.0-windows\MaxiGraf.exe";

            //TCP - запуск MG в скрытом виде виде  / TCP_U - запуск MG в развернутом виде
            //Нужно передовать тип запуска, IP-addres, port namber, ApiKey
            //ПРимер: TCP_U 169.254.5.120 8888 ZyzSsFxcmtoC1LNivMqkWRkbiMqeSv4R
            p.StartInfo.Arguments = "TCP_U " + host + " " + port + " " + ApiKey;
            p.Start();
            p.WaitForInputIdle();

        }



        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);

                OK = true;

                stream = client.GetStream(); // получаем поток

                string message = ApiKey;

                //Отпровляем ApiKey
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(50);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока



                Invoke(new MethodInvoker(() =>
                {
                    addData(string.Format("ApiKey, {0}", ApiKey) + "\r\n");
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                OK = false;

                // Переподключается 
                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            }
        }


        /// <summary>
        ///  отправка сообщений
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="usePrefix">Использовать префикс</param>
        void SendMessage(string message, bool usePrefix = false)
        {
            byte[] data = Encoding.UTF8.GetBytes(usePrefix == false ? message : Prefix + message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(50);

        }
        // получение сообщений
        void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    if (file) continue;

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

                    System.Diagnostics.Debug.WriteLine(message);

                    Invoke(new MethodInvoker(() =>
                    {
                        addData(message);
                    }));






                    if (message.Contains("PleaseTurnOffTheClient"))
                    {
                        //Штатное зарешение соединения

                        Invoke(new MethodInvoker(() =>
                        {
                            addData(message + " Подключение прервано!" + "\r\n");
                        }));

                        Disconnect();
                        break;
                    }
                    else if (message.Contains("YourIdIs="))
                    {
                        //Получаем ID и формируем префикс

                        string[] mes = message.Split('=');

                        for (int i = 0; i < mes.Length; i++)
                            System.Diagnostics.Debug.WriteLine(string.Format("mes {0} {1}", i, mes[i]));


                        ID = mes[1];

                        Prefix = ApiKey + '|' + ID + '|';

                        System.Diagnostics.Debug.WriteLine(string.Format(
                            "ApiKey {0}" + Environment.NewLine +
                            "ID {1}" + Environment.NewLine +
                            "Prefix {2}" + Environment.NewLine
                            ,
                            ApiKey, ID, Prefix
                            ));

                        dataPrefix = Encoding.UTF8.GetBytes(Prefix);

                        byte[] dataL = Encoding.UTF8.GetBytes("Api Tcp Prefix Length" + "=" + dataPrefix.Length);
                        stream.Write(dataL, 0, dataL.Length);
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Подключение прервано! " + ex.Message); //соединение было прервано
                    Disconnect();
                    return;
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text, usePrefix: true);
        }



        object locker = new object();
        public void addData(string data_in)
        {
            lock (locker)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        textBox2.Text += time + " " + data_in + "\r\n";
                    }));

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Подключение прервано! " + ex.Message); //соединение было прервано
                    Disconnect();
                    return;
                }
            }
        }


        bool file = false;


        /// <summary>
        /// Передача LE файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                if (File.Exists(FileName))
                {
                    file = true;

                    BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));

                    byte[] Buffer = Encoding.UTF8.GetBytes(Prefix + "This is a LE file");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    byte[] UB = dataPrefix;


                    while (reader.BaseStream.Position < reader.BaseStream.Length - 256)
                    {
                        Buffer = reader.ReadBytes(256);

                        UB = dataPrefix;

                        //Прибавляем к каждому пакету префикс

                        Array.Resize(ref UB, UB.Length + Buffer.Length);
                        Array.Copy(Buffer, 0, UB, UB.Length - Buffer.Length, Buffer.Length);


                        stream.Write(UB, 0, UB.Length);
                        Thread.Sleep(50);
                    }

                    int len = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                    Buffer = reader.ReadBytes(len);


                    UB = dataPrefix;

                    Array.Resize(ref UB, UB.Length + Buffer.Length);
                    Array.Copy(Buffer, 0, UB, UB.Length - Buffer.Length, Buffer.Length);


                    stream.Write(UB, 0, UB.Length);
                    Thread.Sleep(50);

                    Buffer = Encoding.UTF8.GetBytes(Prefix + "This is the end of file");
                    stream.WriteAsync(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    reader.Close();

                    file = false;
                }
            }
        }


        /// <summary>
        /// Передача TXT (скрипта) файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                if (File.Exists(FileName))
                {
                    file = true;

                    List<string> Data = new List<string>();

                    Data.AddRange(File.ReadAllLines(openFileDialog1.FileName, Encoding.UTF8));

                    //Прибавляем к каждой строке префикс

                    byte[] Buffer = Encoding.UTF8.GetBytes(Prefix + "This is a TXT file");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    for (int i = 0; i < Data.Count; i++)
                    {
                        string dataOut = Data[i] + "*?";

                        Buffer = Encoding.UTF8.GetBytes(Prefix + dataOut);
                        stream.Write(Buffer, 0, Buffer.Length);
                        Thread.Sleep(50);
                    }

                    Buffer = Encoding.UTF8.GetBytes(Prefix + "This is the end of file");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    file = false;
                }
            }
        }

    }
}
