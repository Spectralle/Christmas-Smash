using System;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using Management.UserInterface.FloatingTextPopupSystem;
using Utilities;

namespace CasualGame
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private Transform _textPopupPrefab;
        [SerializeField, SceneObjectsOnly] private Transform _textPopupAnchor;

        private readonly PopupParameters _params = new(
            0f, 0.1f,
            normalTextSize: 0.2f, criticalTextSize: 0.2f,
            normalTextColor: "10CD00", criticalTextColor: "FFB600",
            scaleIncreaseAmount: 0.3f, scaleDecreaseAmount: 0.3f,
            disappearTimerMax: 2.5f
        );


        private void Start()
        {
            bool saveExists = File.Exists(SaveSystem.GetFilePath);
            UtilityHelper_FunctionTimer.Create(() => TextPopup.Create(_textPopupPrefab, _textPopupAnchor.position, 
                saveExists ? "Save file found" : "No Save file found", isCritical:!saveExists, parameters:_params),
                1f);
        }

        public void LoadOptions() => SceneNavigator.LoadSceneAsync(1);
        public void LoadShop() => SceneNavigator.LoadSceneAsync(2);
        public void LoadLevelSelection() => SceneNavigator.LoadScene(3);
        public void QuitGame() => SceneNavigator.QuitGame(this);
    }
}