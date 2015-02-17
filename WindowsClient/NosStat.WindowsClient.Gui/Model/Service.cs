using System.ServiceProcess;

namespace NosStat.WindowsClient.Gui.Model
{
    internal class Service
    {
        private const string NosStateServiceName = "";

        private ServiceController serviceController;
        
        public Service()
        {
            serviceController = new ServiceController(NosStateServiceName);
        }
    }
}