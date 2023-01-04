using System.Collections.Generic;

namespace CasualGame
{
    public class Cell
    {
        public GridItemSO GridItem { get; private set; }
        public Grid<Cell> CellGrid { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public bool IsLocked { get; private set; }
        public List<Cell> Neighbors { get; private set; }


        public Cell(Grid<Cell> grid, int x, int y, int gridWidth, int gridHeight, bool isLocked)
        {
            CellGrid = grid;
            X = x;
            Y = y;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            IsLocked = isLocked;
            Neighbors = GetNeighbors();
        }

        private List<Cell> GetNeighbors()
        {
            var neighbors = new List<Cell>();
            if (Y + 1 < GridHeight)
            {
                neighbors.Add(CellGrid.GetGridObject(X, Y + 1));
                if (X - 1 >= 0)
                    neighbors.Add(CellGrid.GetGridObject(X - 1, Y + 1));
                if (X + 1 < GridWidth)
                    neighbors.Add(CellGrid.GetGridObject(X + 1, Y + 1));
            }

            if (X - 1 >= 0)
                neighbors.Add(CellGrid.GetGridObject(X - 1, Y));
            if (X + 1 < GridWidth)
                neighbors.Add(CellGrid.GetGridObject(X + 1, Y));

            if (Y - 1 < 0)
                return neighbors;

            neighbors.Add(CellGrid.GetGridObject(X, Y - 1));
            if (X - 1 >= 0)
                neighbors.Add(CellGrid.GetGridObject(X - 1, Y - 1));
            if (X + 1 < GridWidth)
                neighbors.Add(CellGrid.GetGridObject(X + 1, Y - 1));

            return neighbors;
        }

        public void ChangeItem(GridItemSO newGridItem)
        {
            GridItem = newGridItem;
            CellGrid.GridObjectChanged(X, Y);
        }

        public void SetIsMovable(bool newValue)
        {
            IsLocked = newValue;
            CellGrid.GridObjectChanged(X, Y);
        }

        public void SwitchIsMovable()
        {
            IsLocked = !IsLocked;
            CellGrid.GridObjectChanged(X, Y);
        }

        public override string ToString() => GridItem ? GridItem.Name : "Empty";
    }
}