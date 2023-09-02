using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class json
    {
        static void Main(string[] args)
        {
            /*
            string id = "123";
            string Name = "小黑";
            JObject patientinfo = new JObject();
            //JArray ids = new JArray();
            //ids.Add(id);
            patientinfo["currentState"] = "0";
            patientinfo["Name"] = Name;
            string sendData = JsonConvert.SerializeObject(patientinfo);
            Console.WriteLine(sendData);
            */
            string message = "1,50.00";
            string data = message.Split(',')[1];
            float money = float.Parse(data);
            Console.WriteLine(money);
            Console.WriteLine(data);
            Console.WriteLine(message[3]);

        }
    }
}
