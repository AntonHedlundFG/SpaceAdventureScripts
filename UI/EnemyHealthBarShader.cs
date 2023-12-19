using UnityEngine;

public class EnemyHealthBarShader : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private EnemyHealth _enemyHP;

    private int _maxHealth;

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void OnEnable() 
    {
        _enemyHP.OnEnemyHealthChange.AddListener(ChangeValue);
    }

    private void OnDisable() 
    {
       _enemyHP.OnEnemyHealthChange.RemoveListener(ChangeValue);
    }

    private void Start()
    {
        if (_enemyHP == null)
        {
            _enemyHP = GetComponentInParent<EnemyHealth>();
        }

        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_enemyHP == null || _renderer == null)
        {
            enabled = false;
            return;
        }

        _maxHealth = _enemyHP.Health;
        ChangeValue(_maxHealth);
    }

    public void ChangeValue(int newVal)
    {
        _renderer.material.SetFloat("_HPValue", (float)(newVal)/(float)(_maxHealth));
    }
}
