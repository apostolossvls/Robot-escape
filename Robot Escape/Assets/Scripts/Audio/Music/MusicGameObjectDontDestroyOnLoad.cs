using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameObjectDontDestroyOnLoad : MonoBehaviour
{
    public static GameObject instance;
    
    void Awake()
    {
        if (instance != null && instance != gameObject){
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
        DontDestroyOnLoad(gameObject);
    }
}
