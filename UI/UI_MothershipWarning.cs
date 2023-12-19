using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MothershipWarning : MonoBehaviour
{
    [SerializeField] private DayNightData DayNightSO;
    [SerializeField] private ObjectiveSystem objSystemScript;
    [SerializeField] private EventSO lastStandEvent;

    private bool isLastStand = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        DayNightSO.OnNightTime.AddListener(OnNight);
        DayNightSO.OnDayStart.AddListener(OnDay);
        lastStandEvent.Event.AddListener(lastS);
    }
    void OnDisable()
    {
        DayNightSO.OnNightTime.RemoveListener(OnNight);
        DayNightSO.OnDayStart.RemoveListener(OnDay);
        lastStandEvent.Event.RemoveListener(lastS);
    }
    // Update is called once per frame
    private void OnNight()
    {
        objSystemScript.Icon.color = new Color(objSystemScript.Icon.color.r, objSystemScript.Icon.color.g, objSystemScript.Icon.color.b, 0f);
        objSystemScript.Title.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 0f);
        objSystemScript.Description.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 0f);
        
        this.GetComponent<Image>().enabled = true;
    }
    private void OnDay()
    {
        if(isLastStand == false)
        {
            objSystemScript.Icon.color = new Color(objSystemScript.Icon.color.r, objSystemScript.Icon.color.g, objSystemScript.Icon.color.b, 1f);
            objSystemScript.Description.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 1f);
            objSystemScript.Title.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 1f);
            this.GetComponent<Image>().enabled = false;
        }
        
    }

    private void lastS()
    {
        isLastStand = true;
        objSystemScript.Icon.color = new Color(objSystemScript.Icon.color.r, objSystemScript.Icon.color.g, objSystemScript.Icon.color.b, 0f);
        objSystemScript.Title.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 0f);
        objSystemScript.Description.color = new Color(objSystemScript.Title.color.r, objSystemScript.Title.color.g, objSystemScript.Title.color.b, 0f);
        
        this.GetComponent<Image>().enabled = true;
    }
}
