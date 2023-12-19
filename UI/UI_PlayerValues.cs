using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerValues : MonoBehaviour
{

    [SerializeField] private Resource[] resources;
    [SerializeField] private Transform playerCenterPiece;
    [SerializeField] private GameObject playerResourcePrefab;

    [SerializeField] private float margin;

    private List<GameObject> activeResourceElements = new List<GameObject>();


    void OnEnable()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            resources[i].OnAmountChanged.AddListener(UpdateUIValues);
        }
    }
    void OnDisable()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            resources[i].OnAmountChanged.RemoveListener(UpdateUIValues);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            float offsetF = ((playerResourcePrefab.GetComponent<RectTransform>().sizeDelta.y * playerResourcePrefab.GetComponent<RectTransform>().localScale.y) + margin) * i;
            Vector3 offsetPos = new Vector3(playerCenterPiece.position.x, playerCenterPiece.position.y + offsetF, 0f);

            GameObject currentPlayerResource = Instantiate(playerResourcePrefab, offsetPos, Quaternion.identity, playerCenterPiece.transform);
            currentPlayerResource.GetComponent<UI_PlayerResource>().Icon.sprite = resources[i].Icon2D;
            currentPlayerResource.GetComponent<UI_PlayerResource>().amountText.color = resources[i].Color;
            activeResourceElements.Add(currentPlayerResource);
        }
    }

    
    void UpdateUIValues(int newValue)
    {
        //update UI
        for(int i = 0; i < resources.Length; i++)
        {
            activeResourceElements[i].GetComponent<UI_PlayerResource>().amountText.text = newValue.ToString();
        }
    }
}
