using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectHighlight : MonoBehaviour
{

    //public bool lookedAt;
    [SerializeField] private Material outline;
    [SerializeField] private Material noOutline;
    [SerializeField] private Renderer rend;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private PlayerSettings _settings;

    [SerializeField] private GameObject displayTextP1;
    [SerializeField] private GameObject displayTextP2;

    private float cd;
    // Start is called before the first frame update
    void Start()
    {
        Material[] mats = rend.materials;
        //outline = mats[2];
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _settings.ResourceUseRadius, playerLayerMask);
        if(colliders.Length == 1)
        {
            
            Material[] mats = rend.materials;
            mats[2] = outline;
            rend.materials = mats;

            showDisplayText("1/2", colliders);
        
        }else if(colliders.Length >= 2)
        {
            showDisplayText("2/2", colliders);
        }
        else
        {
            hideDisplayText();
            Material[] mats = rend.materials;
            mats[2] = noOutline;
            rend.materials = mats;
            // Debug.Log("no player"); //Too many calls
        }
        

    }

    

    private void showDisplayText(string text, Collider[] players)
    {
        


        displayTextP1.GetComponent<TextMeshPro>().text = text;
        displayTextP2.GetComponent<TextMeshPro>().text = text;

        if(players[0].transform.name.Contains("One"))
        {
            displayTextP1.SetActive(true);
            displayTextP1.transform.rotation = Quaternion.LookRotation(displayTextP1.transform.position - players[0].transform.position);
        }
        else if(players[0].transform.name.Contains("Two"))
        {
            displayTextP2.SetActive(true);
            displayTextP2.transform.rotation = Quaternion.LookRotation(displayTextP1.transform.position - players[0].transform.position);
        }

        if(players.Length >= 2)
        {
            if(players[1].transform.name.Contains("One"))
            {
                displayTextP1.SetActive(true);
                displayTextP1.transform.rotation = Quaternion.LookRotation(displayTextP1.transform.position - players[1].transform.position);
            }
            else if(players[1].transform.name.Contains("Two"))
            {
                displayTextP2.SetActive(true);
                displayTextP2.transform.rotation = Quaternion.LookRotation(displayTextP1.transform.position - players[1].transform.position);
            }

        }else
        {
            if(players[0].transform.name.Contains("One"))
            {
                displayTextP2.SetActive(false);
            }
            else if(players[0].transform.name.Contains("Two"))
            {
                displayTextP1.SetActive(false);
                
            }
        }
        
       

    }
    private void hideDisplayText()
    {
        displayTextP1.SetActive(false);
        displayTextP2.SetActive(false);
    }
}
