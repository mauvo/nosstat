using System.IO;
using System.Reflection;

namespace NosStat.WindowsClient.Gui.Model.Extensions
{
    public static class AssemblyExtensions
    {
        public static string DirectoryName(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location);
        }
    }
}
