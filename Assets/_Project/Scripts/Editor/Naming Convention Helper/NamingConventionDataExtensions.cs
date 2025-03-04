using System.Linq;

namespace Asset_Naming_Convention_Helper
{
    public static class NamingConventionDataExtensions
    {
        public static bool IsExtensionMatch(this NamingConventionData namingConventionData, string extension) =>
            namingConventionData.Extensions.Any(x => x == extension);

        public static bool TryRename(this NamingConventionData namingConventionData, string assetName,
            out string result)
        {
            result = namingConventionData.Prefixes[0] + assetName;
            return !namingConventionData.Prefixes.Any(assetName.StartsWith);
        }
    }
}