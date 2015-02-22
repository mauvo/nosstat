using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.ServiceInterfaces
{
    [ServiceContract]
    public interface INosStatService
    {
        [OperationContract]
        bool RegisterForLogEvents(Action<string> logCallback);
    }
}
