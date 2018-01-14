using System;
using System.IO;

namespace Stride.Resources
{
    /// <summary>
    /// Provides access to content resources copied to output directory.
    /// </summary>
    public class CopiedResourcesPath
    {
        public CopiedResourcesPath()
        {
            var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            DirectoryPath = Path.GetDirectoryName(assemblyPath);
        }

        public string DirectoryPath { get; }
        public string GetResourcePath(string localPath) => $"{DirectoryPath}/{localPath}";
        public Uri GetResourceUri(string localPath) => new Uri(GetResourcePath(localPath));
    }
}
