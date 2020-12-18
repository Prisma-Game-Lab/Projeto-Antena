using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenechangerPobre : MonoBehaviour
{
    public void LoadB(int sceneANumber)
    {
        SceneManager.LoadScene(sceneANumber);
    }
}
