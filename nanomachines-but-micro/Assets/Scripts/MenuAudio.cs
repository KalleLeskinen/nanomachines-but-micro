using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public static MenuAudio menuAudio;
    public FMOD.Studio.EventInstance SoundTest;


    public FMOD.Studio.Bus MasterBus;
    public FMOD.Studio.Bus MusicBus;
    public FMOD.Studio.Bus CarBus;
    public FMOD.Studio.Bus SFXBus;

    float MasterVol = 0.8f;
    float MusicVol = 0.8f;
    float CarVol = 0.8f;
    float SFXVol = 0.8f;


    void Awake()
    {

        if(menuAudio == null)
        {
            DontDestroyOnLoad(gameObject);
            menuAudio = this;
        }
        else if(menuAudio != this)
        {
            Destroy(gameObject);
        }
        SoundTest = FMODUnity.RuntimeManager.CreateInstance("event:/ImpactHard");
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

    public void MasterLevel(float maslevel)
    {
        MasterVol = maslevel;
    }

    public void MusicLevel(float muslevel)
    {
        MusicVol = muslevel;
    }
    public void CarLevel(float carlevel)
    {
        CarVol = carlevel;
    }
    public void SFXLevel(float sfxlevel)
    {
        SFXVol = sfxlevel;

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
