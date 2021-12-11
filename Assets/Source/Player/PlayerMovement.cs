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

        private void Update()
        {
            Move();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.2f);
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
            InputHandler();
            SetAnimator();
            transform.position += new Vector3(_horizontal, _vertical, 0f) * _speed * Time.deltaTime;
        }

        public override void GetDamage()
        {
            _deathEvent?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void SetAnimator()
        {
            _animator.SetFloat(InputMagnitude, _inputMagnitude);
            _animator.SetFloat(Horizontal, _horizontal);
            _animator.SetFloat(Vertical, _vertical);
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