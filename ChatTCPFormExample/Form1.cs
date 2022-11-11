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
        static string ApiKey = "ZyzSsFxcmtoC1LNivMqkWRkbiMqeSv4R";
        public string ID;
        
        
        public string Prefix;

        public byte[] dataPrefix;


        private const string host = "169.254.5.120";//"127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

       

        public Form1()
        {
            InitializeComponent();

            Init();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.Write("Введите свое имя: ");
            // userName = "@#$";


            client = new TcpClient();
            try
            {

                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);



            }
            catch (Exception ex)
            {


                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Disconnect();
            }


           while (!OK)
           {
                Application.DoEvents();
                Thread.Sleep(20);
           }



        }

        public void Init()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"E:\Assembly MaxiGraf Last(Current)\LaserEditorCore\bin\win32\Debug\net5.0-windows\MaxiGraf.exe";
            //if (GlobalsProvider.settings.SuperUser)
            p.StartInfo.Arguments = "TCP_U " + host + " " + port + " " + ApiKey; // нужно передовать IP-addres and port namber
            p.Start();
            // p.WaitForExit();
            p.WaitForInputIdle();
            
        }

        bool OK = false;

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);


                OK = true;

                stream = client.GetStream(); // получаем поток

                string message = ApiKey;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                Thread.Sleep(50);
                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                                       // Console.WriteLine("Добро пожаловать, {0}", userName);


                Invoke(new MethodInvoker(() =>
                {
                    //string time = DateTime.Now.ToShortTimeString();
                    addData(/*time + " " +*/  ("Добро пожаловать, {0}", ApiKey) + "\r\n" /*+ "\r\n"*/);
                }));



                //textBox2.Text += ("Добро пожаловать, {0}", ApiKey) + "\r\n" + textBox2.Text;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            }
        }


        // отправка сообщений
        void SendMessage(string message, bool usePrefix = false)
        {
            //Console.WriteLine("Введите сообщение: ");

            //while (true)
            //{
               // string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(usePrefix == false ? message : Prefix + message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(50);
            //textBox2.Text += message + "\r\n" + textBox2.Text;

            //}
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
                    //Console.WriteLine(message);//вывод сообщения
                    //textBox2.Text = message + "\r\n" + textBox2.Text;

                    System.Diagnostics.Debug.WriteLine(message);

                    Invoke(new MethodInvoker(() =>
                    {
                        //string time = DateTime.Now.ToShortTimeString();
                        addData(/*time + " " +*/ message /*+ "\r\n"*/);
                    }));






                    if (message.Contains("PleaseTurnOffTheClient"))
                    {
                       

                        //textBox2.Text = message + " Подключение прервано!" + "\r\n" + textBox2.Text;

                        Invoke(new MethodInvoker(() =>
                        {
                            addData(message + " Подключение прервано!" + "\r\n" );
                        }));




                        Disconnect();
                        break;
                    }
                    else if(message.Contains("YourIdIs="))
                    {
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





                       // SendMessage("Api Tcp Prefix Length"  + "=" + dataPrefix.Length);


                        byte[] dataL = Encoding.UTF8.GetBytes("Api Tcp Prefix Length" + "=" + dataPrefix.Length);
                        stream.Write(dataL, 0, dataL.Length);
                        Thread.Sleep(50);
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
           // Environment.Exit(0); //завершение процесса
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
                catch
                {
                    //Form1.allDead = false;

                    ////if (server != null)
                    //server.DisconnectListener();
                }



            }

        }


        bool file = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;//FolderForTemples + LastNameOfTemple + ".le";
                if (File.Exists(FileName))
                {
                    file = true;

                    BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));
                    
                    byte[] Buffer = Encoding.UTF8.GetBytes(Prefix + "This is a LE file"); 
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);
                    
                    //Buffer = new byte[256];

                    byte[] UB = dataPrefix;
                   

                    while (reader.BaseStream.Position < reader.BaseStream.Length - 256)
                    {
                        Buffer = reader.ReadBytes(256);

                        UB = dataPrefix;
                        
                        Array.Resize(ref UB, UB.Length + Buffer.Length);
                        Array.Copy(Buffer, 0, UB, UB.Length - Buffer.Length, Buffer.Length);

                        //stream.Write(Buffer, 0, Buffer.Length);
                        stream.Write(UB, 0, UB.Length);
                        Thread.Sleep(50);
                    }
                    
                    int len = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                    Buffer = reader.ReadBytes(len);


                    UB = dataPrefix;

                    Array.Resize(ref UB, UB.Length + Buffer.Length);
                    Array.Copy(Buffer, 0, UB, UB.Length - Buffer.Length, Buffer.Length);

                    //stream.Write(Buffer, 0, Buffer.Length);
                    stream.Write(UB, 0, UB.Length);
                    Thread.Sleep(50);

                    Buffer = Encoding.UTF8.GetBytes(Prefix + "This is the end of file");
                    stream.WriteAsync(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    reader.Close();

                    file = false;
                    
                    ////Считываем ответ
                    //len = ClientServer.Read(bufferForReading, 0, 256);
                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) != "LE success")
                    //{
                    //    MessageBox.Show("Не удалось загрузить файл:" + FileName);
                    //    this.Close();
                    //}
                    //SetCounter(Counter);
                    //SetNumber(LastNumber);
                    //LayerTextBox.Text = GetFixNumber();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;//FolderForTemples + LastNameOfTemple + ".le";
                if (File.Exists(FileName))
                {
                    file = true;

                    //BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));
                    //byte[] Buffer = Encoding.UTF8.GetBytes("This is a TXT file");
                    //stream.Write(Buffer, 0, Buffer.Length);
                    //Thread.Sleep(20);
                    ////Buffer = new byte[256];
                    //while (reader.BaseStream.Position < reader.BaseStream.Length - 256)
                    //{
                    //    Buffer = reader.ReadBytes(256);
                    //    stream.Write(Buffer, 0, Buffer.Length);
                    //    Thread.Sleep(20);
                    //}
                    //int len = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                    //Buffer = reader.ReadBytes(len);
                    //stream.Write(Buffer, 0, Buffer.Length);

                    //Thread.Sleep(20);

                    //Buffer = Encoding.UTF8.GetBytes("This is the end of file");
                    //stream.WriteAsync(Buffer, 0, Buffer.Length);

                    //Thread.Sleep(20);


                    //reader.Close();



                    List<string> Data = new List<string>();

                    //if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    //{
                        Data.AddRange(File.ReadAllLines(openFileDialog1.FileName, Encoding.UTF8));
                    //}


                    byte[] Buffer = Encoding.UTF8.GetBytes(Prefix + "This is a TXT file");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Thread.Sleep(50);

                    for (int i = 0; i < Data.Count; i++)
                    {
                        string dataOut = Data[i] + "*?";

                        //Buffer = ObjectToByteArray(dataOut);

                        Buffer = Encoding.UTF8.GetBytes(Prefix + dataOut);
                        stream.Write(Buffer, 0, Buffer.Length);
                        Thread.Sleep(50);

                    }

                    Buffer = Encoding.UTF8.GetBytes(Prefix +"This is the end of file");
                    stream.Write(Buffer, 0, Buffer.Length);

                    Thread.Sleep(50);


                    //int len = ClientServer.Read(bufferForReading, 0, 256);
                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) == "TXT sucsses")
                    //{
                    //    MessageBox.Show((string)ByteArrayToObject(bufferForReading, bufferForReading.Length).ToString());
                    //    //this.Close();
                    //}
                    //else
                    //{
                    //    MessageBox.Show((string)ByteArrayToObject(bufferForReading, bufferForReading.Length).ToString());
                    //}






                    file = false;

                    ////Считываем ответ
                    //len = ClientServer.Read(bufferForReading, 0, 256);
                    //if (Encoding.UTF8.GetString(bufferForReading, 0, len) != "LE success")
                    //{
                    //    MessageBox.Show("Не удалось загрузить файл:" + FileName);
                    //    this.Close();
                    //}
                    //SetCounter(Counter);
                    //SetNumber(LastNumber);
                    //LayerTextBox.Text = GetFixNumber();
                }
            }
        }









        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }








    }
}
