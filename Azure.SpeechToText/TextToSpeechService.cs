using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Azure.SpeechToText
{
  public  class TextToSpeechService
    {
        public string subscriptionKey { get; }

        public event EventHandler<GenericEventArgs<Stream>> OnAudioAvailable;

        public event EventHandler<GenericEventArgs<Exception>> OnError;

        private HttpClient client;

        private HttpClientHandler handler;

        public TextToSpeechService(string _subscriptionKey)
        {
            subscriptionKey = _subscriptionKey;
            var cookieContainer = new CookieContainer();
            handler = new HttpClientHandler() { CookieContainer = new CookieContainer(), UseProxy = false };
            client = new HttpClient(handler);
        }
      
        public Task Speak(CancellationToken cancellationToken, InputOptions inputOptions)
        {
            client.DefaultRequestHeaders.Clear();
            foreach (var header in inputOptions.Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, inputOptions.RequestUri)
            {
                Content = new StringContent(GenerateSsml(inputOptions.Locale, inputOptions.VoiceType.ToString(), inputOptions.VoiceName, inputOptions.Text))
            };

            var httpTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            Console.WriteLine("Response status code: [{0}]", httpTask.Result.StatusCode);

            var saveTask = httpTask.ContinueWith(
                async (responseMessage, token) =>
                {
                    try
                    {
                        if (responseMessage.IsCompleted && responseMessage.Result != null && responseMessage.Result.IsSuccessStatusCode)
                        {
                            var httpStream = await responseMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            this.AudioAvailable(new GenericEventArgs<Stream>(httpStream));
                        }
                        else
                        {
                            this.Error(new GenericEventArgs<Exception>(new Exception(String.Format("Service returned {0}", responseMessage.Result.StatusCode))));
                        }
                    }
                    catch (Exception e)
                    {
                        this.Error(new GenericEventArgs<Exception>(e.GetBaseException()));
                    }
                    finally
                    {
                        responseMessage.Dispose();
                        request.Dispose();
                    }
                },
                TaskContinuationOptions.AttachedToParent,
                cancellationToken);

            return saveTask;
        }

        private string GenerateSsml(string locale, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                  new XElement("voice",
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }

        private void AudioAvailable(GenericEventArgs<Stream> e)
        {
            this.OnAudioAvailable?.Invoke(this, e);
        }

        private void Error(GenericEventArgs<Exception> e)
        {
            this.OnError?.Invoke(this, e);
        }
    }
}
