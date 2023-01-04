using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace CasualGame
{
    [CreateAssetMenu(fileName = "Grid SO", menuName = "SO/Grid", order = 0)]
    public class LevelSO : SerializedScriptableObject
    {
        #if UNITY_EDITOR
        [TableMatrix(DrawElementMethod = "DrawColoredElement", ResizableColumns = false, SquareCells = true, HideColumnIndices = true,HideRowIndices = true)]
        #endif
        [HideLabel, ShowInInspector]
        public bool[,] Grid;

        [BoxGroup("Developer Options")]
        [SerializeField, HorizontalGroup("Developer Options/h"), LabelWidth(45), Range(1,12)] private int _width = 9;
        [SerializeField, HorizontalGroup("Developer Options/h"), LabelWidth(45), Range(1,12)] private int _height = 7;
        
        
        [OnInspectorInit]
        private void InitializeGrid()
        {
            if (Grid == null || Grid.Length == 0)
                Grid = new bool[_width, _height];
        }
        
        [Button, BoxGroup("Developer Options")]
        private void SetGridWhite() => Grid = new bool[_width, _height];
        [Button, BoxGroup("Developer Options")]
        private void SetGridBlack()
        {
            Grid = new bool[_width, _height];
            for (int x = 0; x < Grid.GetLength(0); x++)
                for (int y = 0; y < Grid.GetLength(1); y++)
                    Grid[x, y] = true;
        }

        public void Set(bool[,] newGrid) => Grid = newGrid;
        public bool[,] Get() => Grid;
        public int GetWidth() => Grid.GetLength(0);
        public int GetHeight() => Grid.GetLength(1);

        public void SetCell(int x, int y, bool value) => Grid[x, y] = value;
        public bool GetCell(int x, int y) => Grid[x, y];

        #if UNITY_EDITOR
        private static bool DrawColoredElement(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            Color color = value ? Color.black : Color.white;
            UnityEditor.EditorGUI.DrawRect(rect.Padding(0), color);
            return value;
        }
        #endif
    }
}