using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Gui.Model
{
    public class ServiceManager : INosStatServiceCallbacks
    {
        private readonly Action<string> m_LogMessage;
        public bool IsInstalled { get { return service.IsInstalled; } }

        private Service service;

        public ServiceManager(Action<string> LogMessage)
        {
            m_LogMessage = LogMessage;
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

        public void ExecuteConnectToService()
        {
            InstanceContext context = new InstanceContext(this);
            ChannelFactory<INosStatService> pipeFactory = new DuplexChannelFactory<INosStatService>(context, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/NosStatService"));
            INosStatService nosStatService = pipeFactory.CreateChannel();
            var result = nosStatService.RegisterForLogEvents("Test");
        }

        public void LogMessage(string message)
        {
            m_LogMessage(message);
        }
    }
}