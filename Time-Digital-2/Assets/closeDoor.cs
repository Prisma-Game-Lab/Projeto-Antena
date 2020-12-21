using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeDoor : MonoBehaviour
{
    public List<GameObject> doors = new List<GameObject>();
    private bool doorNotOpen=false;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < doors.Count; i++)
            {
                //print("porta" + i);
                doorConfig door = doors[i].GetComponent<doorConfig>();
                if (door.openDoor || door.startOpen)
                {
                    //print("porta" + i + "Fechar");
                    door.closeDooor = true;
                }
                //else
                //{
                //    //print("porta" + i + "Nao aberta");
                //    doorNotOpen = true;
                //}

            }
            //if (!doorNotOpen)
            //{
            //    print("Destrua");
            //    Destroy(this);

            //}
            //else
            //    doorNotOpen = false;
        }
    }
}
