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
        private readonly string m_LogsFolder;
        private ServiceHost wcfServiceHost;
        private INosStatServiceCallbacks guiCallbacks;
        private SpeechSynthesizer synthesizer;
        private FileSystemWatcher watch;

        public NosgothLogMonitoringService(string logsFolder)
        {
            m_LogsFolder = logsFolder;
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

            Speak("Running");
            
            watch = new FileSystemWatcher();
            watch.Path = m_LogsFolder; 
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
            var lines = File.ReadAllLines(e.FullPath);
            foreach (var line in lines)
            {
                //        for line in open(log_file):
                //if ' Beacon: PlayerName: ' in line:
                //current_player = line.split(' PlayerName: ')[1].split('\r\n')[0]
                //        players[current_player]['name'] = current_player
                //        elif ' Beacon: XpLevel: ' in line:
                //level = line.split(' XpLevel: ')[1].split('\r\n')[0]
                //        players[current_player]['level'] = level
                //        elif ' Beacon: MMR: ' in line:
                //mmr = line.split(' MMR: ')[1].split('\r\n')[0]
                //        players[current_player]['mmr'] = mmr
                //        elif ' Lobby: Team' in line:
                //current_team = line.split(' Lobby: ')[1].split(' ')[0]
                //        teams[current_team] = []
                //        elif ' Lobby: Idx:' in line:
                //name = line.split('.PlayerName:\'')[1].split("'")[0]
                //        teams[current_team].append(players[name])
                //elif ' Log: --- LOADING MOVIE START ---' in line:
                //print_teams(teams)
                //say_teams(speech, teams)
            }
        }

        protected override void OnStop()
        {
            wcfServiceHost.Close();
        }
    }
}
