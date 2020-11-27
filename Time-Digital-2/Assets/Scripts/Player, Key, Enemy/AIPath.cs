using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPath
{
    [HideInInspector]
    public Vector3 destinationPos;
    public bool shouldWait;
    public float waitTime;
}
