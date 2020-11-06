using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerGenerator : MonoBehaviour
{
    public GameObject scanBall;
    public GameObject player;
    public float sonarCooldown;
    private bool canUseSonar = true;

    // Update is called once per frame
    void Update()
    {
        if (canUseSonar) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SonarCooldown());
                Instantiate(scanBall, player.transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator SonarCooldown()
    {
        canUseSonar = false;
        yield return new WaitForSeconds(sonarCooldown);
        canUseSonar = true;
    }
}
