using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenechangerPobre : MonoBehaviour
{
    public void LoadB()
    {
        AudioManager.sharedInstance.UIBack();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }
}
