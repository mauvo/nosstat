using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using NosStat.WindowsClient.Gui.Annotations;
using NosStat.WindowsClient.Gui.Model;

namespace NosStat.WindowsClient.Gui.ViewModel
{
    public class ServiceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ServiceManager serviceManager;

        public ServiceViewModel()
        {
            serviceManager = new ServiceManager();
        }

        private ICommand m_ConnectToService;
        public ICommand ConnectToService
        {
            get
            {
                if (m_ConnectToService == null)
                    m_ConnectToService = CommandFactory.CreateCommand(ExecuteConnectToService);
                return m_ConnectToService;
            }
        }

        private void ExecuteConnectToService()
        {
            serviceManager.ExecuteConnectToService();
        }

        private ICommand m_UninstallService;
        public ICommand UninstallService
        {
            get
            {
                if (m_UninstallService == null)
                    m_UninstallService = CommandFactory.CreateCommand(ExecuteUninstallService);
                return m_UninstallService;
            }
        }

        private void ExecuteUninstallService()
        {
            serviceManager.UninstallService(OnServiceOperationComplete, OnServiceOperationError);
        }

        private ICommand m_InstallService;
        public ICommand InstallService
        {
            get
            {
                if (m_InstallService == null)
                    m_InstallService = CommandFactory.CreateCommand(ExecuteInstallService);
                return m_InstallService;
            }
        }

        private void ExecuteInstallService()
        {
            serviceManager.InstallService(OnServiceOperationComplete, OnServiceOperationError);
        }

        private void OnServiceOperationError(Exception exception)
        {
            if (exception is AggregateException)
            {
                var aggException = (AggregateException) exception;
                var stringBuilder = new StringBuilder();
                foreach (var innerException in aggException.InnerExceptions)
                {
                    stringBuilder.AppendLine(innerException.Message);
                }
                ErrorMessage = stringBuilder.ToString();
            }
            else
            {
                ErrorMessage = exception.Message;
            }
            OnPropertyChanged("StatusMessage");
        }

        private void OnServiceOperationComplete()
        {
            OnPropertyChanged("StatusMessage");
        }

        public string StatusMessage
        {
            get
            {
                if (serviceManager.IsInstalled)
                    return "Service is installed.";
                return "Service is not installed.";
            }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}