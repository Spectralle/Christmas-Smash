using UnityEngine;
using Utilities;
using Management.UserInterface.FloatingTextPopupSystem;
using SpriteShatter;

namespace CasualGame
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Transform _popupPrefab;
        [SerializeField] private ParticleSystem _blockerParticleSystem;

        public bool IsDestructible => _item.Destructible;
        
        private Grid<Cell> _grid;
        private Vector2 _pos;
        private GridItemSO _item;
        private SpriteRenderer _image;

        private static readonly PopupParameters PopupParams = new(
            disappearTimerMax: 1f,
            normalTextSize: 0.2f,
            criticalTextSize: 0.25f,
            normalTextColor: "FFFF00",
            criticalTextColor: "00FF00",
            moveXSpeed:0f,
            moveYSpeed:2f,
            minSortingLayer:10
        );

        
        public void Init(Grid<Cell> grid, int X, int Y, GridItemSO item)
        {
            _grid = grid;
            _pos = new Vector2(X, Y);
            _item = item;

            _image = GetComponentInChildren<SpriteRenderer>();
            _image.sprite = _item.Sprite;

            if (_blockerParticleSystem && !_item.Destructible)
                _blockerParticleSystem.gameObject.SetActive(true);
        }
        
        public void TryDestroyBlock(AudioClip sfxClip)
        {
            if (sfxClip)
                AudioSystem.Instance.PlaySFX(sfxClip);

            if (!_item.Destructible)
                return;
            
            ScoreSystem.Instance.ModifyScoreValue(this, ValSysInit.StrID_CurrentScore, _item.PointScore);

            TextPopup.Create(_popupPrefab, transform.position, _item.PointScore.ToString(),
                _item.PointScore > 3, PopupParams);
            
            GetComponentInChildren<Shatter>().shatter();
            Destroy(GetComponentInChildren<BoxCollider2D>());
            UtilityHelper_FunctionTimer.Create(() => Destroy(_image.gameObject), 0.8f);

            if (_blockerParticleSystem)
                _blockerParticleSystem.gameObject.SetActive(false);
            
            GridManager.CheckForEndGame();
        }
    }
}