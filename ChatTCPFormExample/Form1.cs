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
        //static string ApiKey = "ZyzSsFxcmtoC1LNivMqkWRkbiMqeSv4R";
        static string ApiKey = "1qfcx2J74ASqjR6ip1F636Y3yrReyYWQ";
                               // "ZyzSsFxcmtoC1LNivMqkWRkbiMqeSv4R";

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
           // StartMG();
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
            p.StartInfo.FileName = @"E:\Assembly MaxiGraf Last(Current)\LaserEditorCore\bin\x64\Debug\net5.0-windows\MaxiGraf.exe";//@"D:\MaxiGraf\MaxiGraf.exe";//@"E:\Assembly MaxiGraf Last(Current)\LaserEditorCore\bin\win32\Debug\net5.0-windows\MaxiGraf.exe";

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

                //// запускаем новый поток для получения данных
                //Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                //receiveThread.Start(); //старт потока

                BeginReading();

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


        byte[] superdata = new byte[256];


        public void BeginReading()
        {
            stream.BeginRead(
                superdata, 0, superdata.Length,
                new AsyncCallback(EndReading),
                stream);
        }



        public void EndReading(IAsyncResult ar)
        {
            try
            {

                if (file) return;

                if (SetValueArray)
                {
                    try
                    {
                        int bytes2 = stream.EndRead(ar);

                        string message2 = Encoding.UTF8.GetString(superdata, 0, bytes2);              

                        System.Diagnostics.Debug.WriteLine("EndReading_SetValueArray = " + message2.Replace('\0', '_'));
                        
                        start = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Подключение прервано! " + ex.Message); //соединение было прервано
                        Disconnect();
                        return;
                    }

                    return;
                }
               

                int bytes = stream.EndRead(ar);              
               
                StringBuilder builder = new StringBuilder();               

                string message = Encoding.UTF8.GetString(superdata, 0, bytes);              

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
                    return;
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


            BeginReading();

        }

        public bool start = false;

        public void BeginReadingSetValueArray()
        {
            if (!SetValueArray) return;

            stream.BeginRead(
                superdata, 0, superdata.Length,
                new AsyncCallback(EndReadingSetValueArray),
                stream);


        }

        public void EndReadingSetValueArray(IAsyncResult ar)
        {

            if (file) return;

            if (!SetValueArray)
            {
                try 
                { 

                    int bytes = stream.EndRead(ar);


               
                    StringBuilder builder = new StringBuilder();
               

                    string message = Encoding.UTF8.GetString(superdata, 0, bytes);// builder.ToString();                 

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
                            return;
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

                return;
            }



            try
            { 
                    int bytes = stream.EndRead(ar);

                    string message2 = Encoding.UTF8.GetString(superdata, 0, bytes);// builder.ToString();                 

                    System.Diagnostics.Debug.WriteLine("EndReading2 = " + message2.Replace('\0', '_'));

                    start = true;
            }
            catch (Exception ex)
            {
                 System.Diagnostics.Debug.WriteLine("Подключение прервано! " + ex.Message); //соединение было прервано
                 Disconnect();
                 return;
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
                        textBox2.Text += time + " " + data_in.Trim('\0') + "\r\n";
                    }));

                }
                catch
                {
                   
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

                    BeginReading();
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

                    Buffer = Encoding.UTF8.GetBytes(Prefix +"This is the end of file");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);
                    
                    file = false;

                    BeginReading();
                }
            }
        }

        private void buttonLoadLE_eql_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                if (File.Exists(FileName))
                {
                    string Command = string.Format("LoadLE={0}", openFileDialog1.FileName);
                    textBox1.Text = Command;
                    button1.PerformClick();
                }
            }
        }

        private void buttonSetValueArray_Click(object sender, EventArgs e)
        {
            string data = Prefix + "ШтрихКод.Data=A" + "\0" +
                          "ШтрихКод_1.Data=B" + "\0" +
                          "ШтрихКод_2.Data=C" + "\0" +
                          "ШтрихКод_3.Data=D" + "\0";

            byte[] databyte = UnicodeEncoding.UTF8.GetBytes(data);

            byte[] Buffer = UnicodeEncoding.UTF8.GetBytes(Prefix + "SetValueArray=" + databyte.Length.ToString());
            stream.Write(Buffer, 0, Buffer.Length);
            Thread.Sleep(50);

           
            stream.Write(databyte, 0, databyte.Length);
            Thread.Sleep(50);

            BeginReading();
        }


        bool SetValueArray = false;

        private void buttonSetValueArrayV2_Click(object sender, EventArgs e)
        {

            //НУЖНО БИТЬ НА ПАКЕТЫ 256 (256 + 70 = 326)
            //если databyte.Length > 256 бить на пакеты и несколько SetValueArray

            char separetor = '\0';

            List<string> data = new List<string>();

            for (int i = 0; i < 400; i++)
            {
                if (i == 0)
                    data.Add("ШтрихКод.Data=" + i.ToString() + separetor);
                else
                    data.Add("ШтрихКод_" + i.ToString() + ".Data=" + i.ToString() + separetor);

            }

            List<byte[]> mas_list = new List<byte[]>();

            List<byte> databyte = new List<byte>();

            for (int i = 0; i < data.Count; i++)
            {
                string mes = data[i];

                byte[] ar = UnicodeEncoding.UTF8.GetBytes(mes);

                if (databyte.Count + ar.Length < 256)
                {
                    databyte.AddRange(ar);

                    if (i == data.Count - 1)
                    {
                        mas_list.Add(databyte.ToArray());
                    }
                }
                else
                {
                    mas_list.Add(databyte.ToArray());

                    databyte = new List<byte>();

                    databyte.AddRange(ar);

                    if (i == data.Count - 1)
                    {
                        mas_list.Add(databyte.ToArray());
                    }

                }
            }

            System.Diagnostics.Debug.WriteLine("mas_list.Count = " + mas_list.Count.ToString());

            byte[] UB = dataPrefix;

            SetValueArray = true;

            foreach (byte[] b in mas_list)
            {
                byte[] B = UnicodeEncoding.UTF8.GetBytes(Prefix + "SetValueArray=" + b.Length.ToString());
                stream.Write(B, 0, B.Length);
                Thread.Sleep(50);

                UB = dataPrefix;

                //Прибавляем к каждому пакету префикс

                Array.Resize(ref UB, UB.Length + b.Length);
                Array.Copy(b, 0, UB, UB.Length - b.Length, b.Length);

                stream.Write(UB, 0, UB.Length);
                Thread.Sleep(50);

                start = false;
                BeginReadingSetValueArray();

                while (!start)
                { 
                    Thread.Sleep(50);
                }              

            }

            SetValueArray = false;

            BeginReading();
        }

        private void buttonReConnect_Click(object sender, EventArgs e)
        {
            OK = false;

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
    }
}
