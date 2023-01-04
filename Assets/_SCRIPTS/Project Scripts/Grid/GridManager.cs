using UnityEngine;
using Sirenix.OdinInspector;
using Utilities;


namespace CasualGame
{
    public class GridManager : Singleton<GridManager>
    {
        [Title("Game grid")] [SerializeField, Required, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        private LevelSO[] _allLevels;

        private static int Width { get; set; }
        private static int Height { get; set; }

        private static Grid<Cell> _gameGrid;
        private static float _scoreGoal;


        public static void InitializeGridItems()
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelToLoad, out float levelNumberToLoad);
            Debug.Log($"Loading Level {levelNumberToLoad}...");
            LevelSO levelToLoad = Instance._allLevels[(int)levelNumberToLoad - 1];
            Width = levelToLoad.GetWidth();
            Height = levelToLoad.GetHeight();

            _gameGrid = new Grid<Cell>(Width, Height, 1f, Vector3.zero, levelToLoad.Grid,
                (g, x, y, sx, sy, iv) =>
                    new Cell(g, x, y, sx, sy, iv));

            GridVisualManager.Instance.Setup(_gameGrid);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_ScoreGoal, out _scoreGoal);
        }

        public static void CheckForEndGame()
        {
            ScoreSystem.Instance.GetMainScoreValue(out float score);
            if ((int)score < (int)_scoreGoal)
                return;
            
            GameManager.IsGameCompleted = true;
            UtilityHelper_FunctionTimer.Create(EndGame, 2f);
        }

        private static void EndGame()
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelToLoad, out float levelNumberToLoad);
            int goldEarned = (int)(_scoreGoal / 4);
            UIManager.ShowWinScreen((int)levelNumberToLoad, goldEarned);

            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelsCleared, out float levelsCleared);
            Debug.Log($"{levelNumberToLoad} should be higher than {levelsCleared}");
            ScoreSystem.Instance.SetScoreValueIfBLargerOrNotZero(Instance,
                ValSysInit.StrID_LevelsCleared, levelNumberToLoad);
            ScoreSystem.Instance.SetScoreValueIfBLargerOrNotZero(Instance,
                ValSysInit.StrID_HighestLevelScore, _scoreGoal);
            ScoreSystem.Instance.ModifyScoreValue(Instance, ValSysInit.StrID_Currency, goldEarned);
            ScoreSystem.Instance.ModifyScoreValue(Instance, ValSysInit.StrID_LevelsPlayed, 1);
        }

        public LevelSO GetCurrentLevel()
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelToLoad, out float levelIndex);
            return _allLevels[(int)levelIndex - 1];
        }
    }
}