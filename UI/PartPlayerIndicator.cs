using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartPlayerIndicator : MonoBehaviour
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private GameObject[] displayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _settings.ResourceUseRadius, playerLayerMask);
        if(colliders.Length == 1)
        {
            showDisplayText("HOLD 1/2", colliders);
        
        }
        else if(colliders.Length >= 2)
        {
            showDisplayText("HOLD 2/2", colliders);
        }
        else
        {
            hideDisplayText();
        }
    }
    private void showDisplayText(string text, Collider[] players)
    {

        displayText[0].GetComponent<TextMeshPro>().text = text;
        displayText[1].GetComponent<TextMeshPro>().text = text;

        if(players[0].transform.name.Contains("One"))
        {
            displayText[0].SetActive(true);
            displayText[0].transform.rotation = Quaternion.LookRotation(displayText[0].transform.position - players[0].transform.position);
        }
        else if(players[0].transform.name.Contains("Two"))
        {
            displayText[1].SetActive(true);
            displayText[1].transform.rotation = Quaternion.LookRotation(displayText[1].transform.position - players[0].transform.position);
        }
        
        
        if(players.Length >= 2)
        {
            if(players[1].transform.name.Contains("One"))
            {
                displayText[0].SetActive(true);
                displayText[0].transform.rotation = Quaternion.LookRotation(displayText[0].transform.position - players[1].transform.position);
            }
            else if(players[1].transform.name.Contains("Two"))
            {
                displayText[1].SetActive(true);
                displayText[1].transform.rotation = Quaternion.LookRotation(displayText[1].transform.position - players[1].transform.position);
            }

        }else
        {
            if(players[0].transform.name.Contains("One"))
            {
                displayText[1].SetActive(false);
            }
            else if(players[0].transform.name.Contains("Two"))
            {
                displayText[0].SetActive(false);
                
            }
        }
        
       

    }
    private void hideDisplayText()
    {
        displayText[0].SetActive(false);
        displayText[1].SetActive(false);
    }

    
}
