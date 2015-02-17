using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
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

        public string StatusMessage
        {
            get
            {
                if (serviceManager.IsInstalled)
                    return "Service is installed.";
                return "Service is not installed.";
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}