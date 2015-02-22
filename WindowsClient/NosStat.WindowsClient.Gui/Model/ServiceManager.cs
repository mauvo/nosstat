using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Gui.Model
{
    public class ServiceManager
    {
        private readonly Action<string> m_LogMessage;
        public bool IsInstalled { get { return service.IsInstalled; } }

        private Service service;
        private WcfClient client;

        public ServiceManager(Action<string> LogMessage)
        {
            m_LogMessage = LogMessage;
            service = new Service();

            client = new WcfClient();
            client.OnConnectionChanged += OnServiceConnectionChanged;
            client.OnMessageFromService += OnMessageFromService;
        }

        private void OnServiceConnectionChanged(object sender, bool isconnected)
        {
            if (isconnected)
                m_LogMessage("Connected to Windows Service.");
            else
                m_LogMessage("Not connected to Windows Service.");
        }

        private void OnMessageFromService(object sender, string message)
        {
            m_LogMessage(message);
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