using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Gui.Model
{
    class WcfClient : INosStatServiceCallbacks
    {
        private InstanceContext InstanceContext;
        private ChannelFactory<INosStatService> pipeFactory;
        private INosStatService nosStatService;
        private Timer pingTimer;
        private bool isConnected = false;

        public bool IsConnected { get {return isConnected;} }

        public event OnConnectionChangedHandler OnConnectionChanged;
        internal delegate void OnConnectionChangedHandler(object sender, bool isConnected);

        public event OnMessageFromServiceHandler OnMessageFromService;
        internal delegate void OnMessageFromServiceHandler(object sender, string message);

        public WcfClient()
        {
            InstanceContext context = new InstanceContext(this);
            pipeFactory = new DuplexChannelFactory<INosStatService>(context, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/NosStatService"));
            nosStatService = pipeFactory.CreateChannel();
            pingTimer = new Timer();
            pingTimer.Elapsed += OnPingTimeout;
            pingTimer.Interval = 2000;
            pingTimer.Enabled = true;
        }

        private void OnPingTimeout(object sender, ElapsedEventArgs e)
        {
            try
            {
                nosStatService.Ping();
                UpdateConnected(true);
            }
            catch (Exception)
            {
                nosStatService = pipeFactory.CreateChannel();
                UpdateConnected(false);
            }
        }

        private void UpdateConnected(bool isConnectedNow)
        {
            isConnected = isConnectedNow;
            var handler = OnConnectionChanged;
            if (handler != null)
                OnConnectionChanged(this, isConnectedNow);
        }

        public void LogMessage(string message)
        {
            var handler = OnMessageFromService;
            if (handler != null)
                handler(this, message);
        }
    }

    
}
