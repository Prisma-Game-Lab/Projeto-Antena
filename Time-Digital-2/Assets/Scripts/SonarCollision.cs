using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollision : MonoBehaviour
{
    public float enemyLightTime;
    public Material normalMaterial;
    public Material spottedMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scanner"))
        {
            GameObject body = this.gameObject.transform.GetChild(1).gameObject;
            

            StartCoroutine(disableEnemyLight(body));
        }
    }

    private IEnumerator disableEnemyLight(GameObject body)
    {
        for (int i = 0; i < body.transform.childCount - 1; i++)
        {
            // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
            GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
            Renderer renderer = bodyPart.GetComponent<Renderer>();

            if (renderer)
            {
                renderer.material = spottedMaterial;

            }
            else
            {
                for (int j = 0; j < bodyPart.transform.childCount - 1; j++)
                {
                    GameObject miniBodyPart = bodyPart.gameObject.transform.GetChild(j).gameObject;
                    Renderer miniRenderer = miniBodyPart.GetComponent<Renderer>();

                    if (miniRenderer)
                    {
                        miniRenderer.material = spottedMaterial;
                    }
                }
            }
        }

        yield return new WaitForSeconds(enemyLightTime);

        for (int i = 0; i < body.transform.childCount - 1; i++)
        {
            // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
            GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
            Renderer renderer = bodyPart.GetComponent<Renderer>();

            if (renderer)
            {
                renderer.material = normalMaterial;

            }else
            {
                for (int j = 0; j < bodyPart.transform.childCount - 1; j++)
                {
                    GameObject miniBodyPart = bodyPart.gameObject.transform.GetChild(j).gameObject;
                    Renderer miniRenderer = miniBodyPart.GetComponent<Renderer>();

                    if (miniRenderer)
                    {
                        miniRenderer.material = normalMaterial;
                    }
                }
            }
        }
    }
}
