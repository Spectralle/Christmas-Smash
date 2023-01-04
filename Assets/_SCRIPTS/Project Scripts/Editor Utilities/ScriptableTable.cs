#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CasualGame
{
    // ORIGINAL ODIN DESCRIPTION:
    // This class is used by the RPGEditorWindow to render an overview of all characters using the TableList attribute.
    // All characters are Unity objects though, so they are rendered in the inspector as single Unity object field,
    // which is not exactly what we want in our table. We want to show the members of the unity object.
    //
    // So in order to render the members of the Unity object, we'll create a class that wraps the Unity object
    // and displays the relevant members through properties, which works with the TableList, attribute.

    public class GemTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<GridItemWrapper> allGems;
        public GridItemSO this[int index] => allGems[index].GridItem;

        public GemTable(IEnumerable<GridItemSO> gems) => allGems = gems.Select(x => new GridItemWrapper(x)).ToList();

        private class GridItemWrapper
        {
            public GridItemSO GridItem { get; private set; }

            public GridItemWrapper(GridItemSO gem) => GridItem = gem;

            [TableColumnWidth(50, false)]
            [Required, HideLabel, PreviewField(60f, ObjectFieldAlignment.Center), LabelWidth(70)]
            public Sprite Icon { get => GridItem.Sprite; set { GridItem.Sprite = value; EditorUtility.SetDirty(GridItem); } }

            [TableColumnWidth(120)]
            [ShowInInspector]
            public string Name { get => GridItem.Name; set { GridItem.Name = value; EditorUtility.SetDirty(GridItem); } }

            //[ShowInInspector, ProgressBar(0, 100)]
            //public float Shooting { get { return gem.Skills.Shooting; } set { gem.Skills.Shooting = value; EditorUtility.SetDirty(gem); } }
            
            [ProgressBar(0, 20, 0.2f, 0.5f, 1f)]
            [OnValueChanged("OnValuesChanged"), HideLabel, ShowIf("Destructible", true)]
            public int PointScore { get => GridItem.PointScore; set { GridItem.PointScore = value; EditorUtility.SetDirty(GridItem); } }
        }
    }
    
    public class BlockerTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<BlockerWrapper> allBlockers;
        public GridItemSO this[int index] => allBlockers[index].GridItem;

        public BlockerTable(IEnumerable<GridItemSO> blockers) => allBlockers = blockers.Select(x => new BlockerWrapper(x)).ToList();

        private class BlockerWrapper
        {
            public GridItemSO GridItem { get; private set; }

            public BlockerWrapper(GridItemSO blocker) => GridItem = blocker;

            [TableColumnWidth(50, false)]
            [Required, HideLabel, PreviewField(60f, ObjectFieldAlignment.Center), LabelWidth(70)]
            public Sprite Icon { get => GridItem.Sprite; set { GridItem.Sprite = value; EditorUtility.SetDirty(GridItem); } }

            [TableColumnWidth(120)]
            [ShowInInspector]
            public string Name { get => GridItem.Name; set { GridItem.Name = value; EditorUtility.SetDirty(GridItem); } }
        }
    }
    
    public class UpgradeTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<UpgradeWrapper> allUpgrades;
        public UpgradeSO this[int index] => allUpgrades[index].Upgrade;

        public UpgradeTable(IEnumerable<UpgradeSO> upgrades) =>
            allUpgrades = upgrades.Select(x => new UpgradeWrapper(x)).ToList();

        private class UpgradeWrapper
        {
            public UpgradeSO Upgrade { get; }

            public UpgradeWrapper(UpgradeSO character) => Upgrade = character;

            [TableColumnWidth(50, false), Required]
            [PreviewField(60f, ObjectFieldAlignment.Center), LabelWidth(70)]
            public Sprite Icon { get => Upgrade.Icon; set { Upgrade.Icon = value; EditorUtility.SetDirty(Upgrade); } }

            [TableColumnWidth(120), ShowInInspector]
            public string Name { get => Upgrade.Name; set { Upgrade.Name = value; EditorUtility.SetDirty(Upgrade); } }

            [TableColumnWidth(120), ShowInInspector]
            public string Description { get => Upgrade.Description; set { Upgrade.Description = value; EditorUtility.SetDirty(Upgrade); } }

            [ProgressBar(0, 20, 0.2f, 0.5f, 1f)]
            public int Cost { get => Upgrade.Cost; set { Upgrade.Cost = value; EditorUtility.SetDirty(Upgrade); } }
        }
    }
    
    public class LevelTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<LevelWrapper> allLevels;
        public LevelSO this[int index] => allLevels[index].Level;

        public LevelTable(IEnumerable<LevelSO> levels) =>
            allLevels = levels.Select(x => new LevelWrapper(x)).ToList();

        private class LevelWrapper
        {
            public LevelSO Level { get; private set; }

            public LevelWrapper(LevelSO level) => Level = level;

            [ReadOnly] public string Name => Level.name;
        }
    }
}
#endif