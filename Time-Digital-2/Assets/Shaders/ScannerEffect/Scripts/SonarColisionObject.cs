using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarColisionObject : MonoBehaviour
{
    public float outlineLightTime;
    public float smothFade = 0.02f;
    //private Renderer renderer;
    //private Material material;
    //private Material[] materials;


    /*private void Start()
    {
        getMaterial();
    }*/

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Scanner"))
        {
            StartCoroutine(DisableObjectOutline(gameObject));
        }
    }

    Material getMaterial(GameObject body)
    {
        Renderer renderer;
        Material material;

        if (gameObject.CompareTag("key"))
        {
            renderer = body.transform.GetChild(0).gameObject.GetComponent<Renderer>();
            material = renderer.material;
        }
        else
        {
            renderer = body.GetComponent<Renderer>();
            material = renderer.material;
        }

        return material;
    }

    Material[] getMaterials(Renderer objectRender)
    {
        Material[] materials;
        materials = objectRender.materials;
        return materials;
    }

    private void OnDisable()
    {
        if (this.gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < this.gameObject.transform.childCount - 1; i++)
            {
                // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
                GameObject bodyPart = this.gameObject.gameObject.transform.GetChild(i).gameObject;
                Renderer renderer = bodyPart.GetComponent<Renderer>();

                if (renderer)
                {
                    Material[] materials = getMaterials(renderer);
                    if (materials.Length > 1)
                    {
                        foreach (Material mat in materials)
                        {
                            mat.SetFloat("Vector1_C0B001A6", 0.0f);
                        }
                    }
                    else
                    {
                        Material material = getMaterial(bodyPart);
                        material.SetFloat("Vector1_C0B001A6", 0.0f);
                    }

                }

            }
        }
        else
        {
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            Material[] materials = getMaterials(renderer);
            Material material = getMaterial(gameObject);

            if (materials.Length > 1)
            {
                foreach (Material mat in materials)
                {
                    mat.SetFloat("Vector1_C0B001A6", 0.0f);
                }
            }else
            {
                material.SetFloat("Vector1_C0B001A6", 0.0f);
            }
        }

       

    }

    private IEnumerator DisableObjectOutline(GameObject body)
    {
        if (body.transform.childCount > 0)
        {

            for (int i = 0; i < body.transform.childCount - 1; i++)
            {
                // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA
                
                GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
                Renderer renderer = bodyPart.GetComponent<Renderer>();

                if (renderer)
                {
                    Material[] materials = getMaterials(renderer);
                    if (materials.Length > 1)
                    {
                        foreach (Material mat in materials)
                        {
                            mat.SetFloat("Vector1_C0B001A6", 0.5f);
                            Debug.Log("TA ATIVANDO");
                        }
                    }
                    else
                    {
                        Material material = getMaterial(bodyPart);
                        material.SetFloat("Vector1_C0B001A6", 0.5f);
                    }

                }
            }

            yield return new WaitForSeconds(outlineLightTime);

            for (int i = 0; i < body.transform.childCount - 1; i++)
            {
                // TEMPORARIO DADO A FORMA COMO A FORMIGA E FEITA 
                GameObject bodyPart = body.gameObject.transform.GetChild(i).gameObject;
                Renderer renderer = bodyPart.GetComponent<Renderer>();

                if (renderer)
                {
                    Material[] materials = getMaterials(renderer);
                    if (materials.Length > 1)
                    {
                        float j = 0.5f;
                        while (j > 0)
                        {
                            j -= smothFade;
                            foreach (Material mat in materials)
                            {
                                mat.SetFloat("Vector1_C0B001A6", j);
                            }
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    else
                    {
                        Material material = getMaterial(bodyPart);
                        float j = 0.5f;
                        while (j > 0)
                        {
                            j -= smothFade;
                            material.SetFloat("Vector1_C0B001A6", j);
                            yield return new WaitForEndOfFrame();
                        }
                    }

                }
            }
        }
        else
        {
            Renderer renderer = body.GetComponent<Renderer>();
            Material[] materials = getMaterials(renderer);
            Material material = getMaterial(gameObject);

            if (materials.Length > 1)
            {
                foreach (Material mat in materials)
                {
                    mat.SetFloat("Vector1_C0B001A6", 0.5f);
                }
                yield return new WaitForSeconds(outlineLightTime);
                float i = 0.5f;
                while (i > 0)
                {
                    i -= smothFade;
                    foreach (Material mat in materials)
                    {
                        mat.SetFloat("Vector1_C0B001A6", i);
                    }
                    yield return new WaitForEndOfFrame();
                }

            }
            else
            {
                material.SetFloat("Vector1_C0B001A6", 0.5f);
                yield return new WaitForSeconds(outlineLightTime);
                float i = 0.5f;
                while (i > 0)
                {
                    i -= smothFade;
                    material.SetFloat("Vector1_C0B001A6", i);
                    yield return new WaitForEndOfFrame();
                }

            }

        }
    }

}
