using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeverInteractIndicator : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    

    [SerializeField] private Sprite ButtonIcon;
    [SerializeField] private string InstructionMessage;
    [SerializeField] private string WhenHeldInstructionMessage;
    
    [SerializeField] private TextMeshProUGUI textObj;
    [SerializeField] private Image ButtonObj;

    [SerializeField] private bool isHold;

    private PlayerUseButton _playerUseButtonScript;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    void OnEnable()
    {
        if(this.GetComponent<PlayerUseButton>() != null && isHold == false)
        {
            _playerUseButtonScript = this.GetComponent<PlayerUseButton>();
            _playerUseButtonScript.OnSingleUseEventStop.AddListener(DisableUI);
            textObj.text = InstructionMessage;
            ButtonObj.sprite = ButtonIcon;
        }
        else if(this.GetComponent<PlayerUseButton>() != null && isHold == true)
        {
            _playerUseButtonScript = this.GetComponent<PlayerUseButton>();
            _playerUseButtonScript.OnComboUseEvent.AddListener(heldShow);
            textObj.text = InstructionMessage;
            ButtonObj.sprite = ButtonIcon;
        }
        
    }
    void OnDisable()
    {
        _playerUseButtonScript.OnSingleUseEventStop.RemoveListener(DisableUI);
    }

    // Update is called once per frame
    private void heldShow(bool held)
    {
        if(held)
        {
            textObj.text = WhenHeldInstructionMessage;
        }else
        {
            textObj.text = InstructionMessage;
        }
        

    }

    private void DisableUI()
    {
        canvas.SetActive(false);
    }
}
