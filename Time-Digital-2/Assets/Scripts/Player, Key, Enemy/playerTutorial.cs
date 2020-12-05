using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial"))
        {
            other.GetComponent<tutorialObject>().Object.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial"))
        {
            GameObject tutorial = other.GetComponent<tutorialObject>().Object;
            tutorial.SetActive(false);
            Destroy(tutorial,5f);
        }
    }
}
