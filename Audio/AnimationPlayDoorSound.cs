using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Dungeon1DoorAnimationSound : MonoBehaviour
{
    public FMODUnity.EventReference sound;

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }

}