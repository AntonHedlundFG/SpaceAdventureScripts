using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WaypointManager : MonoBehaviour
{
    
    [SerializeField] private GameObject waypointPrefab;

    private GameObject p1;
    private GameObject p2;

    private GameObject wp1;
    private GameObject wp2;

    private GameObject P1_injuredFOV;
    private GameObject P2_injuredFOV;

    public Transform firstObjective;

    private int playersSpawned;

    [SerializeField] private PlayerHealth p1HPSO;
    [SerializeField] private PlayerHealth p2HPSO;

    void OnEnable()
    {
        p1HPSO?.OnPlayerHealthChange.AddListener(UpdateP1State); //? = NULL check
        p2HPSO?.OnPlayerHealthChange.AddListener(UpdateP2State);
    }
    void OnDisable()
    {
        p1HPSO?.OnPlayerHealthChange.RemoveListener(UpdateP1State);
        p2HPSO?.OnPlayerHealthChange.RemoveListener(UpdateP2State);
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        //assign waypoint system for each joined player
        GameObject player = playerInput.gameObject;
        GameObject wp = Instantiate(waypointPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
        wp.transform.parent = player.transform;
        
        
        if(playersSpawned == 0)
        {
            p1 = player;
            wp1 = wp;
            P1_injuredFOV = wp.transform.Find("InjuredFOV").gameObject;
        }
        else
        {
            p2 = player;
            wp2 = wp;
            P2_injuredFOV = wp.transform.Find("InjuredFOV").gameObject;
        }
        wp.SetActive(false);

        playersSpawned++;

        if(playersSpawned >= 2)
        {
            wp1.SetActive(true);
            wp2.SetActive(true);

            wp1.GetComponentInChildren<UI_resButton>().thisPlayer = p1;
            wp2.GetComponentInChildren<UI_resButton>().thisPlayer = p2;
            //wp1.layer = LayerMask.NameToLayer("wp1");
            wp1.GetComponent<IndividualWaypoint>().otherPlayer = p2;
            wp2.GetComponent<IndividualWaypoint>().otherPlayer = p1;

            wp1.GetComponent<ObjectiveWaypoint>().targetCam = p1.GetComponentInChildren<Camera>();
            wp2.GetComponent<ObjectiveWaypoint>().targetCam = p2.GetComponentInChildren<Camera>();

            wp1.GetComponent<Canvas>().worldCamera = p1.GetComponentInChildren<Camera>();
            wp1.GetComponent<IndividualWaypoint>().targetCam = p1.GetComponentInChildren<Camera>();
            wp1.GetComponent<Canvas>().planeDistance = 0.08f;

            //wp2.layer = LayerMask.NameToLayer("wp2");
            wp2.GetComponent<Canvas>().worldCamera = p2.GetComponentInChildren<Camera>();
            wp2.GetComponent<IndividualWaypoint>().targetCam = p2.GetComponentInChildren<Camera>();
            wp2.GetComponent<Canvas>().planeDistance = 0.08f;

            wp1.GetComponent<IndividualWaypoint>().playerText.text = "Player 2";
            wp2.GetComponent<IndividualWaypoint>().playerText.text = "Player 1";

            AssignNewObjectivePoint(firstObjective);

        }

    }
    void UpdateP1State(int hp)
    {
        if (wp2 == null)
        {
            return;
        }

        if(hp <= 0)
        {
            P1_injuredFOV.SetActive(true);
            wp2.GetComponent<IndividualWaypoint>().waypointIcon.color = new Color(255, 0, 0, wp2.GetComponent<IndividualWaypoint>().waypointIcon.color.a);
            wp2.GetComponent<IndividualWaypoint>().playerText.text = "REVIVE";
        }
        else
        {
            P1_injuredFOV.SetActive(false);
            wp2.GetComponent<IndividualWaypoint>().waypointIcon.color = new Color(255, 255, 255, wp2.GetComponent<IndividualWaypoint>().waypointIcon.color.a);
            wp2.GetComponent<IndividualWaypoint>().playerText.text = "Player 1";
        }
    }
    void UpdateP2State(int hp)
    {
        if (wp1 == null)
        {
            return;
        }

        if(hp <= 0)
        {
            P2_injuredFOV.SetActive(true);
            wp1.GetComponent<IndividualWaypoint>().waypointIcon.color = new Color(255, 0, 0, wp1.GetComponent<IndividualWaypoint>().waypointIcon.color.a);
            wp1.GetComponent<IndividualWaypoint>().playerText.text = "REVIVE";
        }
        else
        {
            P2_injuredFOV.SetActive(false);
            wp1.GetComponent<IndividualWaypoint>().waypointIcon.color = new Color(255, 255, 255, wp1.GetComponent<IndividualWaypoint>().waypointIcon.color.a);
            wp1.GetComponent<IndividualWaypoint>().playerText.text = "Player 2";
        }
    }


    public void AssignNewObjectivePoint(Transform pos)
    {
        wp1.GetComponent<ObjectiveWaypoint>()?.FeedNewPoint(pos);
        wp2.GetComponent<ObjectiveWaypoint>()?.FeedNewPoint(pos);

        if (pos == null)
        {
            return;
        }

        ObjectiveTrigger ot = pos.GetComponent<ObjectiveTrigger>();

        if (ot != null)
        {
            ot.pointActive = true;
        }

    }
    
}
