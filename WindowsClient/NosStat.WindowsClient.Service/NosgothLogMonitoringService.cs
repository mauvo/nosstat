using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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
        private FileSystemWatcher watch;

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
            Speak("Test speech");

            watch = new FileSystemWatcher();
            watch.Path = @"C:\Users\david_000\Documents\my games\Nosgoth\BCMPGame\Logs";
            watch.Filter = "Launch.log";
            watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite; //more options
            watch.Changed += new FileSystemEventHandler(OnChanged);
            watch.EnableRaisingEvents = true;
        }

        private void Speak(string message)
        {
            synthesizer.SpeakAsync(message);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Speak("Log changed");
        }

        protected override void OnStop()
        {
            wcfServiceHost.Close();
        }
    }
}
