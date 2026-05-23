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
            LoadAdditiveAndSetActive(_editorOpenedSceneName);
        }

        private static void BootstrapNormalSetup()
        {
            SceneManager.LoadSceneAsync(BOOTSTRAPPER_SCENE_NAME, LoadSceneMode.Single);
            LoadAdditiveAndSetActive(LEVEL_SCENE_NAME);
        }

        private static void LoadAdditiveAndSetActive(string sceneName)
        {
            AsyncOperation additiveLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            additiveLoad.completed += _ =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                if (loadedScene.IsValid() && loadedScene.isLoaded)
                {
                    SceneManager.SetActiveScene(loadedScene);
                }
            };
        }
    }
}