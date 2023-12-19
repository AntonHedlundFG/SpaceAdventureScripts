using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownManager : MonoBehaviour
{

    

    [SerializeField] private SimpleElementalGun gunScriptP1;
    [SerializeField] private SimpleElementalGun gunScriptP2;

    [SerializeField] private Image P1Icon;
    [SerializeField] private Image P2Icon;

    private float P1waitTime;
    private float P2waitTime;

    
    public void OnJoinedPlayer(SimpleElementalGun gunScript, int id)
    {
        
        if(id == 1)
        {
            gunScriptP1 = gunScript;
            gunScriptP1.OnSecondaryCooldownStarted.AddListener(P1activateCooldownUI);
        }
        else if(id == 2)
        {
            gunScriptP2 = gunScript;
            gunScriptP2.OnSecondaryCooldownStarted.AddListener(P2activateCooldownUI);
        }
        
        
    }
    void OnDisable()
    {
        gunScriptP1?.OnSecondaryCooldownStarted.RemoveListener(P1activateCooldownUI);
        gunScriptP2?.OnSecondaryCooldownStarted.RemoveListener(P2activateCooldownUI);
    }

    // Update is called once per frame
    void Update()
    {
        P1Icon.fillAmount += Time.deltaTime / P1waitTime;
        
        P2Icon.fillAmount += Time.deltaTime / P2waitTime;
        
    }

    private void P1activateCooldownUI(float time)
    {
        P1Icon.fillAmount = 0f;
        P1waitTime = time;
    }
    private void P2activateCooldownUI(float time)
    {
        P2Icon.fillAmount = 0f;
        P2waitTime = time;
    }
}
