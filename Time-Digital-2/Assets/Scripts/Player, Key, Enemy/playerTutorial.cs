using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTutorial : MonoBehaviour
{
    private tutorialObject tutorialScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial") && !playerMovement.current.isStarting)
        {
            tutorialScript = other.GetComponent<tutorialObject>();
            tutorialScript.Object.SetActive(true);
            if (tutorialScript.lightObject!=null)
                tutorialScript.lightObject.SetActive(true);
            Destroy(tutorialScript.Object, 15f);
            Destroy(other.gameObject, 15f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("tutorial") && !playerMovement.current.isStarting)
        {
            if(tutorialScript!=null)
                tutorialScript.Object.SetActive(false);
            //other.GetComponent<tutorialObject>().Object.SetActive(false);
        }
    }
}
