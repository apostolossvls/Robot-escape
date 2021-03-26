using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot3AnimatorCalls : MonoBehaviour
{
    Robot3Audio audioM;

    void Start()
    {
        audioM = GetComponentInParent<Robot3Brain>().GetComponentInChildren<Robot3Audio>();
    }

    public void FootstepsAudioCall(){
        if (audioM) audioM.PlayFootsteps();
    }
}
