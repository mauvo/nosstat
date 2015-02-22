using System;
using System.Reflection;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace NosStat.WindowsClient.Service
{
    /// <summary>
    /// http://www.codeproject.com/Articles/21405/Windows-Services-Can-Install-Themselves
    /// </summary>
    public static class SelfInstaller
    {
        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe(string serviceName)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { _exePath });
                var serviceController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(5));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}