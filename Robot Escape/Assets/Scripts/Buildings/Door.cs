using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected bool IsOpen = false;

    public bool GetIsOpen(){
        return IsOpen;
    }
}
