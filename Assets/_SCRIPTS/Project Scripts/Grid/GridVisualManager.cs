using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor;

namespace CasualGame
{
    public class GridVisualManager : Singleton<GridVisualManager>
    {
        [Title("Grid")]
        [SerializeField, Required, SceneObjectsOnly] private GridLayoutGroup _cellParent;
        [SerializeField, Required, AssetsOnly] private GameObject _cellPrefab;
        [Title("Grid Items")]
        [SerializeField, Required, InlineEditor(InlineEditorObjectFieldModes.Hidden)] private GridItemSO[] _possibleItems;
        [Space]
        [SerializeField, Required, InlineEditor(InlineEditorObjectFieldModes.Hidden)] private GridItemSO[] _frozenItems;

        private Grid<Cell> _grid;

        
        protected override void Awake()
        {
            base.Awake();
            #if UNITY_EDITOR
            UpdateSOAssets();
            #endif
        }

        #if UNITY_EDITOR
        [Button("Update Assets")]
        private void UpdateSOAssets()
        {
            _possibleItems = AssetDatabase.FindAssets("t:GridItemSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<GridItemSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item.Destructible)
                .OrderByDescending(item => item.PointScore)
                .ToArray();
            
            _frozenItems = AssetDatabase.FindAssets("t:GridItemSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<GridItemSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => !item.Destructible)
                .ToArray();
        }
        #endif

        public void Setup(Grid<Cell> grid)
        {
            _grid = grid;
            int scoreGoal = 0;
            GridLayoutGroup gridLayout = GetComponentInChildren<GridLayoutGroup>();
            for (int y = 0; y < _grid.Height; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    bool isLocked = _grid.GetGridObject(x, y).IsLocked;
                    GridItemSO gridItem = !isLocked
                        ? _possibleItems[Random.Range(0, _possibleItems.Length)]
                        : _frozenItems[Random.Range(0, _frozenItems.Length)];
                    _grid.GetGridObject(x, y).ChangeItem(gridItem);

                    GameObject cellGO = Instantiate(_cellPrefab, _cellParent.transform);
                    cellGO.name = gridItem.Name;
                    cellGO.GetComponentInChildren<BoxCollider2D>().size = gridLayout.cellSize;
                    cellGO.GetComponent<Block>().Init(_grid, x, y, gridItem);

                    scoreGoal += !isLocked ? gridItem.PointScore : 0;
                }
            }

            ScoreSystem.Instance.SetScoreValue(this, ValSysInit.StrID_ScoreGoal, scoreGoal);
            UIManager.Instance.Init(scoreGoal);
        }
    }
}