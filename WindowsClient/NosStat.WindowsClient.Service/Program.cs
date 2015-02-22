using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace NosStat.WindowsClient.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // http://www.codeproject.com/Articles/21405/Windows-Services-Can-Install-Themselves
            if (args != null && args.Length > 0)
            {
                if (args.Length > 1 && args[1].Length > 1)
                {
                    Directory.CreateDirectory(GetConfigFolder());
                    File.WriteAllText(GetConfigFilePath(), args[1]);
                }
                if (args[0].Length > 1 && (args[0][0] == '-' || args[0][0] == '/'))
                {
                    switch (args[0].Substring(1).ToLower())
                    {
                        default:
                            break;
                        case "install":
                        case "i":
                            SelfInstaller.InstallMe("NosgothLogMonitoringService");
                            break;
                        case "uninstall":
                        case "u":
                            SelfInstaller.UninstallMe();
                            break;
                    }
                }
            }
            else
            {
                ServiceBase.Run(new NosgothLogMonitoringService(ReadConfigFile()));
            }
        }

        private static string ReadConfigFile()
        {
            var configPath = GetConfigFilePath();
            if (!File.Exists(configPath))
                return "Could not find config file, " + configPath;
            try
            {
                return File.ReadAllText(configPath);
            }
            catch (Exception)
            {
                return "Could not read config file";
            }
        }

        private static string GetConfigFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "NosStat");
        }
        
        private static string GetConfigFilePath()
        {
            return Path.Combine(GetConfigFolder(), "nostat.config");
        }
    }
}
