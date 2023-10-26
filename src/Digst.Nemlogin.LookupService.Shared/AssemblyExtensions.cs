using System.IO;
using System.Reflection;

namespace Digst.Nemlogin.LookupService.Shared
{
    public static class AssemblyExtensions
    {
        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            return assembly.CodeBase.GetAssemblyDirectory();
        }

        private static string GetAssemblyDirectory(this string codebase)
        {
            codebase = codebase.Replace("file:///", string.Empty);
            if (File.Exists(codebase)) codebase = new FileInfo(codebase).Directory.FullName;
            if (!Directory.Exists(codebase)) throw new DirectoryNotFoundException($"{codebase} does not exist");
            return codebase;
        }
    }
}