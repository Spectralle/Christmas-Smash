#if UNITY_EDITOR
using UnityEngine;

namespace CasualGame
{
    using Sirenix.Utilities;
    using Sirenix.OdinInspector;
    using System.Linq;
    using UnityEditor;

    // ORIGINAL ODIN DESCRIPTION:
    // This is a scriptable object containing a list of all characters available
    // in the Unity project. When a character is added from the RPG editor, the
    // list then gets automatically updated via UpdateCharacterOverview. 
    //
    // If you inspect the Character Overview in the inspector, you will also notice, that
    // the list is not directly modifiable. Instead, we've customized it so it contains a 
    // refresh button, that scans the project and automatically populates the list.
    //
    // CharacterOverview inherits from GlobalConfig which is just a scriptable 
    // object singleton, used by Odin Inspector for configuration files etc, 
    // but it could easily just be a simple scriptable object instead.
    // 

    [GlobalConfig("_SCRIPTABLE OBJECTS")]
    public class ScriptableOverview : GlobalConfig<ScriptableOverview> 
    {
        [ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public GridItemSO[] AllGems;
        
        [Space, ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public GridItemSO[] AllBlockers;
        
        [Space, ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public UpgradeSO[] AllUpgrades;
        
        [Space, ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public LevelSO[] AllLevels;

        
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateOverview()
        {
            this.AllGems = AssetDatabase.FindAssets("t:GridItemSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<GridItemSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item.Destructible)
                .ToArray();
            
            this.AllBlockers = AssetDatabase.FindAssets("t:GridItemSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<GridItemSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => !item.Destructible)
                .ToArray();
            
            this.AllUpgrades = AssetDatabase.FindAssets("t:UpgradeSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<UpgradeSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            
            this.AllLevels = AssetDatabase.FindAssets("t:GridSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<LevelSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
    }
}
#endif