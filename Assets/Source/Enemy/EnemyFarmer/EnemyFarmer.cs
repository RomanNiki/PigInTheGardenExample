using Source.Enemy.MovementTypes;
using Source.Interfaces;
using UnityEngine;

namespace Source.Enemy.EnemyFarmer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyFarmer : EnemyBase
    {
        [SerializeField] private Transform[] _checkPoints;
        private IMove _enemyMovement;
        private Vector3 _inputDirection;

        private void Awake()
        {
            _enemyMovement = new MoveToCheckPoints(_speed, _checkPoints, transform);
        }

        private void Update()
        {
            SetAnimator();
        }

        private void FixedUpdate()
        {
            Move();
        }

        protected override void SetAnimator()
        {
            _animator.SetFloat(InputMagnitude, _inputDirection.normalized.magnitude);
            _animator.SetFloat(Horizontal, _inputDirection.normalized.x);
            _animator.SetFloat(Vertical, _inputDirection.normalized.y);
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