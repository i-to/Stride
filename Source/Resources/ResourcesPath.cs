using System;
using System.IO;

namespace Stride.Resources
{
    /// <summary>
    /// Creates paths to access resources.
    /// </summary>
    public class ResourcesPath
    {
        public ResourcesPath()
        {
            var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            DirectoryPath = Path.GetDirectoryName(assemblyPath);
        }

        public ResourcesPath(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public string DirectoryPath { get; }
        public string GetResourcePath(string localPath) => $"{DirectoryPath}/{localPath}";
        public Uri GetResourceUri(string localPath) => new Uri(GetResourcePath(localPath));
    }
}
