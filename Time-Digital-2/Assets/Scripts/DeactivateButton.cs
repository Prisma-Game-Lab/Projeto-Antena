using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateButton : MonoBehaviour
{
    public float openTime;
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public bool buttonPressed = false;
    private bool oneTime = true;

    private void Update()
    {
        if (buttonPressed && oneTime)
        {
            oneTime = false;
            buttonPressed = false;
            StartCoroutine("DeactivateEnemies");
        }
        else if (buttonPressed)
        {
            buttonPressed = false;
        }
    }


    private IEnumerator DeactivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyAI>().turnedOff = true;
        }

        print("Porta aberta");
        yield return new WaitForEndOfFrame();
    }
}
