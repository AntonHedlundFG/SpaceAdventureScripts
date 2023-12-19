using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Health/Player Health")]
public class PlayerHealth : ScriptableObject
{
    [SerializeField] private int _currentHealth = 1;
    public int maxHealth = 1;

    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            if (_currentHealth > 0 && value <= 0)
            {
                OnPlayerDeath.Invoke(true);
            }

            if (_currentHealth <= 0 && value > 0)
            {
                OnPlayerDeath.Invoke(false);
            }
            _currentHealth = value;
            OnPlayerHealthChange.Invoke(_currentHealth);
        }
    }
    public UnityEvent<int> OnPlayerHealthChange;
    public UnityEvent<bool> OnPlayerDeath; //Invokes true upon death, false upon resurrection.
    public void SetMaxHealth(int maxHP)
    {
        
        maxHealth = maxHP;
        Health = maxHP;
        OnPlayerHealthChange.Invoke(_currentHealth);
    }
}
