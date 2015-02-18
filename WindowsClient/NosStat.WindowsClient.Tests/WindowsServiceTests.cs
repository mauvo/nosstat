using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NosStat.WindowsClient.Tests
{
    [TestFixture]
    public class WindowsServiceTests
    {
        [Test]
        public void IsServiceInstalled()
        {
            var serviceName = "ThisDoesNotExist";
            var service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
            var isInstalled = service != null;

            Assert.IsFalse(isInstalled);
        }
    }
}
