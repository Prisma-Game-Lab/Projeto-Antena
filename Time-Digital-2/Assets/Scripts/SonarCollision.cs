using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollision : MonoBehaviour
{
    public float enemyLightTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scanner"))
        {
            GameObject light = this.gameObject.transform.GetChild(0).gameObject;
            StartCoroutine(disableEnemyLight(light));
        }
    }

    private IEnumerator disableEnemyLight(GameObject light)
    {
        light.SetActive(true);
        yield return new WaitForSeconds(enemyLightTime);
        light.SetActive(false);
    }
}
