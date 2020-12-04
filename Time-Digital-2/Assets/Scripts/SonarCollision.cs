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
   
            if (this.gameObject.CompareTag("Enemy"))
            {

                GameObject body = this.gameObject.transform.GetChild(1).gameObject;

                StartCoroutine(DisableEnemyLight(body));
            }
            else if(this.gameObject.CompareTag("key"))
            {
                Debug.Log("OBAAAA");
                GameObject body = this.gameObject.transform.GetChild(0).gameObject;

                StartCoroutine(DisableKeyLight(body));
            }

        }
    }

    private IEnumerator DisableKeyLight(GameObject body)
    {
        Debug.Log("OBAAAA22222222");
        GameObject bodyPart = body.gameObject;
        Renderer renderer = bodyPart.GetComponent<Renderer>();
        if (renderer)
        {
            renderer.material = spottedMaterial;

        }
        yield return new WaitForSeconds(enemyLightTime);

        if (renderer)
        {
            renderer.material = normalMaterial;

        }

    }

    private IEnumerator DisableEnemyLight(GameObject body)
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
