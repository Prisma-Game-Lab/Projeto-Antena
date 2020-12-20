using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlavancaFinal : MonoBehaviour
{
    public GameObject alavanca;
    private button btn;

    void Start(){
        btn = this.GetComponent<button>();
    }

    void Update()
    {
        if(btn.alavancaDesce == true){
            Abaixa();
        }
    }

    public void Abaixa(){
        alavanca.GetComponent<Animator>().SetTrigger("desce");
    }
}
