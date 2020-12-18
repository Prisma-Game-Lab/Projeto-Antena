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
    public bool deactivateDoor = false;
    [HideInInspector]
    public bool buttonPressed = false;
    private bool oneTime = true;

    private void Update()
    {
        if (buttonPressed && oneTime)
        {
            oneTime = false;
            buttonPressed = false;
            if (deactivateDoor)
                StartCoroutine("DeactivateEnemies");
            else
                StartCoroutine("closeDoors");
        }
        else if (buttonPressed)
        {
            buttonPressed = false;
        }
    }
    private IEnumerator closeDoors()
    {
        foreach (GameObject door in doors)
        {
            //door.transform.GetChild(0).gameObject.SetActive(false);
            //door.transform.GetChild(1).gameObject.SetActive(false);
            door.GetComponent<Animator>().SetTrigger("open");
            StartCoroutine(door.GetComponent<doorSounds>().PlayAlarme(openTime));
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mAberto;
                botaoCor.GetComponent<Renderer>().material = mAberto;
            }
        }
        print("Porta aberta");
        yield return new WaitForSeconds(openTime);

        foreach (GameObject door in doors)
        {
            //door.SetActive(true);
            //door.transform.GetChild(0).gameObject.SetActive(true);
            //door.transform.GetChild(1).gameObject.SetActive(true);
            door.GetComponent<Animator>().SetTrigger("close");
            door.GetComponent<doorSounds>().EndAlarme();
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mFechado;
                botaoCor.GetComponent<Renderer>().material = mFechado;
            }
        }
        oneTime = true;
        print("Porta fechada");
    }

    private IEnumerator DeactivateEnemies()
    {
        int enemiesCount = enemiesCollection.transform.childCount;
        for (int i = 0; i < enemiesCount; ++i)
        {
            GameObject enemy = enemiesCollection.transform.GetChild(i).gameObject;
            enemy.GetComponent<EnemyAI>().turnedOff = true;
        }
          

        print("Porta aberta");
        yield return new WaitForEndOfFrame();
    }

}
