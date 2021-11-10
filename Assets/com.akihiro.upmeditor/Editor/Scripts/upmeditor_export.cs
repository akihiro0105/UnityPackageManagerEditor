#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
        private bool sampleToggle = true;
        private string errorLogText = "";

        private void OnGUI()
        {
            minSize = new Vector2(450, 150);

            GUILayout.Label("Unity Package Manager", EditorStyles.largeLabel);
            GUILayout.Label("50文字以下，小文字，数字，ハイフン (-)，アンダースコア (_)，ピリオド (.) のみ使用可能");

            GUILayout.BeginHorizontal();
            GUILayout.Label("package name : ", GUILayout.Width(150));
            packageNameText = EditorGUILayout.TextField(packageNameText);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            packageJsonToggle = GUILayout.Toggle(packageJsonToggle, "/package.json");
            readmeToggle = GUILayout.Toggle(readmeToggle, "/README.md");
            assemblyToggle = GUILayout.Toggle(assemblyToggle, $"/{packageNameText }.asmdef");
            sampleToggle = GUILayout.Toggle(sampleToggle, $"/Sample");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create package", GUILayout.Width(100))) createPackageDirectory(packageNameText);
            GUILayout.EndHorizontal();

            var redText = new GUIStyle();
            redText.normal.textColor = Color.red;
            GUILayout.Label(errorLogText, redText);
        }

        private void createPackageDirectory(string name)
        {
            errorLogText = "";
            if (name == "")
            {
                errorLogText = "パッケージ名を入力してください";
                return;
            }
            if (!name.Equals(Regex.Replace(name, @"[^a-z0-9_.\-]", "")))
            {
                errorLogText = "パッケージ名には小文字，数字，ハイフン (-)，アンダースコア (_)，ピリオド (.) のみ使用可能です";
                return;
            }
            if (name.Length > 50)
            {
                errorLogText = "パッケージ名は50文字以下にしてください";
                return;
            }
            var path = Application.dataPath + "/" + name;
            Directory.CreateDirectory(path);
            if (sampleToggle)
            {
                Directory.CreateDirectory(path + "/Samples");
            }
            if (readmeToggle)
            {
                var doc = path + "/Documentation";
                Directory.CreateDirectory(doc);
                File.WriteAllText(doc + "/README.md", $"# {name}");
            }
            if (assemblyToggle)
            {
                File.WriteAllText(path + $"/{name}.asmdef", JsonUtility.ToJson(new AssemblyDefinitionJson(name), true));
            }
            if (packageJsonToggle)
            {
                File.WriteAllText(path + "/package.json", JsonUtility.ToJson(new PackageJson(name, sampleToggle), true));
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
        public Sample[] samples = new Sample[0];
        public PackageJson(string name, bool isSample)
        {
            this.name = name;
            version = "1.0.0";
            displayName = name;
            description = name;
            var unityVersion = Application.unityVersion.Split('.');
            unity = $"{unityVersion[0]}.{unityVersion[1]}";
            if (isSample) samples = new Sample[] { new Sample("Samples") };
        }
    }

    [Serializable]
    public class Sample
    {
        public string displayName = "Sample";
        public string description = "Sample";
        public string path = "";
        public Sample(string path) => this.path = path;
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
        public AssemblyDefinitionJson(string name) => this.name = name;
    }
}
#endif
