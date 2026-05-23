using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dafral.Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string BOOTSTRAPPER_SCENE_NAME = "Bootstrapper";
        private const string LEVEL_SCENE_NAME = "Level";

#if UNITY_EDITOR
        private static string _editorOpenedSceneName;
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitInEditor()
        {
#if UNITY_EDITOR
            _editorOpenedSceneName = SceneManager.GetActiveScene().name;
            if (_editorOpenedSceneName != BOOTSTRAPPER_SCENE_NAME)
            {
                BootstrapEditorSetup();
            }
            else
            {
                BootstrapNormalSetup();
            }
#else
            BootstrapNormalSetup();
#endif
        }

        private static void BootstrapEditorSetup()
        {
            SceneManager.LoadSceneAsync(BOOTSTRAPPER_SCENE_NAME, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync(_editorOpenedSceneName, LoadSceneMode.Additive);
        }

        private static void BootstrapNormalSetup()
        {
            SceneManager.LoadSceneAsync(BOOTSTRAPPER_SCENE_NAME, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync(LEVEL_SCENE_NAME, LoadSceneMode.Additive);
        }
    }
}