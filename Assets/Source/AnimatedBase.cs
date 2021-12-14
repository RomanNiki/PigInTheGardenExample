using UnityEngine;

namespace Source
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimatedBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        protected static readonly int InputMagnitude = Animator.StringToHash("Input");
        protected static readonly int Horizontal = Animator.StringToHash("Horizontal");
        protected static readonly int Vertical = Animator.StringToHash("Vertical");
        
        protected abstract void SetAnimator();
        
        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
        }
    }
}