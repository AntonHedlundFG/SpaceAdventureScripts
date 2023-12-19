using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Instructions : MonoBehaviour
{

    private int playersJoined;
    [SerializeField] private GameObject instructions;

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playersJoined++;
        if(playersJoined >= 2)
        {
            instructions.GetComponent<Animation>().Play();
        }
    }
}
