using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySounds : MonoBehaviour
{

    public GameObject formigaAudioListGameObject;
    private AudioSource [] formigaAudioList;
    //private const int qntAudio = 4;
    private bool isPlayingAny = false;

    public AudioSource perseguicao;
    private bool isPlayingPerseguicao = false;

    private EnemyAI eAI;

    // Start is called before the first frame update
    void Start()
    {
        int qntChildren = transform.childCount;
        formigaAudioList = new AudioSource[qntChildren];

        for (int i = 0; i < qntChildren; ++i)
        {
            formigaAudioList[i] = formigaAudioListGameObject.transform.GetChild(i).GetComponent<AudioSource>();
            formigaAudioList[i].loop = false;
            //print("For loop: " + transform.GetChild(i));
        }

        eAI = this.GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

        isPlayingAny = false;
        foreach (AudioSource audio in formigaAudioList)
        {
            if (audio.isPlaying)
            {
                isPlayingAny = true;
                break;
            }
        }
        
        if (!isPlayingAny)
        {
            int i = (int) Random.Range(0, formigaAudioList.Length);
            formigaAudioList[i].Play();
        }

        if (eAI.myState == EnemyAI.stateMachine.isAttacking)
        {
            if (!isPlayingPerseguicao)
            {
                isPlayingPerseguicao = true;
                //perseguicao.Play();
                //FindObjectOfType<AudioManager>().PlayRequest(perseguicao, AudioManager.SoundType.Perseguicao);
                AudioManager.sharedInstance.PlayRequest(perseguicao, AudioManager.SoundType.Perseguicao);
            }
        }
        else
        {
            if (isPlayingPerseguicao)
            {
                isPlayingPerseguicao = false;
                //perseguicao.Stop();
                //FindObjectOfType<AudioManager>().StopRequest(AudioManager.SoundType.Perseguicao);
                AudioManager.sharedInstance.StopRequest(AudioManager.SoundType.Perseguicao);
            }
        }
    }
}
