﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public float scanVelocity = 0.3f;
    private Vector3 scaleChange;
    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(scanVelocity, scanVelocity, scanVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += scaleChange;
    }
}