using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public float openTime;
    public List<GameObject> doors = new List<GameObject>();
    public List<GameObject> paths = new List<GameObject>();
    public GameObject botaoCor;
    public Material mFechado, mAberto;
    [HideInInspector]
    public bool buttonPressed = false;
    private bool oneTime = true;

    private void Update()
    {
        if (buttonPressed && oneTime)
        {
            oneTime = false;
            buttonPressed = false;
            StartCoroutine("closeDoors");
        }else if (buttonPressed)
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
            foreach (GameObject path in paths)
            {
                path.GetComponent<Renderer>().material = mFechado;
                botaoCor.GetComponent<Renderer>().material = mFechado;
            }
        }
        oneTime = true;
        print("Porta fechada");
    }
}
