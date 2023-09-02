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
        static void Main(string[] args)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.Speak("识别到黑卡，禁止操作！");
        }
    }

}
