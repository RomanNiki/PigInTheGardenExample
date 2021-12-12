using Source.Enemy.MovementTypes;
using Source.Interfaces;
using UnityEngine;

namespace Source.Enemy.EnemyFarmer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyFarmer : ActorBase
    {
        [SerializeField] private Transform[] _checkPoints;
        [SerializeField] private Sprite[] _sprites;
        private IMove _enemyMovement;
        private Vector3 _inputDirection;
        private SpriteRenderer _spriteRenderer;

        private readonly Vector2[] _vector2S =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        
        private void Awake()
        {
            _enemyMovement = new MoveToCheckPoints(_speed, _checkPoints, transform);
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
                if (Vector2.Dot(_inputDirection.normalized, _vector2S[i]) > 0.4f)
                {
                    _spriteRenderer.sprite = _sprites[i];
                }
            }
        }
        
        protected override void SetAnimator()
        {
            _animator.SetFloat(InputMagnitude, _inputDirection.normalized.magnitude);
        }

        protected override void Move()
        {
            _inputDirection = _enemyMovement.Move();
        }
        
        public override void GetDamage()
        { 
            _deathEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}