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

            inputText = EditorGUILayout.TextField(inputText);

            if (GUILayout.Button("Init package"))
            {
                var path = Application.dataPath + "/"+ inputText;
                Directory.CreateDirectory(path);
                File.WriteAllText(path + "/README.md", "# "+ inputText);
                File.WriteAllText(path + "/LICENSE", "");
                var package = new PackageJson();
                package.name = inputText;
                package.version = "1.0.0";
                package.displayName = inputText;
                package.description = inputText;
                package.unity = "2020.3";
                var data=JsonUtility.ToJson(package, true);
                File.WriteAllText(path + "/package.json", data);

                AssetDatabase.Refresh();
            }
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
    }
}
#endif
