using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hexfunc
{
    class hexwrite
    {
        static void Main()
        {
            float data = 12.34f;

            ///int data2 =300;

            //MessageBox.Show("EEPROM Data: " + Convert.ToString(data2, 16));
            int nValue = 0;


            int nSign;
            if (data >= 0)
            {
                nSign = 0x00;

            }
            else
            {
                nSign = 0x01;

                data = data * (-1);
            }
            int nHead = (int)data;
            float fTail = data % 1;
            String str = Convert.ToString(nHead, 2);
            int nHead_Length = str.Length;

            nValue = nHead;

            int nShift = nHead_Length;
            while (nShift < 24)   // (nHead_Length + nShift < 23)
            {
                if ((fTail * 2) >= 1)
                {
                    nValue = (nValue << 1) | 0x00000001;
                }
                else
                {
                    nValue = (nValue << 1);
                }
                fTail = (fTail * 2) % 1;
                nShift++;
            }

            int nExp = nHead_Length - 1 + 127;
            nExp = nExp << 23;
            nValue = nValue & 0x7FFFFF;
            nValue = nValue | nExp;
            nSign = nSign << 31;
            nValue = nValue | nSign;
            Console.WriteLine(nValue);
            int data1, data2, data3, data4;
            data1 = nValue & 0x000000FF;
     
            data2 = (nValue & 0x0000FF00) >> 8;
            data3 = (nValue & 0x00FF0000) >> 16;
            data4 = (nValue >> 24) & 0x000000FF;
           

            //浮点数转字节
           /* int data1 = 0x41;
            int data2 = 0x45;
            int data3 = 0x70;
            int data4 = 0xA4;

            int data = data1 << 24 | data2 << 16 | data3 << 8 | data4;

            int nSign;
            if ((data & 0x80000000) > 0)
            {
                nSign = -1;
            }
            else
            {
                nSign = 1;
            }
            int nExp = data & (0x7F800000);
            nExp = nExp >> 23;
            float nMantissa = data & (0x7FFFFF);

            if (nMantissa != 0)
                nMantissa = 1 + nMantissa / 8388608;

            float value = nSign * nMantissa * (2 << (nExp - 128));
            Console.WriteLine(value);
           */
        }
    }
}
