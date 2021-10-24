#if UNITY_EDITOR
using UnityEditor;

namespace com.akihiro.upmeditor.editor
{
    public class upmeditor_Editor : EditorWindow
    {
        [MenuItem("UPM Editor/Import")]
        private static void ShowImportWindow() => EditorWindow.GetWindow(typeof(upmeditor_import), true, "Unity Package Manager Import");

        [MenuItem("UPM Editor/Export")]
        private static void ShowExportWindow() => EditorWindow.GetWindow(typeof(upmeditor_export), true, "Unity Package Manager Export");
    }
}
#endif
