#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_import : EditorWindow
    {
        private string gitURL = "";
        private string pathURL = "";
        private string versionURL = "";
        private string upmURL = "";

        private Vector2 scrollPoint = Vector2.zero;
        private bool refreshFlag = false;
        private float refreshTime = 0.0f;

        private AddRequest addRequest;
        private RemoveRequest removeReauest;
        private ListRequest listRequest;
        private PackageCollection packages = null;

        private void OnGUI()
        {
            minSize = new Vector2(600, 300);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Unity Package Manager", EditorStyles.largeLabel);
            if (GUILayout.Button("Open manifest.json", GUILayout.Width(200))) EditorUtility.OpenWithDefaultApp(Application.dataPath + "/../Packages/manifest.json");
            if (GUILayout.Button("Open packages-lock.json", GUILayout.Width(200))) EditorUtility.OpenWithDefaultApp(Application.dataPath + "/../Packages/packages-lock.json");
            GUILayout.EndHorizontal();

            GUILayout.Label("Get Unity Package Manager URL", EditorStyles.largeLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("git clone URL (https or ssh) : ", GUILayout.Width(200));
            gitURL = GUILayout.TextField(gitURL);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("package.json file path : ", GUILayout.Width(200));
            pathURL = GUILayout.TextField(pathURL);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("branch, tag or commit name : ", GUILayout.Width(200));
            versionURL = GUILayout.TextField(versionURL);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Unity Package Manager URL : ", GUILayout.Width(200));
            upmURL = GUILayout.TextArea(UPMPathConverter.ToUPMPath(gitURL, pathURL, versionURL));
            if (GUILayout.Button("Add", GUILayout.Width(100))) AddManifest(upmURL);
            GUILayout.EndHorizontal();

            GUILayout.Space(14);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Control Unity Package Manager", EditorStyles.largeLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh", GUILayout.Width(100))) ListManifest();
            GUILayout.EndHorizontal();
            scrollPoint = GUILayout.BeginScrollView(scrollPoint);
            if (packages != null) foreach (var item in packages)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(item.name, EditorStyles.label, GUILayout.Width(300));
                    GUILayout.Label(item.version, EditorStyles.label, GUILayout.Width(50));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.Width(100))) RemoveManifest(item);
                    if (GUILayout.Button("Reload", GUILayout.Width(100))) PackageLock.Reload(item.name);
                    GUILayout.EndHorizontal();
                }
            GUILayout.EndScrollView();

            if (refreshFlag)
            {
                refreshTime += Time.deltaTime;
                if (refreshTime > 5.0f)
                {
                    refreshFlag = false;
                    refreshTime = 0.0f;
                    ListManifest();
                }
            }
        }

        private void Awake() => ListManifest();

        private void OnInspectorUpdate() => Repaint();

        private void AddManifest(string identifier)
        {
            Debug.Log("Install: " + identifier);
            addRequest = Client.Add(identifier);
            EditorApplication.update += AddProgress;
        }

        private void AddProgress()
        {
            if (!addRequest.IsCompleted) return;
            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Installed: " + addRequest.Result.name);
                refreshFlag = true;
            }
            else if (addRequest.Status >= StatusCode.Failure) Debug.LogError(addRequest.Error.message);
            EditorApplication.update -= AddProgress;
        }

        private void RemoveManifest(PackageInfo package)
        {
            Debug.Log("Remove: " + package.name);
            removeReauest = Client.Remove(package.name);
            EditorApplication.update += RemoveProgress;
        }

        private void RemoveProgress()
        {
            if (!removeReauest.IsCompleted) return;
            if (removeReauest.Status == StatusCode.Success)
            {
                Debug.Log("Removed: " + removeReauest.PackageIdOrName);
                refreshFlag = true;
            }
            else if (removeReauest.Status >= StatusCode.Failure) Debug.LogError(removeReauest.Error.message);
            EditorApplication.update -= AddProgress;
        }

        private void ListManifest()
        {
            Debug.Log("Refresh List");
            listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        private void ListProgress()
        {
            if (!listRequest.IsCompleted) return;
            if (listRequest.Status == StatusCode.Success)
            {
                packages = listRequest.Result;
                foreach (var package in packages) Debug.Log("Package name: " + package.name);
            }
            else if (listRequest.Status >= StatusCode.Failure) Debug.LogError(listRequest.Error.message);
            EditorApplication.update -= ListProgress;
        }
    }

    public static class UPMPathConverter
    {
        public static string ToUPMPath(string sourceURL, string pathURL = "", string versionURL = "")
        {
            var source = sourceURL;
            if (!sourceURL.Contains("git+https://") && !sourceURL.Contains("git+ssh://"))
            {
                if (sourceURL.Contains("git@github.com:") || sourceURL.Contains("vs-ssh.visualstudio.com:"))
                {
                    var index = sourceURL.IndexOf(':');
                    var sourceBuilder = new StringBuilder(sourceURL);
                    sourceBuilder.Replace(':', '/', index, 1);
                    source = $"ssh://{sourceBuilder}";
                }
                source = $"git+{source}";
            }
            var path = (pathURL == "") ? "" : $"?path={pathURL}";
            var version = (versionURL == "") ? "" : $"#{versionURL}";
            return $"{source}{path}{version}";
        }
    }

    public static class PackageLock
    {
        public static void Reload(string name)
        {
            var filePath = Application.dataPath + "/../Packages/packages-lock.json";
            if (File.Exists(filePath))
            {
                var text = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<PackgeLock>(text);
                var target = data.dependencies.Where(item => item.Key == name)?.FirstOrDefault();
                if (target != null) data.dependencies.Remove(target.Value.Key);
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Client.Resolve();
            }
        }

        private class PackgeLock
        {
            public Dictionary<string, PackgeData> dependencies { get; set; }
        }

        private class PackgeData
        {
            public string version { get; set; }
            public int depth { get; set; }
            public string source { get; set; }
            public Dictionary<string, string> dependencies { get; set; }
            public string hash { get; set; }
            public string url { get; set; }
        }
    }
}
#endif
