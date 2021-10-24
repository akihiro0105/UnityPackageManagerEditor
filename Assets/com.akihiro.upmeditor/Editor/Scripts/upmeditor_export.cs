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
        private string inputText = "";

        private void OnGUI()
        {
            GUILayout.Label("UPM Export", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical();


            inputText = EditorGUILayout.TextField(inputText);

            if (GUILayout.Button("Init package"))
            {
                var path = Application.dataPath + "/" + inputText;
                Directory.CreateDirectory(path);
                File.WriteAllText(path + "/README.md", $"# {inputText}");
                var data = JsonUtility.ToJson(new PackageJson(inputText), true);
                File.WriteAllText(path + "/package.json", data);

                AssetDatabase.Refresh();
            }

            EditorGUILayout.EndVertical();
        }
    }

    [Serializable]
    public class PackageJson
    {
        public string name;
        public string version;
        public string displayName;
        public string description;
        public string unity;
        public PackageJson(string name)
        {
            this.name = name;
            version = "0.0.1";
            displayName = name;
            description = name;
            unity = "2020.3";
        }
    }
}
#endif
