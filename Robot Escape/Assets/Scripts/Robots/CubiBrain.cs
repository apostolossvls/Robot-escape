using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubiBrain : MonoBehaviour
{
    public NavMeshCirclePath path;
    public AudioSource source;
    void Update()
    {
        source.enabled = path.moving;
    }
}
