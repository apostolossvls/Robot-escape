using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot3Audio : MonoBehaviour
{
    Robot3Brain brain;
    public AudioSource footsteps;
    bool moving;
    bool movingStopFootsteps;

    void Start()
    {
        movingStopFootsteps = false;
        brain = GetComponentInParent<Robot3Brain>();
        moving = brain.moving;
    }

    void Update(){
        if (moving && !brain.moving){
            StopFootsteps();
        }
        moving = brain.moving;
    }

    public void PlayFootsteps(){
        if (footsteps && !movingStopFootsteps) {
            AudioSource a = footsteps.gameObject.AddComponent<AudioSource>();
            a.clip = footsteps.clip;
            a.loop = false;
            a.playOnAwake = false;
            a.volume = footsteps.volume;
            a.outputAudioMixerGroup = footsteps.outputAudioMixerGroup;
            a.spatialBlend = footsteps.spatialBlend;
            a.minDistance = footsteps.minDistance;
            a.maxDistance = footsteps.maxDistance;
            a.spread = footsteps.spread;
            a.rolloffMode = footsteps.rolloffMode;
            a.Play();
            Destroy(a, 2f);
        }
    }

    public void StopFootsteps(){
        StopCoroutine("StopFootstepsCoroutine");
        StartCoroutine("StopFootstepsCoroutine");
    }
    IEnumerator StopFootstepsCoroutine(){
        movingStopFootsteps = true;
        AudioSource[] sources = GetComponentsInChildren<AudioSource>() as AudioSource[];
        List<AudioSource> s = new List<AudioSource>();
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip == footsteps.clip && footsteps != sources[i]) s.Add(sources[i]);
        }

        float timer = 0;
        while (timer < 0.25f)
        {
            for (int i = 0; i < s.Count; i++)
            {
                if (s[i] != null) s[i].volume *= 0.8f;
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < s.Count; i++)
        {
            if (s[i] != null) s[i].volume = 0;
        }

        AudioSource[] s2 = s.ToArray();
        for (int i = 0; i < s.Count; i++)
        {
            Destroy(s[i]);
        }

        movingStopFootsteps = false;

        yield return null;
    }
}
