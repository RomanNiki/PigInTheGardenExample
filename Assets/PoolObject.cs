using System.Collections;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;

    private void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
}
