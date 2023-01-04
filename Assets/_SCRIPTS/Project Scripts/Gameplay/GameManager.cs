using UnityEngine;

namespace CasualGame
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsGameCompleted;
        
        
        private void Start() => GridManager.InitializeGridItems();
    }
}