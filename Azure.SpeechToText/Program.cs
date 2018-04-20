using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.IO;
using System.Media;
using System.Threading;
using static Azure.SpeechToText.InputOptions;

namespace Azure.SpeechToText
{
   public class Program
    {
        static void Main(string[] args)
        {
            MenuList();
        }

        private static void  MenuList()
        {
            Console.WriteLine("Please Enter Your Choice\n");
            Console.Write(" 1.Speech To Text \n 2.Text To Speech\n 3.Quit\n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Write("You have entered : " + input + "\nPlease wait...");
                    SpeechToTextRest();
                    break;
                case "2":
                    TextToSpeech();
                    break;
                default:
                    Console.Write("Invalid Input");
                    break;
            }


        }

       private static void SpeechToText()
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


        private static void SpeechToTextRest()
        {
            //Recognition mode:1.interactive 2.conversation 3.dictation
            //Language :en-US
            //Output format :1.Default 2. Detailed 3.Simple
            var srService = new SpeechToTextService("interactive","en-IN","Simple");
            srService.SendAudio( Constants.AUDIO_DIRECTORY + "speechToText12.wav");
        }

        private static void TextToSpeech()
        {
            try
            {
                // Create the service                
                Console.WriteLine("press y for continue or n ");
                string yn= Console.ReadLine();

                while (yn.ToLower()!="n")
                {
                    Console.WriteLine("Creating service...");
                    var srService = new TextToSpeechService(Constants.Speech_API_Key);
                    srService.OnAudioAvailable += PlayAudio;
                    srService.OnError += ErrorHandler;
                    Console.WriteLine("Done");
                    Console.WriteLine("");

                    Console.WriteLine("Generating the Authentication Token.");

                    Authorization auth = new Authorization(Constants.Speech_API_Key);
                    var authToken = auth.GetAuthorizationTokenAsync().Result;
                    //Console.WriteLine("Token: {0}\n", authToken);
                    Console.WriteLine("Authentication Done");
                    Console.WriteLine("");
                    Console.WriteLine("Enter Text to Speech\n");
                    string text = Console.ReadLine();

                    Console.WriteLine("Select voice font\n");
                    Console.Write(" 1.en-IN Priya \n 2.en-In Ravi \n 3.default :en-US ZiraUS\n");
                    string input = Console.ReadLine();
                    string voice, locale = "en-IN";
                    switch (input)
                    {
                        case "1":
                            voice = "Microsoft Server Speech Text to Speech Voice (en-IN, PriyaRUS)";
                            break;
                        case "2":
                            voice = "Microsoft Server Speech Text to Speech Voice (en-IN, Ravi, Apollo)";
                            break;
                        default:
                            voice = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
                            locale = "en-US";
                            break;
                    }

                    //voice output support
                    //https://docs.microsoft.com/en-us/azure/cognitive-services/Speech/API-Reference-REST/BingVoiceOutput#main

                    Console.WriteLine("Speaking");
                    var inputOptions = new InputOptions()
                    {
                        RequestUri = new Uri(Constants.BASE_URI + "synthesize"),
                        Text = text,
                        Locale = locale,
                        VoiceName = voice,
                        OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm,
                        AuthorizationToken = "Bearer " + authToken,
                    };
                    Console.WriteLine(inputOptions.Text);
                    srService.Speak(CancellationToken.None, inputOptions).Wait();
                    Console.WriteLine("Done");
                    Console.WriteLine("");
                    Console.WriteLine("want to continue\ny or n ");
                    yn = Console.ReadLine();
                }

              
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadKey();
        }


        private static void PlayAudio(object sender, GenericEventArgs<Stream> args)
        {
            // For SoundPlayer to be able to play the wav file, it has to be encoded in PCM.
            // Use output audio format AudioOutputFormat.Riff16Khz16BitMonoPcm to do that.
            SoundPlayer player = new SoundPlayer(args.EventData);
            player.PlaySync();
            args.EventData.Dispose();
        }
        private static void ErrorHandler(object sender, GenericEventArgs<Exception> e)
        {
            Console.WriteLine("Unable to complete the TTS request: [{0}]", e.ToString());
        }

    }
}
