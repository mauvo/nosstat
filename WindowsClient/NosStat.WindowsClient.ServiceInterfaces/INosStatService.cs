using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.ServiceInterfaces
{
    [ServiceContract(CallbackContract = typeof(INosStatServiceCallbacks))]
    public interface INosStatService
    {
        [OperationContract]
        void Ping();
    }
}
