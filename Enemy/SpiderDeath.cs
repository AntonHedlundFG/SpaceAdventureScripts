using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDeath : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SpiderAnimation _spiderAnimation;
    [SerializeField] private Animator _animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Debug.Log("asdf");
        _animator.SetTrigger("deathTrigger");
        _rigidbody.isKinematic = false;
        _spiderAnimation.enabled = false;
    }
}
