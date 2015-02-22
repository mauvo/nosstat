using System.Reflection;
using System.Configuration.Install;

namespace gotnet.biz.Utilities.WindowsServices
{
    /// <summary>
    /// http://www.codeproject.com/Articles/21405/Windows-Services-Can-Install-Themselves
    /// </summary>
    public static class SelfInstaller
    {
        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}