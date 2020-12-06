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
            Destroy(other.gameObject, 20f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial"))
        {
            other.GetComponent<tutorialObject>().Object.SetActive(false);
        }
    }
}
