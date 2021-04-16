using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class DoorOnOffAnimation : Door
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource openAudioSouce;
    [SerializeField]
    private AudioSource closeAudioSouce;
    [SerializeField]
    private string animatorStringIsOpen = "IsOpen";

    protected virtual void Start(){
        Open(isOpen);
    }

    public void Open(bool value){
        isOpen = value;
        if (animator != null) animator.SetBool(animatorStringIsOpen , isOpen);
    }

    public void PlayOpenAudioSouce(){
        if (!isOpen) return;
        openAudioSouce.Play();
    }

    public void PlayCloseAudioSouce(){
        if (isOpen) return;
        closeAudioSouce.Play();
    }
}
