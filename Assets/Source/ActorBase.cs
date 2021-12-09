using UnityEngine;
using UnityEngine.Events;

namespace Source
{
    [RequireComponent(typeof(Animator))]
    public abstract class ActorBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected Animator _animator;
        public UnityEvent _deathEvent;
        protected static readonly int InputMagnitude = Animator.StringToHash("Input");
        protected static readonly int Horizontal = Animator.StringToHash("Horizontal");
        protected static readonly int Vertical = Animator.StringToHash("Vertical");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected abstract void SetAnimator();
        protected abstract void Move();
        public abstract void GetDamage();
    }
}