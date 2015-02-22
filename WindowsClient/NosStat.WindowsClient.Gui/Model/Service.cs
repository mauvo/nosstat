using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using NosStat.WindowsClient.Gui.Model.Extensions;

namespace NosStat.WindowsClient.Gui.Model
{
    internal class Service
    {
        private const string NosStateServiceName = "NosgothLogMonitoringService";
        private const string NosStatExeName = "NosStat.WindowsClient.Service.exe";

        private ServiceController serviceController;

        public bool IsInstalled { get { return serviceController != null; } }

        public string NosStatServiceExePath
        {
            get { return Path.Combine(Assembly.GetExecutingAssembly().DirectoryName(), NosStatExeName); }
        }

        public Service()
        {
            RefreshServiceController();
        }

        private void RefreshServiceController()
        {
            serviceController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == NosStateServiceName);
        }

        public void Install()
        {
            if(IsInstalled)
                throw new Exception("Cannot install NosStat Service, it is already installed.");

            var info = new ProcessStartInfo(NosStatServiceExePath, "/install")
            {
                Verb = "runas", // indicates to elevate privileges
            };

            var process = new Process
            {
                EnableRaisingEvents = true, // enable WaitForExit()
                StartInfo = info
            };

            process.Start();
            process.WaitForExit(); // sleep calling process thread until evoked process exit

            RefreshServiceController();
        }

        public void Uninstall()
        {
            if (!IsInstalled)
                throw new Exception("Cannot uninstall NosStat Service, it is not installed.");

            var info = new ProcessStartInfo(NosStatServiceExePath, "/uninstall")
            {
                Verb = "runas", // indicates to elevate privileges
            };

            var process = new Process
            {
                EnableRaisingEvents = true, // enable WaitForExit()
                StartInfo = info
            };

            process.Start();
            process.WaitForExit(); // sleep calling process thread until evoked process exit

            RefreshServiceController();
        }


    }
}