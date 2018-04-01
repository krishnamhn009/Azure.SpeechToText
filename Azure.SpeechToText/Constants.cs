

namespace Azure.SpeechToText
{
    public  class Constants
    {
        public const string Speech_API_Key = "<YOUR_KEY>";
        public const string AUDIO_DIRECTORY= @"..\..\audio\";
        public const string AUTHENTICAION_URI = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        public const string BASE_URI = "https://speech.platform.bing.com/";
        public const  string HOST = @"speech.platform.bing.com";
        public const string ContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";
    }
}
