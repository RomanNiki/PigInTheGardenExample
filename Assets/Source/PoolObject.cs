using System.Collections;
using UnityEngine;

namespace Source
{
    public class PoolObject : MonoBehaviour
    {
        [SerializeField] protected float _lifeTime = 1f;

        protected virtual void OnEnable()
        {
            StartCoroutine(Deactivate());
        }

        protected virtual IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
