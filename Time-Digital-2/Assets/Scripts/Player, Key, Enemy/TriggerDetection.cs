using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TriggerDetection : MonoBehaviour
{
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public GameObject endCameraPoint;
    public Light lanterna;
    public Light luzCabecaEsconderijo;

    public AudioSource safeSpot;
    public AudioSource morte;

    private int triggerCount;
    private float lanternaInicial;
    private bool thirdPersonMode;
    private playerMovement playerStats;

    private bool isPlayingFinalMusic = false;


    // Start is called before the first frame update
    void Start()
    {
        thirdPersonMode = true;
        playerStats = playerMovement.current;
        lanternaInicial = lanterna.intensity;
        triggerCount = 0;
        thirdPersonCam.SetActive(thirdPersonMode);
        firstPersonCam.SetActive(!thirdPersonMode);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Entrou num esconderijo
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            inSafeSpot();
        }
        //Foi atacado e morre
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            die();
        }
        //Entrou num checkpoint
        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            enterCheckPoint(collision);
        }
        //Esta do lado de um botao
        else if (collision.gameObject.CompareTag("button"))
        {
            print("botao em area");
            playerStats.button = collision.gameObject;
        }
        //Chegou no final do jogo
        else if (collision.gameObject.CompareTag("End"))
        {
            theEnd(collision);
        }
        else if (collision.gameObject.CompareTag("FinalMusic"))
        {
            if (!isPlayingFinalMusic)
            {
                isPlayingFinalMusic = true;
                musicFinal();
            }
            
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //Saiu do esconderijo
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            outSafeSpot();
        }
        //Saiu de perto de um botao
        else if (collision.gameObject.CompareTag("button"))
        {
            playerStats.button = null;
        }
    }

    private void die()
    {
        AudioManager.sharedInstance.PlayRequest(morte, AudioManager.SoundType.Morte);
        playerStats.isDead = true;
        if (playerStats.inTheEnd)
        {
            playerStats.inTheEnd = false;
            thirdPersonCam.GetComponent<CinemachineFreeLook>().Follow = gameObject.transform;
        }
        //gameObject.GetComponent<ScannerGenerator>().canUseSonar = true;
    }
    private void inSafeSpot()
    {
        if (triggerCount <= 0)
        {
            luzCabecaEsconderijo.enabled = true;
            lanterna.intensity = playerStats.safeSpotLightIntensity;
            thirdPersonCam.SetActive(!thirdPersonMode);
            firstPersonCam.SetActive(thirdPersonMode);
            AudioManager.sharedInstance.PlayRequest(safeSpot, AudioManager.SoundType.SafeSpot);
            playerStats.isSafe = true;
        }
        triggerCount++;
    }

    private void outSafeSpot()
    {
        triggerCount--;
        if (triggerCount <= 0)
        {
            luzCabecaEsconderijo.enabled = false;
            lanterna.intensity = lanternaInicial;
            thirdPersonCam.SetActive(thirdPersonMode);
            firstPersonCam.SetActive(!thirdPersonMode);
            playerStats.isSafe = false;
            AudioManager.sharedInstance.StopRequest(AudioManager.SoundType.SafeSpot);
        }
    }

    private void enterCheckPoint(Collider collision)
    {
        playerStats.lastCheckpointPos = collision.gameObject.transform.position;
        playerStats.lastCheckpointRot = transform.rotation;
        collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        ES3.Save<Vector3>("posicao", playerStats.lastCheckpointPos);
        ES3.Save<Quaternion>("rotacao", playerStats.lastCheckpointRot);
        ES3.Save<bool>("energia", Manager.current.turnOff);
    }

    private void theEnd(Collider collision)
    {
        endCameraPoint.gameObject.transform.position = this.gameObject.transform.position;
        this.gameObject.transform.rotation = endCameraPoint.gameObject.transform.rotation;
        collision.GetComponent<TheEnd>().reachedTheEnd = true;
        thirdPersonCam.GetComponent<CinemachineFreeLook>().Follow = endCameraPoint.gameObject.transform;
        playerStats.inTheEnd = true;
    }

    private void musicFinal()
    {
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Final);
    }
}
