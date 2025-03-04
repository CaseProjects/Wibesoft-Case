using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;

#pragma warning disable CS4014

namespace Asset_Naming_Convention_Helper
{
    public class AssetNamingPostprocessor : AssetPostprocessor
    {
        private static bool _isProcessing;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromPath)
        {
            RenameAssets(importedAssets);
            RenameAssets(movedAssets);
        }

        private static void RenameAssets(IEnumerable<string> paths)
        {
            if (_isProcessing)
                return;

            foreach (var path in paths)
            {
                if (!path.Contains(NamingConventionHelper.TargetPath))
                    break;

                var assetName = Path.GetFileNameWithoutExtension(path);
                var assetExtension = Path.GetExtension(path);

                var conventionData = NamingConventionHelper.NamingConventions.SingleOrDefault(x =>
                    x.IsExtensionMatch(assetExtension));

                if (conventionData != null)
                {
                    if (conventionData.TryRename(assetName, out var assetNameWithPrefix))
                        RenameAssetWithDelay(path, assetNameWithPrefix);
                    ExecuteProcess();
                }
            }
        }

        private static async Task RenameAssetWithDelay(string path, string assetName)
        {
            await Task.Delay(10);
            AssetDatabase.RenameAsset(path, assetName);
        }

        private static async Task ExecuteProcess()
        {
            _isProcessing = true;
            await Task.Delay(100);
            _isProcessing = false;
        }
    }
}