using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeDoor : MonoBehaviour
{
    public List<GameObject> door = new List<GameObject>();
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < door.Count; i++)
            {
                door[i].GetComponent<doorConfig>().closeDooor = true;
            }
            Destroy(this);
        }
    }
}
