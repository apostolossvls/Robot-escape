using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControlOnClick : MonoBehaviour
{
    AudioSource source;
    public Toggle ToggleButton;
    
    public void Start(){
        source = null;
        if (MusicGameObjectDontDestroyOnLoad.instance){
            source = MusicGameObjectDontDestroyOnLoad.instance.GetComponent<AudioSource>();
        }
        else return;

        if (ToggleButton != null){
            ToggleButton.isOn = source.isPlaying;
        }
    }
    public void Play(){
        if (source != null) source.Play();
    }

    public void Pause(){
        if (source != null) source.Pause();
    }

    public void Unpause(){
        if (source != null) source.UnPause();
    }

    public void Stop(){
        if (source != null) source.Stop();
    }

    public void SetOnOff(bool value){
        if (source != null) {
            if (value){
                Unpause();
            }
            else {
                Pause();
            }
        }
    }
}
