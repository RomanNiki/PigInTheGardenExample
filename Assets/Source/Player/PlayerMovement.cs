using UnityEngine;

namespace Source.Player
{
    public sealed class PlayerMovement : ActorBase
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Bomb.Bomb _bomb;
        [SerializeField] private int _bombCount;
        [SerializeField] private bool _bombAutoExpand;
        private float _horizontal;
        private float _vertical;
        private float _inputMagnitude;
        private ObjectPool<Bomb.Bomb> _objectPool;

        protected override void Start()
        {
            base.Start();
            _objectPool = new ObjectPool<Bomb.Bomb>(_bomb, _bombCount){AutoExpand = _bombAutoExpand};
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }

        protected override void Update()
        {
            base.Update();
            InputHandler();
        }

        public void SetBomb()
        {
            if (_objectPool.HasFreeElement(out var bomb))
            {
                bomb.transform.position = transform.position;
            }
            else
            if (_bombAutoExpand)
            {
                var addedBomb = _objectPool.GetFreeElement();
                addedBomb.transform.position = transform.position;
            }
        }

        protected override void Move()
        {
            transform.position += new Vector3(_horizontal, _vertical, 0f) * _speed * Time.deltaTime;
        }

        public override void GetDamage()
        {
            _deathEvent?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void SetAnimator()
        {
            _animator.SetFloat(Params.InputMagnitude, _inputMagnitude);
            _animator.SetFloat(Params.Horizontal, _horizontal);
            _animator.SetFloat(Params.Vertical, _vertical);
        }

        private void InputHandler()
        {
            if (_joystick == null)
            {
                return;
            }
            
            _horizontal = _joystick.Horizontal;
            _vertical = _joystick.Vertical;
            _inputMagnitude = new Vector2(_horizontal, _vertical).magnitude;
        }
    }
}