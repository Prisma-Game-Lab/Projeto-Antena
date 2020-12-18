using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEnd : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Image image;
    public GameObject canvas;
    public bool reachedTheEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (reachedTheEnd)
        {
            canvas.SetActive(true);
            float diff = player.gameObject.transform.position.y - gameObject.transform.position.y;
            Color tempColor = image.color;
            tempColor.a = diff / 3;
            image.color = tempColor;
        }

        if (image.color.a >= 1.0f)
        {
            //ir para os créditos
            Debug.Log("ACABOU O JOGO");
        }

    }

}
