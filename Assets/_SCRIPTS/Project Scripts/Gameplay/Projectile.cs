using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;
using Random = UnityEngine.Random;

namespace CasualGame
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField, Required] private AudioClip[] _gemHitClip;
        [SerializeField, Required] private AudioClip[] _blockerHitClip;
        [SerializeField, Required] private AudioClip[] _wallHitClip;
        
        private Vector3 _currentDirection = Vector3.up;
        private float _currentSpeed = 1f;
        private float _currentBounceCount;
        private float _maxBounceCount;

        private Rigidbody2D _rb;

        private static readonly string WALL = "Wall";
        private static readonly string BLOCK = "Block";


        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void Start()
        {
            PauseManager.OnPaused += OnPause;
            PauseManager.OnResumed += OnResume;
        }
        
        private void OnDestroy()
        {
            PauseManager.OnPaused -= OnPause;
            PauseManager.OnResumed -= OnResume;
        }

        public void Shoot(Vector3 direction, float speed, int maxBounces)
        {
            _currentDirection = direction;
            _currentSpeed = speed;
            _maxBounceCount = maxBounces;

            _rb.velocity = _currentDirection * _currentSpeed;
        }

        private void Update()
        {
            if (GameManager.IsGameCompleted)
                Destroy(gameObject);
            
            _rb.velocity = _currentDirection * _currentSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.transform.CompareTag(WALL) && !other.transform.CompareTag(BLOCK))
                return;
            
            Ricochet(other.GetContact(0).normal);
            
            if (other.transform.CompareTag(BLOCK) && other.transform.parent.TryGetComponent(out Block block))
            {
                block.TryDestroyBlock(block.IsDestructible ?
                    _gemHitClip[Random.Range(0, _gemHitClip.Length)] :
                    _blockerHitClip[Random.Range(0, _blockerHitClip.Length)]
                );
            }
            else if (other.transform.CompareTag(WALL))
                AudioSystem.Instance.PlaySFX(_wallHitClip[Random.Range(0, _wallHitClip.Length)]);

            if (_currentBounceCount >= _maxBounceCount)
                Destroy(gameObject);
        }

        private void Ricochet(Vector3 contactSurfaceNormal)
        {
            if (_currentBounceCount >= _maxBounceCount)
                return;

            _currentDirection = Vector2.Reflect(_currentDirection, contactSurfaceNormal);
            _rb.velocity = _currentDirection * _currentSpeed;
            _currentBounceCount++;
        }

        private void OnPause(object s, EventArgs e) => _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        private void OnResume(object s, EventArgs e)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.velocity = _currentDirection * _currentSpeed;
        }
    }
}