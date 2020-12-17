using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollision : MonoBehaviour
{
    public float enemyLightTime;
    public Material normalMaterial;
    public Material spottedMaterial;
    public GameObject enemyModel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scanner"))
        {
   
            if (this.gameObject.CompareTag("Enemy"))
            {

                GameObject body = enemyModel;

                StartCoroutine(DisableEnemyLight(body));
            }
            else if(this.gameObject.CompareTag("key"))
            {
                GameObject body = this.gameObject.transform.GetChild(0).gameObject;

                StartCoroutine(DisableKeyLight(body));
            }

        }
    }

    private IEnumerator DisableKeyLight(GameObject body)
    {
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
        for (int i = 0; i < body.transform.childCount; i++)
        {   
            if(i!=2)
            {
                // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
                GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
                Renderer renderer = bodyPart.GetComponent<Renderer>();
                if (renderer)
                {
                    Material[] materials = renderer.materials;
                    if(materials.Length > 1)
                    {
                        int k = 0;
                        foreach (Material mat in materials)
                        {

                            materials[k] = spottedMaterial;
                            k++;
                        }
                        renderer.materials = materials;
                    }
                    else
                    {
                        renderer.material = spottedMaterial;
                    }
                    
                }
            }
        }

        yield return new WaitForSeconds(enemyLightTime);

        for (int i = 0; i < body.transform.childCount; i++)
        {
            // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
            if(i != 2)
            {
                GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
                Renderer renderer = bodyPart.GetComponent<Renderer>();
                Material material = bodyPart.GetComponent<MyRealSkin>().myRealMaterial;
                Material[] materials = bodyPart.GetComponent<MyRealSkin>().myMaterials;
                if (renderer)
                {
    
                    if (materials.Length > 1)
                    {
                        Material[] rendererMaterials = renderer.materials;
                        for (int k = 0; k < materials.Length; k++)
                        {
                            rendererMaterials[k] = materials[k];
                        }
                        renderer.materials = rendererMaterials;
                    }

                    else
                    {
                        renderer.material = material;
                    }
                }
            }
        }
    }
}
