using System;
using System.Collections;
using UnityEngine;

namespace Source.Bomb
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private float _timeToExplosion;
        [SerializeField] private LayerMask _levelMask;
        [SerializeField] private PoolObject _explosion;
        [SerializeField] private int _poolExplosionCount = 27;
        [SerializeField] private bool _poolAutoExpand = true;
        private ObjectPool<PoolObject> _explosionPool;
        private const float LifeTime = 0.3f;

        private void Start()
        {
            _explosionPool = new ObjectPool<PoolObject>(_explosion, _poolExplosionCount){AutoExpand = _poolAutoExpand};
        }

        private void OnEnable()
        {
            Invoke(nameof(Explode), _timeToExplosion);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, Vector2.up * _attackRange, Color.black);
            Debug.DrawRay(transform.position, Vector2.right* _attackRange, Color.green);
            Debug.DrawRay(transform.position, Vector2.down * _attackRange, Color.red);
            Debug.DrawRay(transform.position, Vector2.left * _attackRange, Color.blue);
        }

        private void Explode()
        {
            var explosion = _explosionPool.GetFreeElement();
            explosion.transform.position = transform.position;
            StartCoroutine(SpawnExplosion(Vector2.up));
            StartCoroutine( SpawnExplosion(Vector2.down));
            StartCoroutine(SpawnExplosion(Vector2.left));
            StartCoroutine(SpawnExplosion(Vector2.right));
            StartCoroutine(Deactivate());
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(LifeTime);
            gameObject.SetActive(false);
        }

        private IEnumerator SpawnExplosion(Vector2 direction)
        {
            Vector2 pos = transform.position;
            var range = (int) _attackRange;
            for (int i = 1; i < range; i++)
            {
                RaycastHit2D hit =
                Physics2D.Raycast(pos , direction, 
                    i, _levelMask);
                Debug.Log(i);
                if (!hit.collider)
                {
                    var expl = _explosionPool.GetFreeElement();
                    expl.transform.position = transform.position + (i * (Vector3) direction);
                }
                else
                {
                    var damageable =   hit.transform.GetComponent<IDamageable>();
                    damageable?.GetDamage();
                    break;
                }
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
