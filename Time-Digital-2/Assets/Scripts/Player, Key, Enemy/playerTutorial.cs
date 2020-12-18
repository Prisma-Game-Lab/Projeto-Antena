using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTutorial : MonoBehaviour
{
    private tutorialObject tutorialScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial"))
        {
            tutorialScript = other.GetComponent<tutorialObject>();
            tutorialScript.Object.SetActive(true);
            if (tutorialScript.lightObject!=null)
                tutorialScript.lightObject.SetActive(true);
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
