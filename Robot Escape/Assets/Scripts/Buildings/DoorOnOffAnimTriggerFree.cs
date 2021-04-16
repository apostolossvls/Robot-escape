using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorOnOffAnimTriggerFree : DoorOnOffAnimation
{
    [SerializeField]
    private List<string> targets;
    private List<Transform> inbound; 

    protected override void Start(){
        base.Start();
        inbound = new List<Transform>(){};
    }

    private void OnTriggerEnter(Collider other){
        print("other.tag: "+ other.tag);
        if (targets.Contains(other.tag)){
            if (!inbound.Contains(other.transform)){
                inbound.Add(other.transform);
                CheckInbound();
            }
        }
    }

    private void OnTriggerExit(Collider other){
        inbound.Remove(other.transform);
        CheckInbound();
    }

    private void CheckInbound(){
        Open(inbound.Count > 0);
    }
}
