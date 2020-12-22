using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public float openTime;
    public List<GameObject> doors = new List<GameObject>();
    public List<GameObject> paths = new List<GameObject>();
    public GameObject enemiesCollection;
    public GameObject botaoCor;
    public Material mFechado, mAberto;
    public bool energyButton = false;
    public bool noTimer = false;
    [HideInInspector]
    public bool buttonPressed = false;
    private bool oneTime = true;

    public AudioSource audioButaoFinal;
    public bool alavancaDesce = false;

    private void Update()
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
    }
    private IEnumerator openDoors()
    {
        foreach (GameObject door in doors)
        {
            //print("tenta");
            door.GetComponent<doorConfig>().openDoor = true;
            StartCoroutine(door.GetComponent<doorSounds>().PlayAlarme(openTime));
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mAberto;
                botaoCor.GetComponent<Renderer>().material = mAberto;
            }
        }
        //print("Porta aberta");
        yield return new WaitForSeconds(openTime);

        foreach (GameObject door in doors)
        {
            door.GetComponent<doorConfig>().closeDooor = true;
            door.GetComponent<doorSounds>().EndAlarme();
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mFechado;
                botaoCor.GetComponent<Renderer>().material = mFechado;
            }
        }
        oneTime = true;
        //print("Porta fechada");
    }
    private void openDoor()
    {
        foreach (GameObject door in doors)
        {
            //print("tenta");
            door.GetComponent<doorConfig>().openDoor = true;
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

        int enemiesCount = enemiesCollection.transform.childCount;
        for (int i = 0; i < enemiesCount; ++i)
        {
            GameObject enemy = enemiesCollection.transform.GetChild(i).gameObject;
            if (enemy.GetComponent<EnemyAI>())
            {
                enemy.GetComponent<EnemyAI>().turnedOff = true;
                enemy.GetComponentInChildren<Animator>().SetTrigger("morto");
            }
        }
        audioButaoFinal.Play();
        alavancaDesce = true;
        //print("Porta aberta");
        yield return new WaitForEndOfFrame();
        foreach (GameObject path in paths)
        {
            path.GetComponent<Renderer>().material = mAberto;
            botaoCor.GetComponent<Renderer>().material = mAberto;
        }
    }

}
