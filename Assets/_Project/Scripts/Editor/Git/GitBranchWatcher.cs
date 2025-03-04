using System.IO;
using UnityEditor;
using UnityEngine;

public class GitBranchWatcher
{
    private static bool _haveToRefresh;

    [InitializeOnLoadMethod]
    public static void CheckBranchAndRefresh()
    {
        var gitPath = Application.dataPath.Replace(@"/Assets", @"/.git");

        CreateWatcher(gitPath, "HEAD");
        CreateWatcher(Path.Combine(gitPath, "refs"), null);

        EditorApplication.update += () =>
        {
            if (EditorApplication.isFocused || !_haveToRefresh)
                return;

            _haveToRefresh = false;
            AssetDatabase.Refresh();
        };
    }


    private static void CreateWatcher(string path, string watcherFilter)
    {
        var watcher = new FileSystemWatcher(path);

        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        watcher.Changed += OnChanged;

        watcher.Filter = watcherFilter;
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;

        _haveToRefresh = true;
    }
}