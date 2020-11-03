using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerGenerator : MonoBehaviour
{
    public GameObject scanBall;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(scanBall, player.transform.position, Quaternion.identity);
        }
    }
}
