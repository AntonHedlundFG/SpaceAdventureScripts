using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenManager : MonoBehaviour
{
    private int playersConnected;
    public List<Camera> playerCams = new List<Camera>();

    public void UpdateCameraSplit()
    {
        playersConnected++;

        if(playersConnected == 2)
        {
            foreach(GameObject tempObj in GameObject.FindGameObjectsWithTag("PlayerCam")) 
            {
                playerCams.Add(tempObj.GetComponent<Camera>());
            }
            playerCams[0].rect = new Rect(0, 0.5f, 1, 0.5f);
            playerCams[1].rect = new Rect(0, 0, 1, 0.5f);
        }

        
        
    }


}
