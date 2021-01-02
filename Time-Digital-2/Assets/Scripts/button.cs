using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public float openTime;
    public List<doorConfig> doors = new List<doorConfig>();
    public List<GameObject> paths = new List<GameObject>();
    public GameObject enemiesCollection;
    public GameObject botaoCor;
    public Material mFechado, mAberto;
    public bool energyButton = false;
    public bool noTimer = false;
    [HideInInspector]
    public bool buttonPressed = false;

    public AudioSource audioButaoFinal;
    public bool alavancaDesce = false;

    private bool oneTime = true;
    private Manager manager;

    private void Start()
    {
        manager = Manager.current;
    }

    private void Update()
    {
        if (!manager.turnOff)
        {
            if (buttonPressed && oneTime)
            {
                oneTime = false;
                buttonPressed = false;
                if (energyButton)
                {
                    if (doors != null && doors.Count > 0)
                        openDoor();
                    StartCoroutine("DeactivateEnemies");
                }
                else if (noTimer)
                {
                    openDoor();
                }
                else
                    StartCoroutine("openDoors");
            }
            else if (buttonPressed)
            {
                buttonPressed = false;
            }
            checkClosedDoor();
        }
    }
    private IEnumerator openDoors()
    {
        foreach (doorConfig door in doors)
        {
            //print("tenta");
            door.openDoor = true;
            StartCoroutine(door.GetComponent<doorSounds>().PlayAlarme(openTime));
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mAberto;
                botaoCor.GetComponent<Renderer>().material = mAberto;
            }
        }
        //print("Porta aberta");
        yield return new WaitForSeconds(openTime);

        foreach (doorConfig door in doors)
        {
            door.closeDooor = true;
            door.GetComponent<doorSounds>().EndAlarme();
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mFechado;
                botaoCor.GetComponent<Renderer>().material = mFechado;
            }
        }
        oneTime = true;
    }
    private void openDoor()
    {
        foreach (doorConfig door in doors)
        {
            door.openDoor = true;
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mAberto;
                botaoCor.GetComponent<Renderer>().material = mAberto;
            }
        }
        oneTime = true;
    }

    private IEnumerator DeactivateEnemies()
    {
        manager.turnOff = true;
        audioButaoFinal.Play();
        alavancaDesce = true;
        yield return new WaitForEndOfFrame();
        foreach (GameObject path in paths)
        {
            path.GetComponent<Renderer>().material = mAberto;
            botaoCor.GetComponent<Renderer>().material = mAberto;
        }
    }
    private void checkClosedDoor()
    {
        foreach (doorConfig door in doors)
        {
            if (door.closeDooor == true && !door.openDoor)
            {
                foreach (GameObject path in paths)
                {
                    path.GetComponent<Renderer>().material = mFechado;
                    botaoCor.GetComponent<Renderer>().material = mFechado;
                }
            }
        }
    }
}
