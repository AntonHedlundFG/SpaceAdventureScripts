using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public FMODUnity.EventReference Sound;

    private EventInstance _instance;

    private void OnEnable()
    {
        _instance = FMODUnity.RuntimeManager.CreateInstance(Sound);
        _instance.start();
    }

    private void OnDisable()
    {
        _instance.stop(STOP_MODE.ALLOWFADEOUT);
        _instance.release();
    }
}
