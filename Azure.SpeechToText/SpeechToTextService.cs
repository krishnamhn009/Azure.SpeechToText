using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.SpeechToText
{
    public class SpeechToTextService
    {
        public DataRecognitionClient serviceClient { get; }

        public SpeechToTextService(SpeechRecognitionMode mode, string language, string subscriptionKey)
        {
           serviceClient = SpeechRecognitionServiceFactory.CreateDataClient(mode, language, subscriptionKey);
           

            // Event handlers for speech recognition results
            serviceClient.OnResponseReceived += this.OnResponseReceivedHandler;
            serviceClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            serviceClient.OnConversationError += this.OnConversationErrorHandler;
        }
        private void OnResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
            {
                Console.WriteLine("[{0}]", i);
                Console.WriteLine("Confidence={0}", e.PhraseResponse.Results[i].Confidence);
                Console.WriteLine("DisplayText={0}", e.PhraseResponse.Results[i].DisplayText);
                Console.WriteLine("InverseTextNormalizationResult={0}", e.PhraseResponse.Results[i].InverseTextNormalizationResult);
                Console.WriteLine("LexicalForm={0}", e.PhraseResponse.Results[i].LexicalForm);
                Console.WriteLine("MaskedInverseTextNormalizationResult={0}", e.PhraseResponse.Results[i].MaskedInverseTextNormalizationResult);
            }

            Console.WriteLine();
        }
        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            Console.WriteLine("{0}", e.PartialResult);
            Console.WriteLine();
        }
        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            Console.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            Console.WriteLine("Error text: {0}", e.SpeechErrorText);
            Console.WriteLine();
        }
        public void SendAudio(string wavFileName)
        {
            using (FileStream fileStream = new FileStream(wavFileName, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = 0;
                byte[] buffer = new byte[1024];

                try
                {
                    do
                    {
                        // Get more Audio data to send into byte buffer.
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                        // Send of audio data to service. 
                        serviceClient.SendAudio(buffer, bytesRead);
                    }
                    while (bytesRead > 0);
                }
                finally
                {
                    // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                    serviceClient.EndAudio();
                }
            }
        }



    }
}
