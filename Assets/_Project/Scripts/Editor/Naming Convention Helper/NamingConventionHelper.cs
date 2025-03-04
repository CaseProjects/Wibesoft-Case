using UnityEditor;
using UnityEngine;

#pragma warning disable CS4014

namespace Asset_Naming_Convention_Helper
{
    public static class NamingConventionHelper
    {
        public static string TargetPath { get; private set; }
        public static NamingConventionData[] NamingConventions { get; private set; }

        public static void Initialize(NamingConventionDataSO namingConventionDataSo) =>
            (TargetPath, NamingConventions) = namingConventionDataSo;

        [InitializeOnLoadMethod]
        private static void OnRecompile()
        {
            if (TargetPath == null) Initialize(GetConventionDataSo());
        }

        private static NamingConventionDataSO GetConventionDataSo() =>
            Resources.Load<NamingConventionDataSO>("NamingConventions");
    }
}