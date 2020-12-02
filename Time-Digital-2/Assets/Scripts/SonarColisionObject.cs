using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarColisionObject : MonoBehaviour
{
    public float outlineLightTime;
    public float smothFade = 0.02f;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Scanner"))
        {
            StartCoroutine(DisableObjectOutline(gameObject));
        }
    }

    private IEnumerator DisableObjectOutline(GameObject scenarioObject)
    {
        Renderer renderer = scenarioObject.GetComponent<Renderer>();
        Material material = renderer.material;
        
        //Debug.Log(material.name);
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
