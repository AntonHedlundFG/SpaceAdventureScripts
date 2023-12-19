using System.Collections;
using System.Collections.Generic;
//using UnityEditor.MPE;
using UnityEngine;

public class MusicEventManager : MonoBehaviour
{

    [SerializeField] private EventSO firstWing;
    [SerializeField] private EventSO secondWing;
    [SerializeField] private EventSO nightBegins;
    [SerializeField] private EventSO dayBegins;
    [SerializeField] private EventSO lastStand;

    [SerializeField] private GameObject[] musicTriggers;

    [SerializeField] private FMODOverworldMusicPlayer musicPlayer;

    private int prevoiusSectionNum = 0;

    private bool lastStandActivated = false;
    private bool isNight = false;

    private void OnEnable()
    {
        firstWing.Event.AddListener(firstPickup);
        secondWing.Event.AddListener(secondPickup);
        nightBegins.Event.AddListener(nightStart);
        dayBegins.Event.AddListener(dayStart);
        lastStand.Event.AddListener(lastStandStart);
        triggerBoxhandler(0);
    }
    private void OnDisable()
    {
        firstWing.Event.RemoveListener(firstPickup);
        secondWing.Event.RemoveListener(secondPickup);
        nightBegins.Event.RemoveListener(nightStart);
        dayBegins.Event.RemoveListener(dayStart);
        lastStand.Event.RemoveListener(lastStandStart);
    }


    private void firstPickup()
    {
        triggerBoxhandler(1);
    }
    private void secondPickup()
    {
        triggerBoxhandler(2);
    }
    private void nightStart()
    {
        if (lastStandActivated)
        {
            return;
        }

        
        musicPlayer.Progress(12);
        triggerBoxhandler(3);
    }
    private void dayStart()
    {
        if (lastStandActivated)
        {
            return;
        }
        musicPlayer.Progress(musicPlayer.GetParameterVal());
        triggerBoxhandler(prevoiusSectionNum);
    }
    private void lastStandStart()
    {
        musicPlayer.Progress(13);
        triggerBoxhandler(4);
        
    }

    private void triggerBoxhandler(int numToActivate)
    {
        if (lastStandActivated || isNight)
        {
            return;
        }
        

        for (int i = 0; i < musicTriggers.Length; i++)
        {
            if(i == numToActivate)
            {
                if(i != 3)
                {
                    prevoiusSectionNum = i;
                }
                
                musicTriggers[i].SetActive(true);
            }
            else
            {
                musicTriggers[i].SetActive(false);
            }
        }
        if (numToActivate == 4)
        {

            lastStandActivated = true;
        }else if(numToActivate == 3)
        {
            isNight = true;
        }
    }




}
