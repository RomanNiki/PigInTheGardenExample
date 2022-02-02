using Source.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Source
{
    public abstract class ActorBase : AnimatedBase, IDamageable
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected UnityEvent _deathEvent;
        public event UnityAction Death
        {
            add => _deathEvent.AddListener(value);
            remove => _deathEvent.RemoveListener(value);
        }
        protected abstract void Move();
        public abstract void GetDamage();

        protected virtual void FixedUpdate()
        {
            Move();
        }
    }
}