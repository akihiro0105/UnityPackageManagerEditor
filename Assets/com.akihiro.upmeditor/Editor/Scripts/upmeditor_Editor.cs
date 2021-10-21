#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_Editor : EditorWindow
    {
        [MenuItem("UPM Editor/Import")]

        private static void ShowImportWindow() => EditorWindow.GetWindow(typeof(upmeditor_import));

        [MenuItem("UPM Editor/Export")]

        private static void ShowExportWindow() => EditorWindow.GetWindow(typeof(upmeditor_export));

    }
}
#endif
