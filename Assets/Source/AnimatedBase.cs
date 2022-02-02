using UnityEngine;

namespace Source
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimatedBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        public static class Params
        {
            public static readonly int InputMagnitude = Animator.StringToHash("Input");
            public static readonly int Horizontal = Animator.StringToHash("Horizontal");
            public static readonly int Vertical = Animator.StringToHash("Vertical");
        }
        
        private float _normValue;
        
        protected abstract void SetAnimator();
        
        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            SetAnimator();
            _normValue = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}