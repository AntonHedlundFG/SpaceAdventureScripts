using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UNUSED SCRIPT
public class UI_ResourceSystem : MonoBehaviour
{
    [SerializeField] private Resource[] resources;
    [SerializeField] private Transform centerPiece;
    [SerializeField] private GameObject resourceSliderPrefab;

    [SerializeField] private WinConditionManager wcon;   

    [SerializeField] private float margin = 50f;

    private List<Slider> activeResourceSliders = new List<Slider>();
    
    void OnEnable()
    {
        //define and set positions for UI
        
        if(GameObject.Find("WinConditionManager") != null)
        {
            wcon = GameObject.Find("WinConditionManager").GetComponent<WinConditionManager>();
        }else
        {
            Debug.LogWarning("Plase make sure the winconditionmanager is in the scene!");
        }
        

        for(int i = 0; i < resources.Length; i++)
        {
            resources[i].OnAmountChanged.AddListener(UpdateResourceSlider);
            
        }
    }
    void OnDisable()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            resources[i].OnAmountChanged.RemoveListener(UpdateResourceSlider);
        }
    }
    
    void Start()
    {
        for(int i = 0; i < resources.Length; i++)
        {

            float offsetF = ((resourceSliderPrefab.GetComponent<RectTransform>().sizeDelta.y * resourceSliderPrefab.GetComponent<RectTransform>().localScale.y)/2 + margin) * i;
            Vector3 offsetPos = new Vector3(centerPiece.position.x, centerPiece.position.y + offsetF, 0f);
            GameObject currentSliderObj = Instantiate(resourceSliderPrefab, offsetPos, Quaternion.identity, centerPiece.transform);


            MainResource uiContainerRef = currentSliderObj.GetComponent<MainResource>();
            uiContainerRef.sliderBar.color = resources[i].Color;
            uiContainerRef.icon.sprite = resources[i].Icon2D;
            uiContainerRef.icon.color = resources[i].Color;
            Slider currentSlider = currentSliderObj.GetComponent<Slider>();
            //currentSlider.maxValue = 100;
            
            currentSlider.value = resources[i].GatheredAmount;
            activeResourceSliders.Add(currentSlider);
            
            if(i > 0)
            {
                centerPiece.transform.position -= new Vector3(0f, offsetF / (2 * i), 0f);
            }
            /*
            if(wcon.winResource == resources[i])
            {
                uiContainerRef.mainSlider.maxValue = wcon.targetAmount;
            }*/
            
        }

    }
    
    void Update()
    {
        //update UI
        for(int i = 0; i < resources.Length; i++)
        {
            activeResourceSliders[i].value = resources[i].GatheredAmount;
        }
    }

    public void UpdateResourceSlider(int amount)
    {
        if (activeResourceSliders.Count > 0)
            activeResourceSliders[0].value = amount;
    }
}
