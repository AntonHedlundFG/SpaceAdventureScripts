using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private GameObject deathScreen;

    //[SerializeField] private float initialHpVal;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerHealthBar.value = (float)playerHealth.maxHealth;
        playerHealthBar.maxValue = playerHealthBar.value;
        playerHealth.OnPlayerHealthChange.AddListener(UpdateHealthbar);
        
        
    }
    void OnDisable()
    {
        
        playerHealth.OnPlayerHealthChange.RemoveListener(UpdateHealthbar);
    }

    // Update is called once per frame
    void UpdateHealthbar(int newHealth)
    {
        playerHealthBar.value = (float)newHealth;
        playerHealthBar.maxValue = playerHealth.maxHealth;

        if(newHealth <= 0)
        {
            deathScreen.SetActive(true);
        }
        else
        {
            deathScreen.SetActive(false);
        }
    }
}
