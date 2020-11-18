using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSounds : MonoBehaviour
{
    public AudioSource andando;
    public AudioSource morte;
    public AudioSource cooldown;

    public GameObject sonarList;
    private AudioSource[] eco;

    private bool sonarPlayed = false;

    //ref outros scripts
    private playerMovement pm;
    private ScannerGenerator sGen;

    // Start is called before the first frame update
    void Start()
    {
        pm = this.GetComponent<playerMovement>();
        sGen = this.GetComponent<ScannerGenerator>();

        eco = sonarList.transform.GetComponentsInChildren<AudioSource>();
        //andando = this.GetComponentInChildren<AudioSource>();
        andando.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAndando();
        UpdateSonar();
    }

    void UpdateAndando()
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

    void UpdateSonar()
    {
        if (!sonarPlayed && !sGen.canUseSonar)
        {
            sonarPlayed = true;
            //toca sonar
            int i = (int)Random.Range(0, eco.Length);
            AudioSource randEco = eco[i];
            randEco.Play();
        }
        if (sonarPlayed && sGen.canUseSonar)
        {
            sonarPlayed = false;
            //toca cooldown
            cooldown.Play();
        }
    }
}
