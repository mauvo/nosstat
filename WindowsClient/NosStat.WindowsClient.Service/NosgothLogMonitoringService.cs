using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Service
{
    public partial class NosgothLogMonitoringService : ServiceBase
    {
        private ServiceHost wcfServiceHost;
        private INosStatServiceCallbacks guiCallbacks;
        private SpeechSynthesizer synthesizer;

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

            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10
            synthesizer.SpeakAsync("Test speech");
        }

        protected override void OnStop()
        {
            wcfServiceHost.Close();
        }
    }
}
