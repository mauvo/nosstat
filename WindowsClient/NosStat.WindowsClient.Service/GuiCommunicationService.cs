using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NosStat.WindowsClient.ServiceInterfaces;

namespace NosStat.WindowsClient.Service
{
    class GuiCommunicationService : INosStatService
    {
        public bool RegisterForLogEvents(Action<string> logCallback)
        {
            return true;
        }
    }
}
