using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorConfig : MonoBehaviour
{
    public bool startOpen;
    public bool dontTurnOff;
    public int doorPassword;
    [HideInInspector]
    public bool openDoor = false;
    [HideInInspector]
    public bool closeDooor = false;
    public GameObject ledVerde, ledVermelho;
    public Material verdeAceso, verdeApagado, vermelhoAceso, vermelhoApagado;

    private Animator anim;
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
        if (((openDoor || startOpen) && oneTime) && (dontTurnOff || !Manager.current.turnOff))
        {
            open();
            oneTime = false;
            closeDooor = false;
            oneTime2 = true;
        }
        else if ((closeDooor || (Manager.current.turnOff && !dontTurnOff && startOpen)) && oneTime2)
        {
            close();
            openDoor = false;
            startOpen = false;
            oneTime2 = false;
            oneTime = true;
        }
    }

    private void close()
    {
        anim.SetTrigger("close");
        this.GetComponent<doorSounds>().PlayClose();
        if (ledVermelho != null && ledVerde != null)
        {
            rendererMaterials[1] = verdeApagado;
            rendererMaterials2[1] = vermelhoAceso;
            ledVerde.GetComponent<Renderer>().materials = rendererMaterials;
            ledVermelho.GetComponent<Renderer>().materials = rendererMaterials2;
        }
    }
    private void open()
    {
        anim.SetTrigger("open");
        this.GetComponent<doorSounds>().PlayOpen();
        if (ledVermelho != null && ledVerde != null)
        {
            rendererMaterials[1] = verdeAceso;
            rendererMaterials2[1] = vermelhoApagado;
            ledVerde.GetComponent<Renderer>().materials = rendererMaterials;
            ledVermelho.GetComponent<Renderer>().materials = rendererMaterials2;
        }
    }
}
