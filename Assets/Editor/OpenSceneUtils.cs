using UnityEditor;
using UnityEditor.SceneManagement;

namespace Wepie.Editor
{
    public class OpenSceneUtils
    {
        [MenuItem("Tools/OpenScene(Entry) %q")]
        public static void EntranceDebug()
        {
            if (EditorApplication.isPlaying) return;
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/Entry.unity");
            EditorApplication.isPlaying = true;
        }
    }
}