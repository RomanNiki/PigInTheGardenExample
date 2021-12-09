using System;
using UnityEngine;

namespace Source.Enemy.EnemyFarmer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyFarmer : ActorBase
    {
        [SerializeField] private Transform[] _checkPoints;
        [SerializeField] private Sprite[] _sprites;
        private int _currentCheckPoint;
        private Vector3 _input;
        private SpriteRenderer _spriteRenderer;

        private Vector2[] _vector2s = new[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void SetAnimator()
        {
           _animator.SetFloat(InputMagnitude, _input.normalized.magnitude);
        }

        private void Update()
        {
            SetAnimator();
        }

        private void FixedUpdate()
        {
            Move();
            RotateCharacter();
        }

        private void RotateCharacter()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Vector2.Dot(_input.normalized, _vector2s[i]) > 0.4f)
                {
                    _spriteRenderer.sprite = _sprites[i];
                }
            }
        }

        protected override void Move()
        {
            if (((Vector2)_checkPoints[_currentCheckPoint].position - (Vector2)transform.position).magnitude > 0.1f)
            {
                _input = (_checkPoints[_currentCheckPoint].position -
                         transform.position).normalized * _speed * Time.deltaTime;
                transform.position += _input;
            }
            else
            {
                if (_currentCheckPoint < _checkPoints.Length - 1)
                {
                    _currentCheckPoint++;
                }
                else
                {
                    _currentCheckPoint = 0;
                }
            }
        }

        public override void GetDamage()
        {
            Destroy(gameObject);
        }
    }
}