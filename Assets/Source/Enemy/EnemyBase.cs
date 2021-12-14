using Source.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Source.Enemy
{
    public abstract class EnemyBase : AnimatedBase, IDamageable
    {
        [SerializeField] protected float _speed;
        public UnityEvent _deathEvent;
        protected abstract void Move();
        public abstract void GetDamage();
    }
}