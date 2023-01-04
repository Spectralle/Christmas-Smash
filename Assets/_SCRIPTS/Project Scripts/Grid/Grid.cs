using System;
using UnityEngine;


public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int X;
        public int Y;
    }
    
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }
    public Vector3 Anchor { get; private set; }
    private TGridObject[,] _gridArray;


    public Grid(int width, int height, float cellSize, Vector3 anchor, bool[,] gridSetup, Func<Grid<TGridObject>, int, int, int, int, bool, TGridObject> createGridObject)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        Anchor = anchor;

        _gridArray = new TGridObject[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
            for (int y = 0; y < _gridArray.GetLength(1); y++)
                _gridArray[x, y] = createGridObject(this, x, y, width, height, gridSetup[x,y]);
    }

    public void GridObjectChanged(int x, int y) =>
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { X = x, Y = y });
    
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
            _gridArray[x, y] = value;
    }
    
    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
            return _gridArray[x, y];
        else
        {
            Debug.Log("GetGridObject(" + x + ", " + y + ") failed: Invalid X and/or Y.");
            return default(TGridObject);
        }
    }

    private Vector3 GetAnchoredWorldPosition(int x, int y) =>
        Anchor - (new Vector3(Width, Height) / 2f) + GetWorldPosition(x, y);
    
    private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * CellSize;
}
