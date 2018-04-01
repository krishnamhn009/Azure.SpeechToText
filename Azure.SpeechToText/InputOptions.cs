using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.SpeechToText
{
  public  class InputOptions
    {
        public enum Gender
        {
            Female,
            Male
        }

        public enum AudioOutputFormat
        {
            /// <summary>
            /// raw-8khz-8bit-mono-mulaw request output audio format type.
            /// </summary>
            Raw8Khz8BitMonoMULaw,

            /// <summary>
            /// raw-16khz-16bit-mono-pcm request output audio format type.
            /// </summary>
            Raw16Khz16BitMonoPcm,

            /// <summary>
            /// riff-8khz-8bit-mono-mulaw request output audio format type.
            /// </summary>
            Riff8Khz8BitMonoMULaw,

            /// <summary>
            /// riff-16khz-16bit-mono-pcm request output audio format type.
            /// </summary>
            Riff16Khz16BitMonoPcm,

            // <summary>
            /// ssml-16khz-16bit-mono-silk request output audio format type.
            /// It is a SSML with audio segment, with audio compressed by SILK codec
            /// </summary>
            Ssml16Khz16BitMonoSilk,

            /// <summary>
            /// raw-16khz-16bit-mono-truesilk request output audio format type.
            /// Audio compressed by SILK codec
            /// </summary>
            Raw16Khz16BitMonoTrueSilk,

            /// <summary>
            /// ssml-16khz-16bit-mono-tts request output audio format type.
            /// It is a SSML with audio segment, and it needs tts engine to play out
            /// </summary>
            Ssml16Khz16BitMonoTts,

            /// <summary>
            /// audio-16khz-128kbitrate-mono-mp3 request output audio format type.
            /// </summary>
            Audio16Khz128KBitRateMonoMp3,

            /// <summary>
            /// audio-16khz-64kbitrate-mono-mp3 request output audio format type.
            /// </summary>
            Audio16Khz64KBitRateMonoMp3,

            /// <summary>
            /// audio-16khz-32kbitrate-mono-mp3 request output audio format type.
            /// </summary>
            Audio16Khz32KBitRateMonoMp3,

            /// <summary>
            /// audio-16khz-16kbps-mono-siren request output audio format type.
            /// </summary>
            Audio16Khz16KbpsMonoSiren,

            /// <summary>
            /// riff-16khz-16kbps-mono-siren request output audio format type.
            /// </summary>
            Riff16Khz16KbpsMonoSiren,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        public InputOptions()
        {
            this.Locale = "en-us";
            this.VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
            this.OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm;
            this.SearchApp = "07D3234E49CE426DAA29772419F436CA";
            this.SearchClientId = "1ECFAE91408841A480F00935DC390960";
        }


        public string SearchApp { get; set; }

        public string SearchClientId { get; set; }

        /// <summary>
        /// Gets or sets the request URI.
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// Gets or sets the audio output format.
        /// </summary>
        public AudioOutputFormat OutputFormat { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get
            {
                List<KeyValuePair<string, string>> toReturn = new List<KeyValuePair<string, string>>();
                toReturn.Add(new KeyValuePair<string, string>("Content-Type", "application/ssml+xml"));

                string outputFormat;

                switch (this.OutputFormat)
                {
                    case AudioOutputFormat.Raw16Khz16BitMonoPcm:
                        outputFormat = "raw-16khz-16bit-mono-pcm";
                        break;

                    case AudioOutputFormat.Raw8Khz8BitMonoMULaw:
                        outputFormat = "raw-8khz-8bit-mono-mulaw";
                        break;

                    case AudioOutputFormat.Riff16Khz16BitMonoPcm:
                        outputFormat = "riff-16khz-16bit-mono-pcm";
                        break;

                    case AudioOutputFormat.Riff8Khz8BitMonoMULaw:
                        outputFormat = "riff-8khz-8bit-mono-mulaw";
                        break;

                    case AudioOutputFormat.Ssml16Khz16BitMonoSilk:
                        outputFormat = "ssml-16khz-16bit-mono-silk";
                        break;

                    case AudioOutputFormat.Raw16Khz16BitMonoTrueSilk:
                        outputFormat = "raw-16khz-16bit-mono-truesilk";
                        break;

                    case AudioOutputFormat.Ssml16Khz16BitMonoTts:
                        outputFormat = "ssml-16khz-16bit-mono-tts";
                        break;

                    case AudioOutputFormat.Audio16Khz128KBitRateMonoMp3:
                        outputFormat = "audio-16khz-128kbitrate-mono-mp3";
                        break;

                    case AudioOutputFormat.Audio16Khz64KBitRateMonoMp3:
                        outputFormat = "audio-16khz-64kbitrate-mono-mp3";
                        break;

                    case AudioOutputFormat.Audio16Khz32KBitRateMonoMp3:
                        outputFormat = "audio-16khz-32kbitrate-mono-mp3";
                        break;

                    case AudioOutputFormat.Audio16Khz16KbpsMonoSiren:
                        outputFormat = "audio-16khz-16kbps-mono-siren";
                        break;

                    case AudioOutputFormat.Riff16Khz16KbpsMonoSiren:
                        outputFormat = "riff-16khz-16kbps-mono-siren";
                        break;

                    default:
                        outputFormat = "riff-16khz-16bit-mono-pcm";
                        break;
                }

                toReturn.Add(new KeyValuePair<string, string>("X-Microsoft-OutputFormat", outputFormat));
                // authorization Header
                toReturn.Add(new KeyValuePair<string, string>("Authorization", this.AuthorizationToken));
                // Refer to the doc
                toReturn.Add(new KeyValuePair<string, string>("X-Search-AppId", this.SearchApp));
                // Refer to the doc
                toReturn.Add(new KeyValuePair<string, string>("X-Search-ClientID", this.SearchClientId));
                // The software originating the request
                toReturn.Add(new KeyValuePair<string, string>("User-Agent", "TTSClient"));

                return toReturn;
            }
            set
            {
                Headers = value;
            }
        }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        public String Locale { get; set; }

        /// <summary>
        /// Gets or sets the type of the voice; male/female.
        /// </summary>
        public Gender VoiceType { get; set; }

        /// <summary>
        /// Gets or sets the name of the voice.
        /// </summary>
        public string VoiceName { get; set; }

        /// <summary>
        /// Authorization Token.
        /// </summary>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}
