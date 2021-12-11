using System.Collections;
using UnityEngine;

namespace Source.Bomb
{
    public sealed class Bomb : PoolObject, IDamageable
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private float _timeToExplosion;
        [SerializeField] private LayerMask _levelMask;
        [SerializeField] private PoolObject _explosion;
        [SerializeField] private int _poolExplosionCount = 9;
        [SerializeField] private bool _poolAutoExpand = true;
        [SerializeField] private Collider2D _bombCollider;
        private static ObjectPool<PoolObject> _explosionPool;
        private bool _exploded;

        private void Awake()
        {
            _explosionPool = new ObjectPool<PoolObject>(_explosion, _poolExplosionCount){AutoExpand = _poolAutoExpand};
        }

        protected override void OnEnable()
        {
            _exploded = false;
            _bombCollider.enabled = true;
            StartCoroutine(Explode(_timeToExplosion));
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Debug.DrawRay(position, Vector2.up * _attackRange, Color.black);
            Debug.DrawRay(position, Vector2.right* _attackRange, Color.green);
            Debug.DrawRay(position, Vector2.down * _attackRange, Color.red);
            Debug.DrawRay(position, Vector2.left * _attackRange, Color.blue);
        }

        private IEnumerator Explode(float timeToExplosion)
        {
            yield return new WaitForSeconds(timeToExplosion);
            _exploded = true;
            _bombCollider.enabled = false;
            var explosion = _explosionPool.GetFreeElement();
            explosion.transform.position = transform.position;
            yield return SpawnExplosion(Vector2.up);
            yield return SpawnExplosion(Vector2.down);
            yield return SpawnExplosion(Vector2.left);
            yield return SpawnExplosion(Vector2.right);
            base.OnEnable();
        }

        private IEnumerator SpawnExplosion(Vector2 direction)
        {
            Vector2 pos = transform.position;
            for (var i = 1; i < _attackRange; i++)
            {
                var hit =
                Physics2D.CircleCast(pos , 0.2f ,direction, 
                    i, _levelMask);
                if (!hit.collider)
                {
                    var explosion = _explosionPool.GetFreeElement();
                    explosion.transform.position = pos + (i *  direction);
                }
                else
                {
                    var damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.GetDamage();
                    break;
                }
                yield return new WaitForSeconds(.05f);
            }
        }

        public void GetDamage()
        {
            if (_exploded) return;
            StopAllCoroutines();
            StartCoroutine(Explode(0f));
        }
    }
}
