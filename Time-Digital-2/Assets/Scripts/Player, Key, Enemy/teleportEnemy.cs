using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportEnemy : MonoBehaviour
{
    public Transform teleportPlace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Enemy")
        {
            other.gameObject.transform.position = teleportPlace.transform.position;
            other.gameObject.GetComponent<EnemyFollowPath>().pathIndex = 1;
        }
    }
}
