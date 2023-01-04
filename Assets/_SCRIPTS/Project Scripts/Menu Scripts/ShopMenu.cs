using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utilities;

namespace CasualGame
{
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField, Required, InlineEditor()] private TextMeshProUGUI _popupText;
        
        
        private void Awake()
        {
            // todo Load upgrade data from save system
        }
        
        public void PurchaseUpgrade(UpgradeSO selectedUpgrade)
        {
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Currency, out float gold);
            if (selectedUpgrade.Cost <= (int)gold)
            {
                ScoreSystem.Instance.ModifyScoreValue(this, ValSysInit.StrID_Currency, -selectedUpgrade.Cost);
                Debug.Log($"Purchased upgrade {selectedUpgrade.Name} for {selectedUpgrade.Cost} gold.");
                _popupText.SetText("Purchased upgrade");
                UtilityHelper_FunctionTimer.Create(() => _popupText.SetText(""), 7f);
            }
            else
            {
                Debug.Log($"Cannot purchase upgrade {selectedUpgrade.Name} for {selectedUpgrade.Cost}.\nNot enough gold.");
                _popupText.SetText("Cannot purchase upgrade");
                UtilityHelper_FunctionTimer.Create(() => _popupText.SetText(""), 7f);
            }
        }

        public void ExitShop() => SceneNavigator.UnloadSceneAsync(3);
    }
}