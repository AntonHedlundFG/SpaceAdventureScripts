using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODStartMenuMusic : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Music;
    private void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Start Menu Music");
        Music.start();
        Music.release();
    }

    private void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
