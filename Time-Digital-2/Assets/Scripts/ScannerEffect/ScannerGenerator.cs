using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerGenerator : MonoBehaviour
{
    public GameObject scanBall;
    public playerMovement player;
    public float sonarCooldown;

    [HideInInspector]
    public bool canUseSonar = true;

    private void Start()
    {
        player = playerMovement.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUseSonar) {
            if (Input.GetKeyDown(KeyCode.Space) && !player.isDead)
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
