using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.Gui.Model
{
    public class ServiceManager
    {
        public bool IsInstalled { get { return service.IsInstalled; } }

        private Service service;

        public ServiceManager()
        {
            service = new Service();
        }

        public void InstallService(Action onServiceInstallCompleteCallback, Action<Exception> onServiceInstallFailedCallback)
        {
            Task doingServiceInstall = DoServiceInstall();
            doingServiceInstall.ContinueWith((t) => OnServiceOperationCompleteCallback(t, onServiceInstallCompleteCallback, onServiceInstallFailedCallback));
        }

        public void UninstallService(Action onServiceOperationComplete, Action<Exception> onServiceOperationError)
        {
            Task doingServiceUninstall = DoServiceUninstall();
            doingServiceUninstall.ContinueWith((t) => OnServiceOperationCompleteCallback(t, onServiceOperationComplete, onServiceOperationError));
        }

        private Task DoServiceInstall()
        {
            return Task.Factory.StartNew(() =>
            {
                service.Install();
            });
        }

        private Task DoServiceUninstall()
        {
            return Task.Factory.StartNew(() =>
            {
                service.Uninstall();
            });
        }

        private void OnServiceOperationCompleteCallback(Task task, Action onServiceInstallCompleteCallback, Action<Exception> onServiceInstallFailedCallback)
        {
            try
            {
                task.Wait();
                Task.Factory.StartNew(onServiceInstallCompleteCallback);
            }
            catch (Exception e)
            {
                Task.Factory.StartNew(() => onServiceInstallFailedCallback(e));
            }
        }
    }
}