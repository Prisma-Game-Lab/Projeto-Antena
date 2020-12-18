using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSounds : MonoBehaviour
{
    public AudioSource alarme;

    public AudioSource open;

    private bool hasPlayedOpen = false;

    public static AudioSource [] alarmeInstances = new AudioSource [10];
    public static int alarmeQnt = 0;

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
        float fadeOut = 6.0f;
        if (alarme == null){
            Debug.Log("aqui");
        }
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
        alarmeInstances[alarmeQnt] = alarme;
        alarmeQnt++;
        open.Play();
    }

    public void EndAlarme(){
        alarmeQnt--;
    }

    public void PlayOpen(){
        if (!open.isPlaying && !hasPlayedOpen){
            hasPlayedOpen = true;
            open.Play();
        }
    }

    public static void pauseAlarme(){
        foreach (AudioSource audio in alarmeInstances){
            audio.Pause();
        }
        
    }

    public static void resumeAlarme(){
        foreach (AudioSource audio in alarmeInstances){
            audio.Play();
        }
    }
}
