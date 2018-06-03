using System.Windows.Media;

namespace Stride.Resources
{
    public class ResourceLoader
    {
        public readonly ResourcesPath ResourcesPath;

        public ResourceLoader(ResourcesPath resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }

        /// <summary>
        /// Provides access to the resources copied to output directory along with executing assembly.
        /// </summary>
        public static ResourceLoader OfCopiedResources()
            => new ResourceLoader(new ResourcesPath());

        public GlyphTypeface LoadTypeface(string resourcePath)
        {
            var uri = ResourcesPath.GetResourceUri(resourcePath);
            return new GlyphTypeface(uri);
        }
    }
}