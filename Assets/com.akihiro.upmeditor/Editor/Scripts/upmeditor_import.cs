#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_import : EditorWindow
    {
        private void OnGUI()
        {
            var manifest = Application.dataPath + "/../Packages/manifest.json";
            GUILayout.Label("UPM Import", EditorStyles.boldLabel);
            if (GUILayout.Button("Open manifest.json"))
            {
                EditorUtility.OpenWithDefaultApp(manifest);
            }

            if (GUILayout.Button("Reload"))
            {
                GetManifestJson(manifest);
            }

        }

        private void GetManifestJson(string manifest)
        {
            var json = File.ReadAllText(manifest);
            if (json != null)
            {
                var manifestJson = JsonConvert.DeserializeObject<ManifestJson>(json);
                var key = "com.akihiro.nanokontrol2.sdk";
                if (manifestJson.dependencies.ContainsKey(key))
                {
                    manifestJson.dependencies.Remove(key);
                }
                else
                {
                    manifestJson.dependencies.Add(key, "git+ssh://git@github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk");
                }
                var data = JsonConvert.SerializeObject(manifestJson, Formatting.Indented);
                File.WriteAllText(manifest, data);
            }
        }


    }

    #region Manifest.json
    [Serializable]
    public class ManifestJson
    {
        public List<Scopedregistry> scopedRegistries = new List<Scopedregistry>();
        public Dictionary<string, string> dependencies = new Dictionary<string, string>();
    }

    [Serializable]
    public class Scopedregistry
    {
        public string name;
        public string url;
        public List<string> scopes = new List<string>();
    }
    #endregion
}
#endif
