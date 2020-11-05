using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySounds : MonoBehaviour
{
    private AudioSource [] audioList;
    private const int qntAudio = 4;
    private bool isPlayingAny = false;

    // Start is called before the first frame update
    void Start()
    {
        audioList = new AudioSource[qntAudio];
        for (int i = 0; i < qntAudio; i++)
        {
            string name = string.Concat("formiga", (i+1).ToString());
            audioList[i] = GetChildrenAudioSource(name);
            audioList[i].loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        isPlayingAny = false;
        foreach (AudioSource audio in audioList)
        {
            if (audio.isPlaying)
            {
                isPlayingAny = true;
                break;
            }
        }
        
        if (!isPlayingAny)
        {
            int i = (int) Random.Range(0, audioList.Length);
            audioList[i].Play();
        }
    }

    AudioSource GetChildrenAudioSource(string name)
    {
        Transform trans = this.transform;
        Transform audios = trans.Find("Audios");
        Transform childTrans = audios.Find(name);

        if (childTrans != null)
        {
            return childTrans.gameObject.GetComponent<AudioSource>();
        }

        Debug.Log(string.Concat(name, " failed!\n"));
        return null;
        
    }
}
