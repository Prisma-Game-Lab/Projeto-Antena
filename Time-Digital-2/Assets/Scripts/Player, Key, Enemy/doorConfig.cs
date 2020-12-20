using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorConfig : MonoBehaviour
{
    public int doorPassword;
    [HideInInspector]
    public bool openDoor = false;
    private Animator anim;
    public GameObject ledVerde, ledVermelho;
    public Material verdeAceso, vermelhoApagado;
    private Material[] rendererMaterials, rendererMaterials2;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        if (ledVermelho != null && ledVerde != null)
        {
            rendererMaterials = ledVermelho.GetComponent<Renderer>().materials;
            rendererMaterials2 = ledVerde.GetComponent<Renderer>().materials;
        }
    }
    private void Update()
    {
        if (openDoor)
        {
            anim.SetTrigger("open");
            if (ledVermelho != null && ledVerde != null)
            {
                rendererMaterials[1] = verdeAceso;
                rendererMaterials2[1] = vermelhoApagado;
                ledVerde.GetComponent<Renderer>().materials = rendererMaterials;
                ledVermelho.GetComponent<Renderer>().materials = rendererMaterials2;
            }
            this.GetComponent<doorSounds>().PlayOpen();
            //gameObject.SetActive(false);
        }
    }
}
