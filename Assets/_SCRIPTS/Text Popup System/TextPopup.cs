using UnityEngine;
using TMPro;
using Utilities;

namespace Management.UserInterface.FloatingTextPopupSystem
{
    public class TextPopup : MonoBehaviour
    {
        private PopupParameters _parameters;
        private TextMeshPro _textMesh;
        private Vector3 _moveSpeed;
        private float _disappearTimer;
        private Color _textColor;
        private static int _sortingOrder;
        private static int _minSortingOrder;


        private void Awake() => _textMesh = transform.GetComponent<TextMeshPro>();

        public static TextPopup Create(Transform prefab, Vector3 position, string popupMessage, bool isCritical = false, PopupParameters parameters = null)
        {
            Transform damagePopupTransform = Instantiate(prefab, position, Quaternion.identity);
            
            TextPopup textPopup = damagePopupTransform.GetComponent<TextPopup>();
            textPopup._parameters = parameters ?? new PopupParameters();
            textPopup.Setup(popupMessage, isCritical);
            
            return textPopup;
        }

        private void Setup(string popupMessage, bool isCritical)
        {
            _textMesh.SetText(popupMessage);
            
            _textMesh.fontSize = !isCritical ?
                _parameters.normalTextSize :
                _parameters.criticalTextSize;
            _textMesh.fontStyle = !isCritical ?
                FontStyles.Normal :
                FontStyles.Bold;
            _textColor = !isCritical ?
                UtilityHelper.GetColorFromString(_parameters.normalTextColor) :
                UtilityHelper.GetColorFromString(_parameters.criticalTextColor);
            
            _textColor.a = Mathf.Clamp01(_parameters.textStartingAlpha);
            _textMesh.color = _textColor;
            _disappearTimer = _parameters.disappearTimerMax;
            _moveSpeed = new Vector3(_parameters.moveXSpeed, _parameters.moveYSpeed);

            if (_minSortingOrder < _parameters.minSortingLayer)
                _minSortingOrder = _parameters.minSortingLayer;
            
            if (_sortingOrder < _minSortingOrder)
                _sortingOrder = _minSortingOrder;
            else if (_sortingOrder >= _minSortingOrder + 800)
                _sortingOrder = _minSortingOrder;
            else
                _sortingOrder++;
            _textMesh.sortingOrder = _sortingOrder;
        }

        private void Update()
        {
            // Move by MoveSpeed
            _moveSpeed -= _moveSpeed * (4f * Time.deltaTime);
            transform.position += _moveSpeed * Time.deltaTime;

            if (_disappearTimer > _parameters.disappearTimerMax * 0.5f)
                // First part of popup's lifetime
                transform.localScale += Vector3.one * (_parameters.scaleIncreaseAmount * Time.deltaTime);
            else
                // Last part of popup's lifetime
                transform.localScale -= Vector3.one * (_parameters.scaleDecreaseAmount * Time.deltaTime);

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0)
            {
                // Start disappearing
                _textColor.a -= _parameters.disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;
                if (_textColor.a < 0)
                    Destroy(gameObject);
            }
        }
    }
}