using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSounds : MonoBehaviour
{
    public AudioSource alarme;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayAlarme(float totalTime)
    {
        float fadeOut = 5.0f;
        float clipDuration = alarme.clip.length - fadeOut;
        if (clipDuration > totalTime)
        {
            float offset = clipDuration - totalTime;
            alarme.time = offset;
        }
        else
        {
            float offset = totalTime - clipDuration;
            yield return new WaitForSeconds(offset);
        }
        alarme.Play();
    }
}
