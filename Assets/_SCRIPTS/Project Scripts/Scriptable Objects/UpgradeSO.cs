using Sirenix.OdinInspector;
using UnityEngine;

namespace CasualGame
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "SO/Upgrade", order = 0)]
    public class UpgradeSO : SerializedScriptableObject
    {
        [Required] public string Name;
        public string Description;
        [Required] public Sprite Icon;
        [Min(0)] public int Cost;
    }
}