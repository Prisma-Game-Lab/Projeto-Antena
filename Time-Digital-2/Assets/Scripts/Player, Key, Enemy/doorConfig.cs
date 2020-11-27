using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorConfig : MonoBehaviour
{
    public int doorPassword;
    [HideInInspector]
    public bool openDoor = false;
    private void Update()
    {
        if (openDoor)
        {
            gameObject.SetActive(false);
        }
    }
}
