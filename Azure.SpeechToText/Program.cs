using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.SpeechToText
{
   public class Program
    {
        static void Main(string[] args)
        {
            // Call the Speech to Text
            SpeechToText();
        }

        static void SpeechToText()
        {
            try
            {
                // Create the service
                Console.WriteLine("Creating the Speech to Text Service.");
                var srService = new SpeechToTextService(SpeechRecognitionMode.ShortPhrase, "en-us", Constants.Speech_API_Key);
                Console.WriteLine("Done");
                Console.WriteLine("");

                srService.SendAudio(Constants.AUDIO_DIRECTORY + "speechToText1.wav");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
            Console.ReadKey();
        }
    }
}
