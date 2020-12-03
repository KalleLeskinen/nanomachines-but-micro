﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    FMOD.Studio.EventInstance SoundTest;


    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.Bus MusicBus;
    FMOD.Studio.Bus CarBus;
    FMOD.Studio.Bus SFXBus;

    float MasterVol = 0.5f;
    float MusicVol = 0.5f;
    float CarVol = 0.5f;
    float SFXVol = 0.5f;


    void Awake()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        CarBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Car");
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
    }


    void Update()
    {
        MasterBus.setVolume(MasterVol);
        MusicBus.setVolume(MusicVol);
        CarBus.setVolume(CarVol);
        SFXBus.setVolume(SFXVol);
    }

    public void MasterLevel(float level)
    {
        MasterVol = level;
    }

    public void MusicLevel(float level)
    {
        MusicVol = level;
    }
    public void CarLevel(float level)
    {
        CarVol = level;
    }
    public void SFXLevel(float level)
    {
        SFXVol = level;

        FMOD.Studio.PLAYBACK_STATE pLAYBACK;
        SoundTest.getPlaybackState(out pLAYBACK);
        if (pLAYBACK != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SoundTest.start();
        }
    }
    public void OKSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/menuOk");
    }
    public void HoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/menuHover");
    }
    public void BackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/menuBack");
    }
}
