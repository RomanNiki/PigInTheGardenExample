using UnityEngine;

namespace Source.Player
{
    public sealed class PlayerMovement : ActorBase
    {
        [SerializeField] private DynamicJoystick _dynamicJoystick;
        [SerializeField] private Bomb.Bomb _bomb;
        [SerializeField] private LayerMask _levelMask;
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
            if (Physics2D.CircleCast(transform.position, 0.2f, Vector2.zero))
            {
                if (_objectPool.HasFreeElement(out var bomb))
                {
                    bomb.transform.position = transform.position;
                }

                if (_bombAutoExpand)
                {
                    var addedBomb = _objectPool.GetFreeElement();
                    addedBomb.transform.position = transform.position;
                }
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
            if (_dynamicJoystick == null)
            {
                return;
            }
            
            _horizontal = _dynamicJoystick.Horizontal;
            _vertical = _dynamicJoystick.Vertical;
            _inputMagnitude = new Vector2(_horizontal, _vertical).magnitude;
        }
    }
}