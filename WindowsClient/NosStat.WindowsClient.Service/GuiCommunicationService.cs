using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class GuiCommunicationService : INosStatService
    {
        public string RegisterForLogEvents(string message)
        {
            var callback = OperationContext.Current.GetCallbackChannel<INosStatServiceCallbacks>();
            Task.Factory.StartNew(() => callback.LogMessage("Test log message"));
            return message;
        }
    }
}
