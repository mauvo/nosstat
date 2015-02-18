using System;
using System.Configuration.Install;
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
            ManagedInstallerClass.InstallHelper(new string[] {NosStatServiceExePath});
            RefreshServiceController();
        }

        
    }
}