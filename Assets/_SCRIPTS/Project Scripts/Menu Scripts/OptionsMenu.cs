using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CasualGame
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField, Required] private Slider _MusicSlider;
        [SerializeField, Required] private Slider _UISlider;

        [SerializeField] private Button _resetSaveButton;

        [Space]
        [SerializeField, Required] private AudioMixer _audioMixer;
        
        
        private void Awake() => LoadOptions();

        public void MusicVolumePreview(float volume) =>
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(_MusicSlider.value) * 20);

        public void UIVolumePreview(float volume) =>
            _audioMixer.SetFloat("UIVolume", Mathf.Log10(_UISlider.value) * 20);

        public void SaveOptions()
        {
            ScoreSystem.Instance.SetScoreValue(this, ValSysInit.StrID_Opt_MusicVol, _MusicSlider.value);
            ScoreSystem.Instance.SetScoreValue(this, ValSysInit.StrID_Opt_UIVol, _UISlider.value);

            SaveSystem.Save(this);
            SceneNavigator.LoadScene(0);
        }
        
        private void LoadOptions()
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Opt_MusicVol, out float musicVolume);
            _MusicSlider.value = musicVolume;
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Opt_UIVol, out float uiVolume);
            _UISlider.value = uiVolume;
        }

        public void ResetSaveData()
        {
            SaveSystem.ResetSavedData();
            _resetSaveButton.interactable = false;
        }

        public void ExitOptions() => SceneNavigator.LoadScene(0);
    }
}