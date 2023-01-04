#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace CasualGame
{
    // Be sure to check out OdinMenuStyleExample.cs as well. It shows you various ways to customize
    // the look and behaviour of OdinMenuTrees.

    public class ManagementWindow : OdinMenuEditorWindow
    {
        [MenuItem("TIS Tools/Game Management Window")]
        private static void OpenWindow()
        {
            ManagementWindow window = GetWindow<ManagementWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        // Take a look at SomeData.cs to see how serialization works in Editor Windows.
        //[SerializeField]
        //private SomeData someData = new SomeData();

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Project Settings",    Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault()},
                { "Gems",                this,             EditorIcons.LockUnlocked           },
                { "Blockers",            this,             EditorIcons.LockLocked             },
                { "Upgrades",            this,             EditorIcons.Upload                 },
                { "Levels",              this,             EditorIcons.GridBlocks             },
                //{ "Some Class",          this.someData                                        }
            };
            
            tree.Add("Gems", new GemTable(ScriptableOverview.Instance.AllGems));
            tree.Add("Blockers", new BlockerTable(ScriptableOverview.Instance.AllBlockers));
            tree.Add("Upgrades", new UpgradeTable(ScriptableOverview.Instance.AllUpgrades));
            tree.Add("Levels", new LevelTable(ScriptableOverview.Instance.AllLevels));

            tree.AddAllAssetsAtPath("Gems", "_SCRIPTABLE OBJECTS/Gems", typeof(GridItemSO));
            tree.AddAllAssetsAtPath("Blockers", "_SCRIPTABLE OBJECTS/Blocks", typeof(GridItemSO));
            tree.AddAllAssetsAtPath("Upgrades", "_SCRIPTABLE OBJECTS/Upgrades", typeof(UpgradeSO));
            tree.AddAllAssetsAtPath("Levels", "_SCRIPTABLE OBJECTS/Levels", typeof(LevelSO));

            tree.MenuItems.Add(new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle));

            // As you can see, Odin provides a few ways to quickly add editors / objects to your menu tree.
            // The API also gives you full control over the selection, etc..
            // Make sure to check out the API Documentation for OdinMenuEditorWindow, OdinMenuTree and OdinMenuItem for more information on what you can do!

            return tree;
        }
        
        protected override void OnBeginDrawEditors()
        {
            OdinMenuItem selected = this.MenuTree.Selection.FirstOrDefault();
            int toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                    GUILayout.Label(selected.Name);
                    
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh SO List")))
                    ScriptableOverview.Instance.UpdateOverview();
                    
                if (selected != null)
                {
                    switch (selected.Name)
                    {
                        case "Gems":
                            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Grid Item")))
                            {
                                ScriptableObjectCreator.ShowDialog<GridItemSO>("Assets/_SCRIPTABLE OBJECTS/Gems", obj =>
                                {
                                    obj.Name = obj.name;
                                    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                                });
                            }
                            break;
                        case "Blockers":
                            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Grid Blocker")))
                            {
                                ScriptableObjectCreator.ShowDialog<GridItemSO>("Assets/_SCRIPTABLE OBJECTS/Blockers", obj =>
                                {
                                    obj.Name = obj.name;
                                    obj.Destructible = false;
                                    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                                });
                            }
                            break;
                        case "Upgrades":
                            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Upgrade")))
                            {
                                ScriptableObjectCreator.ShowDialog<UpgradeSO>("Assets/_SCRIPTABLE OBJECTS/Upgrades", obj =>
                                {
                                    obj.Name = obj.name;
                                    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                                });
                            }
                            break;
                        case "Levels":
                            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Level")))
                            {
                                ScriptableObjectCreator.ShowDialog<LevelSO>("Assets/_SCRIPTABLE OBJECTS/Levels", 
                                    obj => base.TrySelectMenuItemWithObject(obj));
                            }
                            break;
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
#endif
