using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace CasualGame
{
    public class LevelSelectMenu : MonoBehaviour
    {
        [SerializeField, Required] private Button[] _levelButtons;
        [SerializeField, Required] private LineRenderer _lineRenderer;


        private void Awake() => _lineRenderer = GetComponent<LineRenderer>();

        private void Start()
        {
            UpdateLinePointPositions();
            UpdateLevelAccess();
        }

        public void ReturnToMainMenu() => SceneNavigator.LoadScene(0);
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ScoreSystem.Instance.ModifyScoreValue(this, ValSysInit.StrID_LevelsCleared, 1);
                UpdateLevelAccess();
            }
        }
        #endif

        private void UpdateLevelAccess()
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelsCleared, out float levelsCleared);
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                _levelButtons[i].interactable = i <= (int)levelsCleared;
                _levelButtons[i].GetComponentInChildren<TextAnimator>().enabled = i <= (int)levelsCleared;
            }
        }
        
        [Button]
        private void UpdateLinePointPositions()
        {
            if (!_lineRenderer)
                return;

            int pointNumber = _levelButtons.Length;
            if (Application.isPlaying)
            {
                ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelsCleared, out float levelsCleared);
                pointNumber = Mathf.Min((int)levelsCleared + 1, pointNumber);
            }
            _lineRenderer.positionCount = pointNumber;
            for (int i = 0; i < _lineRenderer.positionCount; i++)
                _lineRenderer.SetPosition(i, _levelButtons[i].transform.parent.position);
        }

        public void LoadLevel(int levelNumber)
        {
            ScoreSystem.Instance.SetScoreValue(this,ValSysInit.StrID_LevelToLoad, levelNumber);
            SceneNavigator.LoadScene(4);
        }
    }
}
