using UnityEngine;
using Utilities;
using Management.UserInterface.FloatingTextPopupSystem;

public class PopupTesting : MonoBehaviour
{
    [SerializeField] private Transform _textObjectPrefab;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            bool isCritical = Random.Range(0, 10) < 3;
            TextPopup.Create(_textObjectPrefab, UtilityHelper.GetMouseWorldPosition(), Random.Range(10, 300).ToString(), isCritical);
        }
    }
}