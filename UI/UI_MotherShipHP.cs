using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_MotherShipHP : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth motherHP;
    // Start is called before the first frame update
    void OnEnable()
    {
        slider.maxValue = (float)motherHP.maxHealth;
        slider.value = slider.maxValue;
        motherHP.OnPlayerHealthChange.AddListener(UpdateBar);
    }
    void OnDisable()
    {
        motherHP.OnPlayerHealthChange.RemoveListener(UpdateBar);
    }

    
    private void UpdateBar(int newVal)
    {
        slider.value = (float)newVal;
        slider.maxValue = motherHP.maxHealth;
    }
}
