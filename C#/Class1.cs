using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Program
    {
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
        private static string FloatToHex(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            StringBuilder stringBuffer = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i != 0)
                {
                    stringBuffer.Insert(0, "");
                }
                string hex = Convert.ToString(bytes[i], 2);
                stringBuffer.Insert(0, hex);
                // 位数不够补0
                for (int j = hex.Length; j < 8; j++)
                {
                    stringBuffer.Insert(0, "0");
                }
            }
            return stringBuffer.ToString();
        }
        static void Main(string[] args)
        {
            

            float F = 200.00F;

            string s1 = BitConverter.ToString(BitConverter.GetBytes(F));
            Console.WriteLine(s1.Replace("-",""));
            string s = "0000F241";
            byte[] arr = new byte[s.Length / 2];
            for (int i = 0; i < s.Length / 2; i++)
            {
                arr[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
            }
            float f1 = BitConverter.ToSingle(arr, 0);
            Console.WriteLine(f1);

            //写数据
            byte[] hex = new byte[16];

            hex = strToToHexByte(s);
            Console.WriteLine(hex.Length);
            for(int i = 0; i < 4; i++)
            {
                Console.WriteLine(hex[i]);
            }
            string hextostr = byteToHexStr(hex,4);
            Console.WriteLine(hextostr);

            
        }
    }
}
