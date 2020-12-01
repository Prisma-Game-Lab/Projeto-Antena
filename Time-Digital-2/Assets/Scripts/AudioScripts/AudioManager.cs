using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Morte,
        Perseguicao,
    }

    /*private static Dictionary<SoundType, int> soundRequest = new Dictionary<SoundType, int> {
        {SoundType.Morte , 0},
        {SoundType.Perseguicao , 0}
    };*/

    private Dictionary<SoundType, int> soundRequest;
    private Dictionary<SoundType, AudioSource> soundCurrentAudioSource;
    public static AudioManager sharedInstance;

    public AudioMixer audioMixer;
    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot perseguicao;


    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        soundRequest = new Dictionary<SoundType, int>();
        soundCurrentAudioSource = new Dictionary<SoundType, AudioSource>();

        foreach(SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            soundRequest[soundType] = 0;
            soundCurrentAudioSource[soundType] = null;
        }

        //audioMixer = Resources.Load<AudioMixer>("audioMixer");
    }


    //private static AudioSource perseguicaoAudioSource = null;

    public void PlayRequest(AudioSource audioSource, SoundType type)
    {
        soundRequest[type] ++;
        if (soundRequest[type] == 1)
        {
            //Debug.Log("Play");
            soundCurrentAudioSource[type] = audioSource;
            soundCurrentAudioSource[type].Play();

            perseguicao.TransitionTo(1.0f);

            if (!audioSource.loop)
            {
                StartCoroutine(WaitForSound(audioSource.clip.length, type));
            }

        }

    }

    public void StopRequest(SoundType type)
    {
        soundRequest[type] --;
        if (soundRequest[type] == 0)
        {
            //Debug.Log("Stop");
            soundCurrentAudioSource[type].Stop();
            soundCurrentAudioSource[type] = null;

            normal.TransitionTo(1.0f);
        }

    }

    private IEnumerator WaitForSound(float duration, SoundType type)
    {
        yield return new WaitForSeconds(duration);
        StopRequest(type);
    }

/*
    private void Update() {
        if (audioMixer != null){
            //audioMixer.SetFloat("MasterVol", -80.0f);
            float f;
            if (audioMixer.GetFloat("PlayerVol", out f))
            {
                Debug.Log(f);
                audioMixer.SetFloat("PlayerVol", f - 0.01f);
            }
            //AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("Master");
        }
    }
*/

    


}
