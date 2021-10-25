#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_export : EditorWindow
    {
        private string packageNameText = "";
        private bool readmeToggle = true;
        private bool assemblyToggle = true;
        private bool packageJsonToggle = true;

        private void OnGUI()
        {
            minSize = new Vector2(400, 100);

            GUILayout.Label("Unity Package Manager", EditorStyles.largeLabel);

            GUILayout.BeginHorizontal();
            GUILayout.Label("package name : ", GUILayout.Width(150));
            packageNameText = EditorGUILayout.TextField(packageNameText);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            packageJsonToggle = GUILayout.Toggle(packageJsonToggle, "/package.json");
            readmeToggle = GUILayout.Toggle(readmeToggle, "/README.md");
            assemblyToggle = GUILayout.Toggle(assemblyToggle, $"/{packageNameText }.asmdef");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create package", GUILayout.Width(100))) createPackageDirectory(packageNameText);
            GUILayout.EndHorizontal();
        }

        private void createPackageDirectory(string name)
        {
            var path = Application.dataPath + "/" + name;
            Directory.CreateDirectory(path);
            if (readmeToggle)
            {
                File.WriteAllText(path + "/README.md", $"# {name}");
            }
            if (assemblyToggle)
            {
                var assembly = JsonUtility.ToJson(new AssemblyDefinitionJson(name), true);
                File.WriteAllText(path + $"/{name}.asmdef", assembly);
            }
            if (packageJsonToggle)
            {
                var data = JsonUtility.ToJson(new PackageJson(name), true);
                File.WriteAllText(path + "/package.json", data);
            }
            AssetDatabase.Refresh();
        }
    }

    [Serializable]
    public class PackageJson
    {
        public string name = "";
        public string version = "";
        public string displayName = "";
        public string description = "";
        public string unity = "";
        public PackageJson(string name)
        {
            this.name = name;
            version = "1.0.0";
            displayName = name;
            description = name;
            var unityVersion = Application.unityVersion.Split('.');
            unity = $"{unityVersion[0]}.{unityVersion[1]}";
        }
    }

    [Serializable]
    public class AssemblyDefinitionJson
    {
        public string name;
        public string rootNamespace = "";
        public string[] references = new string[0];
        public string[] includePlatforms = new string[0];
        public string[] excludePlatforms = new string[0];
        public bool allowUnsafeCode = false;
        public bool overrideReferences = false;
        public string[] precompiledReferences = new string[0];
        public bool autoReferenced = true;
        public string[] defineConstraints = new string[0];
        public string[] versionDefines = new string[0];
        public bool noEngineReferences = false;
        public AssemblyDefinitionJson(string name)
        {
            this.name = name;
        }
    }
}
#endif
