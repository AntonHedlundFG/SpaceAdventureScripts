using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private EnemyHealth enemyHP;

    private void OnEnable() 
    {
        enemyHP.OnEnemyHealthChange.AddListener(ChangeValue);
    }

    private void OnDisable() 
    {
        enemyHP.OnEnemyHealthChange.RemoveListener(ChangeValue);
    }

    private void Start() 
    {
        hpBar.maxValue = enemyHP.Health;
        hpBar.value = enemyHP.Health;
    }

    public void ChangeValue(int newVal)
    {
        hpBar.value = newVal;
    }
}
