using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPartSystem : MonoBehaviour
{
    [SerializeField] private GameObject firstpartPrefab;
    [SerializeField] private GameObject seondpartPrefab;

    [SerializeField] private EventSO firstwingPickup;
    [SerializeField] private EventSO secondwingPickup; 
    [SerializeField] private EventSO firstwingReturn; 
    [SerializeField] private EventSO secondwingReturn; 


    void OnEnable()
    {
        firstwingPickup.Event.AddListener(firstAdd);
        secondwingPickup.Event.AddListener(secondAdd);
        firstwingReturn.Event.AddListener(returnFirst);
        secondwingReturn.Event.AddListener(returnSecond);
    }
    void OnDisable()
    {
        firstwingPickup.Event.RemoveListener(firstAdd);
        secondwingPickup.Event.RemoveListener(secondAdd);
        firstwingReturn.Event.RemoveListener(returnFirst);
        secondwingReturn.Event.RemoveListener(returnSecond);
    }

    private void firstAdd()
    {
        firstpartPrefab.SetActive(true);
    }
    private void secondAdd()
    {
        seondpartPrefab.SetActive(true);
    }
    private void returnFirst()
    {
       firstpartPrefab.SetActive(false);
    }
    private void returnSecond()
    {
       seondpartPrefab.SetActive(false);
    }


    
    
}
