using System.Collections;
using System.Collections.Generic;
//using UnityEditor.MPE;
using UnityEngine;

public class FMODOverworldMusicPlayer : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Music;

    private int lastParamValPassed = 0;

    private void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Overworld Music");
        Music.start();
        Music.release();
    }

    /*public void OntriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Intro")
        {
            Music.start();
            Music.setParameterByName("Progress", 0f);
        }
    }*/
    public int GetParameterVal()
    {
        return lastParamValPassed;
    }
    public void Progress (int ProgressLevel)
    {
        if(ProgressLevel != 12)
        {
            lastParamValPassed = ProgressLevel;
        }
        Music.setParameterByName("Parameter 1", ProgressLevel);
    }

    private void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
