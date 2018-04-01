using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Azure.SpeechToText
{
   public class Authorization
    {
        private readonly string subscriptionKey;
        public Authorization(string subscriptionKey)
        {
            if (subscriptionKey == null)
            {
                throw new ArgumentNullException(nameof(subscriptionKey));
            }
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentException(nameof(subscriptionKey));
            }
            this.subscriptionKey = subscriptionKey;
        }
        public Task<string> GetAuthorizationTokenAsync()
        {
            return FetchToken(Constants.AUTHENTICAION_URI, subscriptionKey);
        }
        private static async Task<string> FetchToken(string fetchUri, string subscriptionKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                using (var result = await client.PostAsync(Constants.AUTHENTICAION_URI, null).ConfigureAwait(false))
                {
                    return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

            }

        }
    }
}
