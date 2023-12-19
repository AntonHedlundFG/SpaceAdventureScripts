using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ObjectiveSystem : MonoBehaviour
{


    [SerializeField] public Image Icon;
    [SerializeField] public TextMeshProUGUI Title;
    [SerializeField] public TextMeshProUGUI Description;
    [SerializeField] private Animation anim;
    // Start is called before the first frame update
    void OnEnable()
    {
        ClearMissionHUD();
        StartMission("Investigate Ancient Building", "Search for the missing ship parts in the ruins.", Icon.sprite);
    }

    public void StartMission(string missionTitle, string missionDesc, Sprite iconSprite)
    {
        anim.Play();
        Icon.enabled = true;
        Icon.sprite = iconSprite;
        Title.text = missionTitle;
        Description.text = missionDesc;
    }
    public void ClearMissionHUD()
    {
        Icon.enabled = false;
        Title.text = null;
        Description.text = null;
    }
}
