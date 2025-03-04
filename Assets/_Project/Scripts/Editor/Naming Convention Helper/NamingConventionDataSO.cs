namespace Asset_Naming_Convention_Helper
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NamingConventions", menuName = "Naming Convention Helper")]
    public class NamingConventionDataSO : ScriptableObject
    {
        public string TargetFolder = "";
        public NamingConventionData[] NamingConventionDatas;
        private void OnValidate() => NamingConventionHelper.Initialize(this);

        public void Deconstruct(out string targetFolder, out NamingConventionData[] namingConventionDatas) =>
            (targetFolder, namingConventionDatas) = (TargetFolder, NamingConventionDatas);
    }

    [Serializable]
    public class NamingConventionData
    {
        [field: SerializeField] public string[] Extensions { get; private set; }
        [field: SerializeField] public string[] Prefixes { get; private set; }
    }
}