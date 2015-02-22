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
        private static List<INosStatServiceCallbacks> allCallbacks = new List<INosStatServiceCallbacks>();
        public static IEnumerable<INosStatServiceCallbacks> AllCallbacks;

        private INosStatServiceCallbacks callback;

        public GuiCommunicationService()
        {
            callback = OperationContext.Current.GetCallbackChannel<INosStatServiceCallbacks>();
            allCallbacks.Add(callback);
        }

        ~GuiCommunicationService()
        {
            allCallbacks.Remove(callback);
        }

        public void Ping()
        {
            
        }
    }
}
