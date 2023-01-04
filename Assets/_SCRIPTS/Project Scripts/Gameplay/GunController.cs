using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace CasualGame
{
    public class GunController : MonoBehaviour
    {
        [Title("Gun")]
        [SerializeField, Required] private KeyCode _shootKey = KeyCode.Mouse0;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunAimPoint;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunShotIndicator1;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunShotIndicator2;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunShotIndicator3;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunPowerIndicator1;
        [SerializeField, Required, SceneObjectsOnly] private Image _gunPowerIndicator2;
        [SerializeField, Required, SceneObjectsOnly] private Transform _gunAnchorPoint;
        [SerializeField, Range(1f, 160f)] private float _gunAngleRange;
        [SerializeField, Range(0f, 10f)] private float _shotDelay = 1.5f;
        [SerializeField, MinMaxSlider(0.9f, 10f, true)] private Vector2 _shotPowerRange = new(0.9f,2.3f);
        [SerializeField, MinMaxSlider(0.2f, 10f, true)] private Vector2 _shotPowerStrength = new(3f,9f);
        [SerializeField, Range(0.1f, 1f)] private float _shotStrengthMultiplier = 1f;
        [SerializeField, AssetsOnly, Required] private AudioClip[] _gunShotSfx; 
        
        [Title("Projectile")]
        [SerializeField, Required, PreviewField, AssetsOnly] private GameObject _projectilePrefab;
        [SerializeField, Required, SceneObjectsOnly] private Transform _projectileParentObj;
        [SerializeField, Range(0, 30)] private int _projectileBounces;

        private bool _isActive = true;
        private Camera _camera;
        private LineRenderer _lr;
        private Vector3 _inputPosition;
        private Vector3 _inputDirection;
        private Vector3 _clampedTargetPosition;
        private Vector3 _clampedTargetDirection;
        private float _shotTimer;
        private float _shotSpeed;
        private float _distanceToAnchor;
        


        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _camera = Camera.main;
            Vector3 pos = _gunAnchorPoint.position;
            pos.Set(pos.x, pos.y, 0);
            
            _gunPowerIndicator1.fillAmount = 0;
            _gunPowerIndicator2.fillAmount = 0;
            
            #if !UNITY_EDITOR_WIN || !UNITY_STANDALONE_WIN
            _gunAimPoint.enabled = false;
            #else
            _gunAimPoint.enabled = true;
            #endif
        }

        private void Start()
        {
            PauseManager.OnPaused += ToggleGunControl;
            PauseManager.OnResumed += ToggleGunControl;
        }

        private void OnDestroy()
        {
            PauseManager.OnPaused -= ToggleGunControl;
            PauseManager.OnResumed -= ToggleGunControl;
        }

        private void ToggleGunControl(object s, EventArgs e)
        {
            _isActive = !_isActive;
            #if UNITY_EDITOR
            Debug.Log("Gun active: " + _isActive);
            #endif
        }
        
        private void Update()
        {
            if (!_isActive)
                return;
            
            if (PauseManager.IsPaused() || GameManager.IsGameCompleted)
                return;
            
            if (_shotTimer < _shotDelay)
                _shotTimer += Time.deltaTime;

            _gunShotIndicator1.fillAmount = _shotTimer / _shotDelay;
            _gunShotIndicator2.fillAmount = _shotTimer / _shotDelay;
            _gunShotIndicator3.fillAmount = _shotTimer / _shotDelay;

            CalculateProjectileValues();
            
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            _gunAimPoint.transform.position = _inputPosition;
            #endif

            HandleGunInput();
        }

        private void HandleGunInput()
        {
            if (Input.GetKeyDown(_shootKey))
            {
                _lr.enabled = true;
                #if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
                _gunAimPoint.transform.position = _inputPosition;
                _gunAimPoint.enabled = true;
                #endif
            }
            else if (Input.GetKey(_shootKey))
            {
                #if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
                _gunAimPoint.transform.position = _inputPosition;
                #endif
            }
            else if (Input.GetKeyUp(_shootKey))
            {
                _lr.enabled = false;
                ShootProjectile(_shotSpeed);
                #if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
                _gunAimPoint.enabled = false;
                #endif
            }
        }

        private void CalculateProjectileValues()
        {
            Vector3 anchorPos = _gunAnchorPoint.position;
            _inputPosition = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
            _inputDirection = (Vector2)(_inputPosition - anchorPos).normalized;
            float dot = Vector2.Dot(_inputDirection, Vector2.up);
            
            _distanceToAnchor = Vector2.Distance(anchorPos, _inputPosition);
            _shotSpeed = Mathf.Clamp(ConvertRange(_shotPowerRange, _shotPowerStrength,
                _distanceToAnchor), _shotPowerStrength.x, _shotPowerStrength.y);
            _shotSpeed *= _shotStrengthMultiplier;

            _clampedTargetPosition = _inputPosition;
            _clampedTargetDirection = _inputDirection;
            
            if (_distanceToAnchor < _shotPowerRange.x)
                _clampedTargetPosition = anchorPos + (_clampedTargetDirection * _shotPowerRange.x);
            else if (_distanceToAnchor > _shotPowerRange.y)
                _clampedTargetPosition = anchorPos + (_clampedTargetDirection * _shotPowerRange.y);
            else
            {
                _clampedTargetPosition = anchorPos + _clampedTargetDirection *
                    Mathf.Clamp(_distanceToAnchor, _shotPowerRange.x, _shotPowerRange.y);
            }
            
            int aimAngle = UtilityHelper.GetAngleFromVector180(Vector2.up);
            int angle = UtilityHelper.GetAngleFromVector180(_inputPosition - _gunAnchorPoint.position);
            int angleDifference = (angle - aimAngle);
            if (angleDifference > 180) angleDifference -= 360;
            if (angleDifference < -180) angleDifference += 360;
            if (!(Math.Abs(angleDifference - _gunAngleRange / 2f) < 0.1f))
                Debug.DrawRay(_gunAnchorPoint.position,
                    2 * new Vector3(0f, 0f, Math.Abs(angleDifference - _gunAngleRange / 2f)));
            else if (!(Math.Abs(angleDifference - -_gunAngleRange / 2f) < 0.1f))
                Debug.DrawRay(_gunAnchorPoint.position,
                    2 * new Vector3(0f, 0f, Math.Abs(angleDifference - -_gunAngleRange / 2f)));

            _lr.positionCount = 2;
            _lr.SetPosition(0, anchorPos);
            _lr.SetPosition(1, _clampedTargetPosition);
            
            float powerValue = ConvertRange(_shotPowerStrength, new Vector2(0, 1), _shotSpeed);
            _gunPowerIndicator1.fillAmount = powerValue;
            _gunPowerIndicator2.fillAmount = powerValue;
            
            #if UNITY_EDITOR
            UtilityHelper.DebugDrawCircle(_inputPosition, 0.1f, Color.red, 0.01f, 10);

            if (UtilityHelper.IsPositionInsideFov(_gunAnchorPoint.position, Vector2.up,
                    _inputPosition, _gunAngleRange)) _clampedTargetDirection = _inputDirection;
            else
                Debug.DrawLine(_inputPosition, _clampedTargetPosition, Color.red, 0.01f);
            Vector3 offsetMin = _inputDirection * _shotPowerRange.x;
            Vector3 offsetMax = _inputDirection * _shotPowerRange.y;
            UtilityHelper.DebugDrawCircle(anchorPos + offsetMin, 0.08f, Color.magenta, 0.01f, 4);
            UtilityHelper.DebugDrawCircle(anchorPos + offsetMax, 0.08f, Color.blue, 0.01f, 4);
            Debug.DrawLine(anchorPos + offsetMin, anchorPos + offsetMax, Color.green, 0.01f);
            #endif
        }

        private static float ConvertRange(Vector2 input, Vector2 output, float value)
        {
            float convFactor =
                Mathf.Abs(output.y - output.x) /
                Mathf.Abs(input.y - input.x);
            return output.x + (convFactor * (value - input.x));
        }

        private void ShootProjectile(float speed)
        {
            if (!_projectilePrefab || _shotTimer < _shotDelay)
                return;

            _shotTimer = 0f;

            GameObject projectile = Instantiate(
                _projectilePrefab,
                _gunAnchorPoint.position,
                Quaternion.identity,
                _projectileParentObj
            );

            projectile.GetComponent<Projectile>().Shoot(
                _clampedTargetDirection.normalized,
                speed,
                _projectileBounces
            );
            
            if (_gunShotSfx.Length > 0)
                AudioSystem.Instance.PlaySFX(_gunShotSfx[Random.Range(0, _gunShotSfx.Length)]);
            ScoreSystem.Instance.ModifyScoreValue(this, ValSysInit.StrID_ShotsTaken, 1);
        }
    }
}