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

    public void playAlarme()
    {
        alarme.Play();
        Debug.Log("ALARME");
    }
}
