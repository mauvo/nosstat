using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Service
{
    public partial class NosgothLogMonitoringService : ServiceBase
    {
        private ServiceHost wcfServiceHost;
        private INosStatServiceCallbacks guiCallbacks;

        public NosgothLogMonitoringService()
        {
            ServiceName = "NosgothLogMonitoringService";
        }

        protected override void OnStart(string[] args)
        {
            wcfServiceHost = new ServiceHost(typeof (GuiCommunicationService),new Uri[] {new Uri("net.pipe://localhost")});
            wcfServiceHost.AddServiceEndpoint(typeof(INosStatService), new NetNamedPipeBinding(), "NosStatService");
            wcfServiceHost.Open();
            guiCallbacks = new CallbackAggregator(GuiCommunicationService.AllCallbacks);
        }

        protected override void OnStop()
        {
            wcfServiceHost.Close();
        }
    }
}
