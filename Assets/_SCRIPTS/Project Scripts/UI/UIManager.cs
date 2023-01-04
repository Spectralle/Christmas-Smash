using System;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI.ProceduralImage;

namespace CasualGame
{
    public class UIManager : Singleton<UIManager>
    {
        [Title("Game HUD")]
        [SerializeField, Required] private TextMeshProUGUI _timerText;
        [SerializeField, Required] private TextMeshProUGUI _scoreText;
        [SerializeField, Required] private TextMeshProUGUI _shotCountText;
        [SerializeField, Required] private ProceduralImage _scoreProgressBar;
        [Title("Game End")]
        [SerializeField, Required] private Canvas _winScreen;
        [SerializeField, Required] private TextMeshProUGUI _finalTime;
        [SerializeField, Required] private TextMeshProUGUI _finalShots;
        [SerializeField, Required] private TextMeshProUGUI _finalScore;
        [SerializeField, Required] private TextMeshProUGUI _finalGoldEarned;
        [Title("Pause Effects")]
        [SerializeField, Required] private Canvas _pauseScreen;
        
        private float _gameTimer;
        private float _receivedValue, _currentSbFill, _realSbFill;

        
        public void Init(int scoreGoal)
        {
            ScoreSystem.Instance.OnValueChanged += UpdateUI;

            _gameTimer = 0;
            ScoreSystem.Instance.GetMainScoreValue(out float score);
            _scoreText.SetText($"{(int)score} / {scoreGoal}");
            _scoreProgressBar.fillAmount = 0;
            
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ShotsTaken, out float shots);
            _shotCountText.SetText(shots.ToString());
        }

        private void Start()
        {
            PauseManager.OnPaused += PauseGame;
            PauseManager.OnResumed += ResumeGame;
        }

        private void OnDestroy()
        {
            ScoreSystem.Instance.OnValueChanged -= UpdateUI;
            PauseManager.OnPaused -= PauseGame;
            PauseManager.OnResumed -= ResumeGame;
        }

        private void Update()
        {
            if (PauseManager.IsPaused() || GameManager.IsGameCompleted)
                return;
            
            _gameTimer += Time.smoothDeltaTime;
            int mins = (int)_gameTimer / 60;
            int secs = (int)_gameTimer - (60 * mins);
            _timerText.SetText($"{mins:00}:{secs:00}");
            
            if (!(Math.Abs(_currentSbFill - _realSbFill) > 0.01f))
                return;

            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ScoreGoal, out float scoreGoal);
            float newVal = _receivedValue / scoreGoal;
            _scoreProgressBar.fillAmount = Mathf.Lerp(_scoreProgressBar.fillAmount, newVal, Time.deltaTime * 3f);
            _currentSbFill = _scoreProgressBar.fillAmount;
        }

        private void UpdateUI(object s, ScoreSystem.OnValueChangedEventArgs e)
        {
            switch (e.Name)
            {
                case ValSysInit.StrID_CurrentScore:
                    ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ScoreGoal, out float scoreGoal);
                    _scoreText.SetText($"{e.NewValue} / {scoreGoal}");
                    _receivedValue = e.NewValue;
                    _currentSbFill = _scoreProgressBar.fillAmount;
                    _realSbFill = e.NewValue / scoreGoal;
                    break;
                case ValSysInit.StrID_ShotsTaken:
                    _shotCountText.SetText(e.NewValue.ToString());
                    break;
                default:
                    break;
            }
        }

        private void PauseGame(object s, EventArgs e) => _pauseScreen.enabled = true;

        private void ResumeGame(object s, EventArgs e) => _pauseScreen.enabled = false;

        public static void ShowWinScreen(int levelID, int goldEarned)
        {
            ResetUIScreens();
            
            LevelSO level = GridManager.Instance.GetCurrentLevel();
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ShotsTaken, out float shotsUsed);

            LevelsSaveData.SetLevelData(levelID, (int)Instance._gameTimer, (int)shotsUsed);
            
            int minutes = (int)Instance._gameTimer / 60;
            int seconds = (int)Instance._gameTimer - (60 * minutes);
            Instance._finalTime.SetText($"{minutes:00}:{seconds:00}");
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ShotsTaken, out float finalShots);
            Instance._finalShots.SetText(finalShots.ToString());
            ScoreSystem.Instance.GetMainScoreValue(out float score);
            Instance._finalScore.SetText(score.ToString());
            Instance._finalGoldEarned.SetText(goldEarned.ToString());
            
            ValSysInit.ResetScoreSystem();
            SaveSystem.Save(Instance);
            
            Instance._winScreen.enabled = true;
        }

        private static void ResetUIScreens()
        {
            Instance._winScreen.enabled = false;
            Instance._pauseScreen.enabled = false;
        }

        public void ReturnToLevelSelect()
        {
            SceneNavigator.LoadScene(3);
            GameManager.IsGameCompleted = false;
        }

        public void ReturnToMain() => SceneNavigator.LoadScene(0);
    }
}