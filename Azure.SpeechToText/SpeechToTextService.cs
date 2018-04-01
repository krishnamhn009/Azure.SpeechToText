using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.IO;
using System.Net;

namespace Azure.SpeechToText
{
    public class SpeechToTextService
    {
        public DataRecognitionClient serviceClient { get; }

        public string RequestURI { get; }
        /// <summary>
        /// using client library=>yet to implement
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="language"></param>
        /// <param name="subscriptionKey"></param>
        public SpeechToTextService(SpeechRecognitionMode mode, string language, string subscriptionKey)
        {
            serviceClient = SpeechRecognitionServiceFactory.CreateDataClient(mode, language, subscriptionKey);
            // Event handlers for speech recognition results
            serviceClient.OnResponseReceived += this.OnResponseReceivedHandler;
            serviceClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            serviceClient.OnConversationError += this.OnConversationErrorHandler;
        }

        public SpeechToTextService(string recoginattionMode, string language, string outputFormat)
        {
            //build request uri
            RequestURI = Constants.BASE_URI + "speech/recognition/" + recoginattionMode;
            RequestURI += "/cognitiveservices/v1?language=" + language + "&locale=" + language + "&format=" + outputFormat;

        }
        public void SendAudio(string wavFileName)
        {
            /* * Input your own audio file or use read from a microphone stream directly.*/         
            string responseString;
            FileStream fs = null;
            try
            {
                Authorization auth = new Authorization(Constants.Speech_API_Key);
                var token = auth.GetAuthorizationTokenAsync().Result;
                Console.WriteLine("Token: {0}\n", token);
                Console.WriteLine("Request Uri: " + RequestURI + Environment.NewLine);

                HttpWebRequest request = null;
                request = (HttpWebRequest)HttpWebRequest.Create(RequestURI);
                request.SendChunked = true;
                request.Accept = @"application/json;text/xml";
                request.Method = "POST";
                request.ProtocolVersion = HttpVersion.Version11;
                request.Host = Constants.HOST;
                request.ContentType = Constants.ContentType;
                request.Headers["Authorization"] = "Bearer " + token;
                using (fs = new FileStream(wavFileName, FileMode.Open, FileAccess.Read))
                {
                    /** Open a request stream and write 1024 byte chunks in the stream one at a time. */
                    byte[] buffer = null;
                    int bytesRead = 0;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        /** Read 1024 raw bytes from the input audio file.*/
                        buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                        { requestStream.Write(buffer, 0, bytesRead); }
                        // Flush
                        requestStream.Flush();
                    }
                    /** Get the response from the service.*/
                    Console.WriteLine("Response:");

                    using (WebResponse response = request.GetResponse())
                    {
                        Console.WriteLine(((HttpWebResponse)response).StatusCode);
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = sr.ReadToEnd();
                        }
                        Console.WriteLine(responseString);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
            }
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
      



    }
}
