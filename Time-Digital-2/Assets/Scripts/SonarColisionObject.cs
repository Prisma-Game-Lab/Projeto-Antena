using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarColisionObject : MonoBehaviour
{
    public float outlineLightTime;
    public float smothFade = 0.02f;
    private Renderer renderer;
    private Material material;


    private void Start()
    {
        getMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Scanner"))
        {
            StartCoroutine(DisableObjectOutline(gameObject));
        }
    }

    void getMaterial()
    {
        if (gameObject.CompareTag("key"))
        {
            renderer = gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();
            material = renderer.material;
        }
        else
        {
            renderer = gameObject.GetComponent<Renderer>();
            material = renderer.material;
        }
    }

    private void OnDisable()
    {

        material.SetFloat("Vector1_C0B001A6", 0.0f);

    }

    private IEnumerator DisableObjectOutline(GameObject scenarioObject)
    {
        material.SetFloat("Vector1_C0B001A6", 1.0f);
        yield return new WaitForSeconds(outlineLightTime);
        float i = 1.0f;
        while(i > 0)
        {
            i -= smothFade;
            material.SetFloat("Vector1_C0B001A6", i);
            yield return new WaitForEndOfFrame();
        }
        
    }

}
