using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cooldown : MonoBehaviour
{

    [SerializeField] private SimpleElementalGun _gunScript;
    [SerializeField] private Image icon;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        _gunScript = transform.parent.GetComponentInChildren<SimpleElementalGun>();
        _gunScript?.OnSecondaryCooldownStarted.AddListener(activateCooldown);   
    }
    void OnDisable()
    {
        _gunScript?.OnSecondaryCooldownStarted.RemoveListener(activateCooldown);   
    }
    void Update()
    {
        if(waitTime > 0)
        {
            icon.fillAmount += Time.deltaTime / waitTime;
        }
        
    }

    private void activateCooldown(float time)
    {
        icon.fillAmount = 0f;
        waitTime = time;
    }
}
