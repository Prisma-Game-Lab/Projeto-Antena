﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public float openTime;
    public List<GameObject> doors = new List<GameObject>();
    [HideInInspector]
    public bool buttonPressed = false;

    private void Update()
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            StartCoroutine("closeDoors");
        }
    }
    private IEnumerator closeDoors()
    {
        foreach (GameObject door in doors)
        {
            //door.SetActive(false);
            door.transform.GetChild(0).gameObject.SetActive(false);
            door.transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(door.GetComponent<doorSounds>().PlayAlarme(openTime));
        }
        print("Porta aberta");
        yield return new WaitForSeconds(openTime);

        foreach (GameObject door in doors)
        {
            //door.SetActive(true);
            door.transform.GetChild(0).gameObject.SetActive(true);
            door.transform.GetChild(1).gameObject.SetActive(true);
        }
        print("Porta fechada");
    }
}
