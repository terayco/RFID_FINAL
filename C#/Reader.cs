using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HFrfid;
using Fleck;
using System.Speech.Synthesis;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace develop
{
    class rfidReader
    {
        static RfidReader Reader = new RfidReader();//初始化对象
        public static string byteToHexStr(byte[] bytes, int len)  //数组转十六进制字符
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        private static byte[] strToToHexByte(string hexString) //字符串转16进制
        {
            //hexString = hexString.Replace(" ", " "); 
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private static void openPort(string port) 
        {
            bool flg = Reader.Connect(port, 9600);
            if (flg == true)
            {
                Console.WriteLine("打开端口成功");
            }
            else
            {
                Console.WriteLine("端口无法打开");
            }
        }
        //中文转16进制
        public static string GetHexFromChs(string s)  
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                         //throw new ArgumentException("s is not valid chinese string!");
            }

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");

            byte[] bytes = chs.GetBytes(s);

            string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
            }

            return str;
        }
        //16进制转中文
        public static string GetChsFromHex(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
                            //throw new ArgumentException("hex is not a valid number!", "hex");
            }
            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                        System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message.
                    throw new ArgumentException("不是一个有效十六进制数!", "hex");
                }
            }

            // 获得 GB2312，Chinese Simplified。
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");


            return chs.GetString(bytes);
        }
        private static string returnCardId()     //读卡号
        {
            int status;
            byte[] type = new byte[2];
            byte[] id = new byte[4];
            Reader.Cmd = Cmd.M1_ReadId;//读卡号命令
            Reader.Addr = Convert.ToByte("20", 16);//读写器地址,设备号
            Reader.Beep = Beep.On;
            status = Reader.M1_Operation();
            if (status == 0)//读卡成功
            {
                for (int i = 0; i < 2; i++)//获取2字节卡类型
                {
                    type[i] = Reader.RxBuffer[i];
                }
                for (int i = 0; i < 4; i++)//获取4字节卡号
                {
                    id[i] = Reader.RxBuffer[i + 2];
                }
                string ss = byteToHexStr(type, 2);
                ss = byteToHexStr(id, 4);
                return ss;
            }
            else
                return null;
        }

        private static string readData(string dataBlock, string mode)     //读数据
        {
            int status;
            byte[] blockdata = new byte[16];
            
            Reader.Cmd = Cmd.M1_KeyA_ReadBlock; //读卡数据块命令，验证KeyA
          

            Reader.Addr = Convert.ToByte("20", 16); //读写器地址
            //块数用变量代替
            Reader.M1Block = Convert.ToByte(dataBlock, 10);//数据块块号
            Reader.Beep = Beep.On;//读数据块成功蜂鸣器提示

            status = Reader.M1_Operation();//读操作
            if (status == 0)//读成功
            {
                for (int i = 0; i < 16; i++)
                {
                    blockdata[i] = Reader.RxBuffer[i];//获取读到的数据，将读到的数据拷贝到blockdata数组
                }
                
                string ss = byteToHexStr(blockdata, 16); //将读取的数据转换成字符串
               
                int index = 0;
             
                //找到第一个非0元素
                while (ss[index] == '0' && index < 31)  //读取16字节数据
                {
                    index += 1;
                }
               //Console.WriteLine("index="+index);     
                //把前面的0去掉
                ss = ss.Substring(index,ss.Length-index);

                //Console.WriteLine(ss);
                Console.WriteLine("读取数据成功!");
                if (mode == "chinese")
                    ss = GetChsFromHex(ss);
                return ss;
            }
            else
            {
               Console.WriteLine( "错误码：" + status.ToString());
            }
            return null;
        }
        //写数据
        private static void writeData(string dataBlock,string dataWritten, string mode)
        {
            int status;

            byte[] buff = new byte[16];
           if (mode == "chinese")
            {
                dataWritten = GetHexFromChs(dataWritten);
            }
           while (dataWritten.Length != 32)
            {
                //将数据补足32位
                dataWritten = "0" + dataWritten ;
            }
            byte[] data = strToToHexByte(dataWritten);

            Reader.Cmd = Cmd.M1_KeyA_WriteBlock;
            
            //Reader.Cmd = Cmd.M1_KeyB_WriteBlock;

            Reader.Addr = Convert.ToByte("20", 16);
           Reader.M1Block = Convert.ToByte(dataBlock, 10);
            Reader.Beep = Beep.On;

            for (int i = 0; i < 16; i++)
                Reader.TxBuffer[i] = data[i];//将要写的16字节数保存到结构体成员TxBuffer数组中

            status = Reader.M1_Operation();
            if (status == 0x00)//写成功
            {
                Console.WriteLine( "写数据到指定块{1}成功", dataWritten,dataBlock);
                return;
            }
            Console.WriteLine( "错误码：" + status.ToString());
        }
        private static int initWallet(float moneyf)
        {
            int status;
            //浮点数转为16进制字符串
            string money = BitConverter.ToString(BitConverter.GetBytes(moneyf));
            money = money.Replace("-", "");
            //Console.WriteLine(money);
            Reader.Cmd = Cmd.M1_KeyA_InitVal;
            byte[] buff = new byte[16];

             while (money.Length != 32)
             {
                 //将数据补足32位
                 money = money + "0";
             }
             byte[] data = strToToHexByte(money);
       
            
            Reader.Addr = Convert.ToByte("20", 16);
            Reader.M1Block = Convert.ToByte("60", 10);
            Reader.Beep = Beep.On;

            for (int i = 0; i < 16; i++)
                Reader.TxBuffer[i] = data[i];//将要写的16字节数保存到结构体成员TxBuffer数组中

            status = Reader.M1_Operation();
            if (status == 0x00)//写成功
            {
                Console.WriteLine("钱包初始化成功",money);
                return 1;
            }
            else
                Console.WriteLine("错误码：" + status.ToString());
            return 0;
        }
        
        private static string readWallet()     //查询钱包余额
        {
            int status;
            byte[] blockdata = new byte[16];
            //if (comboBox1_1.Text == "KeyA")
            Reader.Cmd = Cmd.M1_KeyA_ReadBlock; 

            Reader.Addr = Convert.ToByte("20", 16); //读写器地址
            //块数用变量代替
            Reader.M1Block = Convert.ToByte("60", 10);//数据块块号
            Reader.Beep = Beep.On;//读数据块成功蜂鸣器提示

            status = Reader.M1_Operation();//读操作
            if (status == 0)//读成功
            {
                for (int i = 0; i < 16; i++)
                {
                    blockdata[i] = Reader.RxBuffer[i];//获取读到的数据，将读到的数据拷贝到blockdata数组
                }

                string ss = byteToHexStr(blockdata, 16); //将读取的数据转换成字符串
                //Console.WriteLine("字符串" + ss);
                // Int32 pvalue = BitConverter.ToInt32(blockdata, 0);
                // ss = pvalue.ToString();
                byte[] arr = new byte[ss.Length / 2];
                for (int i = 0; i < ss.Length / 2; i++)
                {
                    arr[i] = Convert.ToByte(ss.Substring(i * 2, 2), 16);
                }
                float f1 = BitConverter.ToSingle(arr, 0);
                string result = f1.ToString();
                Console.WriteLine("钱包查询成功!");
                return result;
            }
            else
            {
                Console.WriteLine("错误码：" + status.ToString());
            }
            return null;
        }
        private static int chargeWallet(float money)   //钱包扣钱
        {
            float current_money = float.Parse(readWallet());
            float result = current_money - money;
            if(initWallet(result) == 1)
            {
                Console.WriteLine("钱包扣钱成功！");
                return 1;
            }
            else
            {
                Console.WriteLine("钱包扣钱失败！");
                return 0;
            }
        }

        private static int addWallet(float money) //钱包加钱
        {

            float current_money = float.Parse(readWallet());
            float result = current_money + money;
            if (initWallet(result) == 1)
            {
                Console.WriteLine("钱包增值成功！");
                return 1;
            }
            else
            {
                Console.WriteLine("钱包增值失败！");
                return 0;
            }
        }
        static void Main(string[] args)
        {

            openPort("COM7");
            string cardId = returnCardId();
           
            //writeData("61", "1", "data");
           // Thread.Sleep(100);


            //阻塞位
            bool isUsing = false;
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://0.0.0.0:7181");
            string workerName = "";
            string jobNumber = "";
            string workerSex = "";
            string workerBalance = "";
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("与客户端已经连接上 Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("与客户端已经断开连接 Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    isUsing = true;
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("客户端 Echo: " + message));
                    if (message.Split(',')[0] == "0") //充值
                    {
                        try
                        {
                            string data = message.Split(',')[1];
                            float money = float.Parse(data);
                            addWallet(money);
                            Thread.Sleep(100);

                            jobNumber = readData("56", "data");
                            Thread.Sleep(100);

                            workerName = readData("57", "chinese");
                            Thread.Sleep(100);

                            workerSex = readData("58", "chinese");
                            Thread.Sleep(100);

                            string currentState = "1";

                            workerBalance = readWallet();
                            string sendData = currentState + "," + jobNumber + "," + workerName + "," + workerSex + "," + workerBalance;
                            socket.Send(sendData);
                           // Console.WriteLine("发送数据3：" + sendData);
                        }
                        catch
                        {
                            Console.WriteLine("操作失败，请重试！");
                        }

                        
                    }
                    else if (message.Split(',')[0] == "1") //扣钱
                    {
                        try
                        {
                            string data = message.Split(',')[1];
                            float money = float.Parse(data);
                            chargeWallet(money);
                            Thread.Sleep(100);

                            jobNumber = readData("56", "data");
                            Thread.Sleep(100);

                            workerName = readData("57", "chinese");
                            Thread.Sleep(100);

                            workerSex = readData("58", "chinese");
                            Thread.Sleep(100);

                            string currentState = "1";

                            workerBalance = readWallet();
                            string sendData = currentState + "," + jobNumber + "," + workerName + "," + workerSex + "," + workerBalance;
                            socket.Send(sendData);
                            //Console.WriteLine("发送数据4：" + sendData);
                        }
                        catch
                        {
                            Console.WriteLine("操作失败，请重试！");
                        }
                    }
                    else     //初始化卡
                    {
                        try
                        {

                            jobNumber = message.Split(',')[1];
                            workerName = message.Split(',')[2];
                            workerSex = message.Split(',')[3];

                            float money = float.Parse(message.Split(',')[4]);
                            Console.WriteLine("money" + money);
                            string cardStatus = message.Split(',')[5];
                            writeData("56", jobNumber, "data");
                            Thread.Sleep(100);
                         
                            writeData("57", workerName, "chinese");
                            Thread.Sleep(100);

                            writeData("58", workerSex, "chinese");
                            Thread.Sleep(100);
                          
                            initWallet(money);
                            workerBalance = money.ToString();
                            Thread.Sleep(100);

                            writeData("61", cardStatus, "data");
                            Thread.Sleep(100);
                           
                            string sendData = jobNumber + "," + workerName + "," + workerSex + "," + money;
                            Thread.Sleep(300);
                            socket.Send("1"+sendData);
                            Console.WriteLine("修改数据：" + sendData);
                        }
                        catch
                        {
                            Console.WriteLine("操作失败，请重试！");
                        }
                    }
                    isUsing = false;
                };
            });


            int sendAccount = 0;
            while (true)
            {
                foreach (var socket in allSockets.ToList())
                {

                    if (sendAccount == 0)
                    { 
                        if (returnCardId() != null)
                        {   //读到卡则发送1标志位
                            string card_status = readData("61", "data");
                            if (card_status == "0" && sendAccount == 0)
                            {
                                //正常卡
                                string currentState = "1";
                                
                                jobNumber = readData("56", "data");
                                Thread.Sleep(100);
                                
                                workerName = readData("57", "chinese");
                                Thread.Sleep(100);
                               
                                workerSex = readData("58", "chinese");
                                Thread.Sleep(100);
                              
                                workerBalance = readWallet();
                                string sendData = currentState + "," + jobNumber + "," + workerName + "," + workerSex + "," + workerBalance;
                                socket.Send(sendData);

                                sendAccount += 1;
                                Thread.Sleep(2000);
                            }
                        
                            else if (card_status == "1" || card_status == "2")
                            {
                                //挂失卡或者黑卡
                                string currentState = "2";
                                string msg;
                                if (card_status == "1")
                                {
                                    msg = "该卡为挂失卡,请先解除挂失状态！";
                                    string sendData = currentState + "," + msg;
                                    socket.Send(sendData);
                                    SpeechSynthesizer speech = new SpeechSynthesizer();
                                    speech.Speak("该卡为挂失卡,请先解除挂失状态！");
                                }

                                else
                                {
                                    msg = "识别到黑卡，禁止操作！";
                                    string sendData = currentState + "," + msg;
                                    socket.Send(sendData);
                                    SpeechSynthesizer speech = new SpeechSynthesizer();
                                    speech.Speak("识别到黑卡，禁止操作！");
                                }
                            }
                           
                        }
                        else
                        {   //没读到卡，等待态
                            string currentState = "0";
                            socket.Send(currentState);
                    
                        }
                    }
                    else
                    {
                        if(isUsing == false)
                        {
                            if (returnCardId() != null)
                            {

                                string card_status = readData("61", "data"); 
                                if(card_status == "1" || card_status == "2")
                                {
                                    if(card_status == "2" && isUsing == false)
                                    {
                                        string msg = "识别到黑卡，禁止操作！";
                                        string sendData = "2" + "," + msg;
                                        socket.Send(sendData);
                                        SpeechSynthesizer speech = new SpeechSynthesizer();
                                        speech.Speak(msg);
                                        Thread.Sleep(1000);
                                    }

                                    else if(card_status == "1" && isUsing == false)
                                    {
                                        string msg = "该卡为挂失卡，请先解除挂失状态！";
                                        string sendData = "2" + "," + msg;
                                        socket.Send(sendData);
                                        SpeechSynthesizer speech = new SpeechSynthesizer();
                                        speech.Speak(msg);
                                        Thread.Sleep(1000);
                                    }              
                                }
                                else
                                {
                                    string currentState = "1";
                                    string sendData = currentState + "," + jobNumber + "," + workerName + "," + workerSex + "," + workerBalance;
                                    Thread.Sleep(1000);
                                    socket.Send(sendData);
                                }
   
                            }
                            else
                            {
                                string currentState = "0";
                                socket.Send(currentState);
                                Console.WriteLine("未识别到卡");
                                Thread.Sleep(500);
                            }
                        } 
                    }
                }
                Thread.Sleep(500);
            }

        }
    }
}
