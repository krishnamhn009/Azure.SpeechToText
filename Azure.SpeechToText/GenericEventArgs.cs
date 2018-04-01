using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.SpeechToText
{
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEventArgs{T}" /> class.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public GenericEventArgs(T eventData)
        {
            this.EventData = eventData;
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        public T EventData { get; private set; }
    }
}