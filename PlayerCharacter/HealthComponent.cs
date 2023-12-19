using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private PlayerHealth _healthSO;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private Animator _animator;

#if UNITY_EDITOR
    [SerializeField] private bool _doTestKillPlayer = false;
#endif

    public bool IsDead;

    private void Awake()
    {
        _healthSO?.SetMaxHealth(_settings.MaxHealth);
        IsDead = false;
        
        _healthSO?.OnPlayerDeath.AddListener(DeathStatusChange);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (_doTestKillPlayer)
        {
            _doTestKillPlayer = false;
            _healthSO.Health = 0;
        }
    }
#endif

    private void OnDisable()
    {
        _healthSO?.OnPlayerDeath.RemoveListener(DeathStatusChange);
    }

    public void TakeDamage(int dmg)
    {
        _healthSO.Health -= dmg;
    }

    public void Resurrect()
    {
        _healthSO.Health = _settings.ResurrectHealth;
    }

    public void DeathStatusChange(bool status)
    {
        IsDead = status;
        
        if (_animator != null)
            _animator.SetBool("isDowned", status);
    }

    public PlayerHealth GetPlayerHealth() { return _healthSO; }
}
