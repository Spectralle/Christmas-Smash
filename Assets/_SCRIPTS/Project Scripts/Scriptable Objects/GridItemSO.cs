using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable InconsistentNaming

namespace CasualGame
{
    [CreateAssetMenu(fileName = "Grid Item SO", menuName = "SO/Grid Item", order = 0)]
    public class GridItemSO : ScriptableObject
    {
        [Required, HideLabel, PreviewField(60f, ObjectFieldAlignment.Center), OnValueChanged("OnValuesChanged")]
        [BoxGroup("b", showLabel:false), HorizontalGroup("b/h", width:65)]
        public Sprite Sprite;
        
        [Required, HideInInspector]
        public string Name;

        [ProgressBar(0, 20, 0.2f, 0.5f, 1f), BoxGroup("b/h/$Name")]
        [OnValueChanged("OnValuesChanged"), HideLabel, Range(0, 20), ShowIf("Destructible")]
        public int PointScore = 1;

        [HideInInspector]
        public bool Destructible = true;

        
        #if UNITY_EDITOR
        private void OnValuesChanged()
        {
            PointScore = Destructible switch
            {
                false when PointScore > 0 => 0,
                true when PointScore == 0 => 1,
                _ => PointScore
            };
            
            string n = Sprite ? Sprite.name.Replace("-", " ") : "NO SPRITE!";
            string d = Destructible ? $"{PointScore}p" : "Indestructible";
            Name = $"{n} - {d}";
            name = Name;
            
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, name);
            AssetDatabase.SaveAssets();
        }

        [Button, BoxGroup("b/h/$Name")]
        private void ToggleDestructible()
        {
            Destructible = !Destructible;
            OnValuesChanged();
        }
        #endif
    }
}