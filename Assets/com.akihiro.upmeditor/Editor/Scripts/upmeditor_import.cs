#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_import : EditorWindow
    {
        private AddRequest addRequest;
        private RemoveRequest removeReauest;
        private ListRequest listRequest;
        private PackageCollection packages;

        private void OnGUI()
        {
            GUILayout.Label("UPM Import", EditorStyles.boldLabel);

            if (GUILayout.Button("Open manifest.json")) EditorUtility.OpenWithDefaultApp(Application.dataPath + "/../Packages/manifest.json");

            if (GUILayout.Button("Add"))
            {
                AddManifest("git+ssh://git@github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk");
            }
            if (GUILayout.Button("Remove"))
            {
                RemoveManifest("com.akihiro.nanokontrol2.sdk");
            }
            if (GUILayout.Button("List"))
            {
                ListManifest();
            }
        }

        private void AddManifest(string identifier)
        {
            addRequest=Client.Add(identifier);
            EditorApplication.update += AddProgress;
        }

        private void AddProgress()
        {
            if (addRequest.IsCompleted)
            {
                if (addRequest.Status == StatusCode.Success)
                    Debug.Log("Installed: " + addRequest.Result.packageId);
                else if (addRequest.Status >= StatusCode.Failure)
                    Debug.Log(addRequest.Error.message);

                EditorApplication.update -= AddProgress;
            }
        }

        private void RemoveManifest(string packageName)
        {
            removeReauest = Client.Remove(packageName);
            EditorApplication.update += RemoveProgress;
        }

        private void RemoveProgress()
        {
            if (removeReauest.IsCompleted)
            {
                if (removeReauest.Status == StatusCode.Success)
                    Debug.Log("Removed: " + removeReauest.PackageIdOrName);
                else if (removeReauest.Status >= StatusCode.Failure)
                    Debug.Log(removeReauest.Error.message);

                EditorApplication.update -= AddProgress;
            }
        }

        private void ListManifest()
        {
            listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        private void ListProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    packages = listRequest.Result;
                    foreach (var package in packages)
                        Debug.Log("Package name: " + package.name);
                }
                else if (listRequest.Status >= StatusCode.Failure)
                    Debug.Log(listRequest.Error.message);

                EditorApplication.update -= ListProgress;
            }
        }
    }
}
#endif
