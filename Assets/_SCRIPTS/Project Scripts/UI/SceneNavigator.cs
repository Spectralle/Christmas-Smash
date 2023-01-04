using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace CasualGame
{
    public static class SceneNavigator
    {
        public static Scene ActiveScene => SceneManager.GetActiveScene();
        private static SceneNavigatorTransition SNT => SceneNavigatorTransition.Instance;

        private static readonly float _fadeDuration = 0.7f;
        private static readonly float _fadeDelay = 0.3f;
        private static readonly Vector2 _fadeZeroToOne = Vector2.up;
        private static readonly Vector2 _fadeOneToZero = Vector2.right;
        

        public static void LoadScene(int buildIndex) =>
            SNT.Fade2Way(_fadeZeroToOne, _fadeOneToZero, _fadeDuration, _fadeDelay, 
                () => SceneManager.LoadScene(buildIndex));

        public static void LoadSceneAsync(int buildIndex) =>
            SNT.Fade2Way(_fadeZeroToOne, _fadeOneToZero, _fadeDuration, _fadeDelay, 
                () => SceneManager.LoadSceneAsync(buildIndex));

        public static void UnloadSceneAsync(int buildIndex)
        {
            if (ActiveScene != SceneManager.GetSceneByBuildIndex(buildIndex))
                return;

            SceneManager.UnloadSceneAsync(buildIndex);
        }

        public static void QuitGame(object sender)
        {
            SaveSystem.Save(sender);
            
            SNT.Fade(0, 1, _fadeDuration);
            #if UNITY_EDITOR
            UtilityHelper_FunctionTimer.Create(() => EditorApplication.isPlaying = false, _fadeDuration + 0.1f);
            #else
            UtilityHelper_FunctionTimer.Create(() => Application.Quit(), _fadeDuration + 0.1f);
            #endif
        }
    }
}