using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Service
{
    class CallbackAggregator : INosStatServiceCallbacks
    {
        private readonly IEnumerable<INosStatServiceCallbacks> m_Callbacks;

        public CallbackAggregator(IEnumerable<INosStatServiceCallbacks> callbacks)
        {
            m_Callbacks = callbacks;
        }

        public void LogMessage(string message)
        {
            foreach (var callback in m_Callbacks)
            {
                callback.LogMessage(message);
            }
        }
    }
}
