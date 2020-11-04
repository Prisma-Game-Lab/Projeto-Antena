using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSounds : MonoBehaviour
{
    private AudioSource andando;
    private playerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = this.GetComponent<playerMovement>();
        andando = this.GetComponentInChildren<AudioSource>();
        andando.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!andando.isPlaying && pm.isMoving)
        {
            andando.Play();
        }
        else if (andando.isPlaying && !pm.isMoving)
        {
            andando.Pause();
        }
    }
}
