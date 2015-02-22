﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.ServiceInterfaces
{
    public interface INosStatServiceCallbacks
    {
        [OperationContract]
        void LogMessage(string message);
    }
}
