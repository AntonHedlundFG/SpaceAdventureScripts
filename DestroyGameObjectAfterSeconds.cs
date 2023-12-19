using System.Collections;
using UnityEngine;

public class DestroyGameObjectAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _duration;

    private void Start() => StartCoroutine(DestroyAfterSeconds(_duration));

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
