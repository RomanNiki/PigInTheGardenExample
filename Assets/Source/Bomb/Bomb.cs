using System.Collections;
using Source.Interfaces;
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
        [SerializeField] private float _verticalDisplacement = 0.11f ;
        private static ObjectPool<PoolObject> _explosionPool;
        private bool Exploded => !_bombCollider.enabled;

        private void Awake()
        {
            _explosionPool = new ObjectPool<PoolObject>(_explosion, _poolExplosionCount){AutoExpand = _poolAutoExpand};
        }

        protected override void OnEnable()
        {
            _bombCollider.enabled = true;
            StartCoroutine(Explode(_timeToExplosion));
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Debug.DrawRay(position, new Vector2(_verticalDisplacement, Vector2.up.y * _attackRange), Color.black);
            Debug.DrawRay(position, Vector2.right* _attackRange, Color.green);
            Debug.DrawRay(position, new Vector2(-_verticalDisplacement, Vector2.down.y * _attackRange), Color.red);
            Debug.DrawRay(position, Vector2.left * _attackRange, Color.blue);
        }

        private IEnumerator Explode(float timeToExplosion)
        {
            yield return new WaitForSeconds(timeToExplosion);
            _bombCollider.enabled = false;
            var explosion = _explosionPool.GetFreeElement();
            explosion.transform.position = transform.position;
            yield return SpawnExplosion(new Vector2(_verticalDisplacement, Vector2.up.y));
            yield return SpawnExplosion(new Vector2(-_verticalDisplacement, Vector2.down.y));
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
            if (Exploded) return;
            StopAllCoroutines();
            StartCoroutine(Explode(0f));
        }
    }
}
