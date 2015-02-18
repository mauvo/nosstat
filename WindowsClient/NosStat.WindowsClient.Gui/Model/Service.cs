using System.Linq;
using System.ServiceProcess;

namespace NosStat.WindowsClient.Gui.Model
{
    internal class Service
    {
        private const string NosStateServiceName = "";

        private ServiceController serviceController;

        public bool IsInstalled { get { return serviceController != null; } }
        
        public Service()
        {
            serviceController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == NosStateServiceName);
        }
    }
}