using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_resButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerSettings _settings;
    public LayerMask _playerMask;
    public LayerMask _motherShipMask;
    
    [SerializeField] private Sprite A_button_sprite;
    private HealthComponent hc;

    private Image buttonImg;
    [SerializeField] private Image progressOverlay;

    public GameObject thisPlayer;

    private bool returnTime = false;
    [SerializeField] private EventSO onFirstwingPickup;
    [SerializeField] private EventSO onSecondwingPickup;
    [SerializeField] private EventSO onfirstReturn;
    [SerializeField] private EventSO onsecondReturn;

    [SerializeField] private GameObject ShipReturnUI;

    void Start()
    {
        buttonImg = this.GetComponent<Image>();
    }

    void OnEnable()
    {
        onFirstwingPickup.Event.AddListener(returnPartTime);
        onSecondwingPickup.Event.AddListener(returnPartTime);
        onfirstReturn.Event.AddListener(doneReturning);
        onsecondReturn.Event.AddListener(doneReturning);
    }
    void OnDisable()
    {
        onFirstwingPickup.Event.RemoveListener(returnPartTime);
        onSecondwingPickup.Event.RemoveListener(returnPartTime);
        onfirstReturn.Event.RemoveListener(doneReturning);
        onsecondReturn.Event.RemoveListener(doneReturning);
    }
    // Update is called once per frame
    void Update()
    {
        if(returnTime)
        {
            if(FindMothership())
            {
                ShipReturnUI.SetActive(true);
            }else
            {
                ShipReturnUI.SetActive(false);
            }
        }
        else
        {
            ShipReturnUI.SetActive(false);
        }

        if(FindDeadPlayer())
        {
            DisplayButton(A_button_sprite);
        }else
        {
            progressOverlay.fillAmount = 0f;
            buttonImg.color = new Color(buttonImg.color.r,buttonImg.color.g,buttonImg.color.b,0f);
        }

    }
    

    private bool FindDeadPlayer()
    {
        Collider[] players = Physics.OverlapSphere(thisPlayer.transform.position, _settings.ResourceUseRadius, _playerMask);
        foreach (Collider cld in players)
        {
            if (cld.TryGetComponent<HealthComponent>(out hc) && hc.gameObject != thisPlayer && hc.IsDead)
            {
                return true;
            }
        }
        return false;
    }
    private bool FindMothership()
    {
        Collider[] mothership = Physics.OverlapSphere(thisPlayer.transform.position, _settings.ResourceUseRadius, _motherShipMask);
        foreach (Collider cld in mothership)
        {
            return true;
        }
        return false;
    }
    

    public void DisplayButton(Sprite icon)
    {
        if(thisPlayer.GetComponent<PlayerResurrectAction>() != null)
        {
            progressOverlay.fillAmount = thisPlayer.GetComponent<PlayerResurrectAction>().resPercentage;
        }
        buttonImg.color = new Color(buttonImg.color.r,buttonImg.color.g,buttonImg.color.b,1f);
        buttonImg.sprite = icon;
    }

    private void returnPartTime()
    {
        returnTime = true;
    }
    private void doneReturning()
    {
        returnTime = false;
    }

}
