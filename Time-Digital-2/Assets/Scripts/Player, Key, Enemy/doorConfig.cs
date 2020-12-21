using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorConfig : MonoBehaviour
{
    public bool startOpen;
    public int doorPassword;
    [HideInInspector]
    public bool openDoor = false;
    [HideInInspector]
    public bool closeDooor = false;
    private Animator anim;
    public GameObject ledVerde, ledVermelho;
    public Material verdeAceso, vermelhoApagado;
    private Material[] rendererMaterials, rendererMaterials2;
    private bool oneTime = true;
    private bool oneTime2 = true;

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
        if ((openDoor || startOpen) && oneTime)
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
            oneTime = false;
            closeDooor = false;
            oneTime2 = true;
            //print("abriu");
        }else if (closeDooor && oneTime2)
        {
            anim.SetTrigger("close");
            this.GetComponent<doorSounds>().PlayClose();
            openDoor = false;
            startOpen = false;
            oneTime2 = false;
            oneTime = true;
            //print("fechou");
        }
    }
}
