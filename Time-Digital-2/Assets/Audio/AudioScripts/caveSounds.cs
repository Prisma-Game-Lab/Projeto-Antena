using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caveSounds : MonoBehaviour
{
    public AudioSource cave1;
    public AudioSource cave3;

    private float timer = 0.0f;
    private float timeLimit = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        cave1.loop = false;
        cave3.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        //RandomAudio();
    }

    private void RandomAudio(){
        if (timer >= timeLimit)
        {
            timeLimit = Random.Range(40.0f, 60.0f);
            timer = 0.0f;

            int i = (int)Random.Range(0, 2);
            if (i == 0)
            {
                cave1.Play();
            }
            else
            {
                cave3.Play();
            }
        }
        else
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
        }
    }
}
