using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerGenerator : MonoBehaviour
{
    public GameObject scanBall;
    public playerMovement player;
    public float sonarCooldown;
    public GameObject sonarCooldownObject;
    public Material sonarOnMaterial;
    public Material sonarOffMaterial;
    private Renderer sonarCooldownObjectRenderer;

    [HideInInspector]
    public bool canUseSonar = true;

    private void Start()
    {
        player = playerMovement.current;
        sonarCooldownObjectRenderer = sonarCooldownObject.GetComponent<Renderer>();
        sonarCooldownObjectRenderer.material = sonarOnMaterial;
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
        sonarCooldownObjectRenderer.material = sonarOffMaterial;
        yield return new WaitForSeconds(sonarCooldown);
        canUseSonar = true;
        sonarCooldownObjectRenderer.material = sonarOnMaterial;
    }
}
