using System;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.Gui.Model
{
    public class ServiceManager
    {
        public bool IsInstalled { get; private set; } = false;

        public void InstallService(Action onServiceInstallCompleteCallback, Action<Exception> onServiceInstallFailedCallback)
        {
            Task doingServiceInstall = DoServiceInstall();
            doingServiceInstall.ContinueWith((t) => OnServiceInstallCompleteCallback(t, onServiceInstallCompleteCallback, onServiceInstallFailedCallback));
        }

        private Task DoServiceInstall()
        {
            return Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(1000);
                IsInstalled = true;
            });
        }

        private void OnServiceInstallCompleteCallback(Task task, Action onServiceInstallCompleteCallback, Action<Exception> onServiceInstallFailedCallback)
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