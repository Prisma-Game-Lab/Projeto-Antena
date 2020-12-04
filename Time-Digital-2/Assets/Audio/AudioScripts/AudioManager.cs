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
        Proximidade
    }

    /*private static Dictionary<SoundType, int> soundRequest = new Dictionary<SoundType, int> {
        {SoundType.Morte , 0},
        {SoundType.Perseguicao , 0}
    };*/

    public static AudioManager sharedInstance;


    private Dictionary<SoundType, int> soundRequest;
    private Dictionary<SoundType, AudioSource> soundCurrentAudioSource;
    private Dictionary<SoundType, AudioMixerSnapshot> soundSnapshot;
  

    public float transitionDuration;
    public AudioMixer sfxMixer;
    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot persegMorte;
    public AudioMixerSnapshot proximidade;


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

        soundSnapshot = new Dictionary<SoundType, AudioMixerSnapshot>{
            [SoundType.Morte] = persegMorte,
            [SoundType.Perseguicao] = persegMorte,
            [SoundType.Proximidade] = proximidade
        };

        //audioMixer = Resources.Load<AudioMixer>("audioMixer");
    }


    //private static AudioSource perseguicaoAudioSource = null;

    public void PlayRequest(AudioSource audioSource, SoundType type)
    {
        soundRequest[type] ++;
        if (soundRequest[type] == 1)
        {
            //Debug.Log("Play");
            if (soundSnapshot.ContainsKey(type)){
                soundSnapshot[type].TransitionTo(transitionDuration);
            }
            else{
                Debug.Log("SoundType sem Snapshot Correspondente!");
                normal.TransitionTo(transitionDuration);
            }

            if (audioSource == null)
            {
                Debug.Log("AudioSource nulo!");
                return;
            }

            soundCurrentAudioSource[type] = audioSource;
            soundCurrentAudioSource[type].Play();
            
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
            normal.TransitionTo(transitionDuration); 

            if (soundCurrentAudioSource[type] == null)
            {
                Debug.Log("AudioSource nulo!");
                return;
            }

            soundCurrentAudioSource[type].Stop();
            soundCurrentAudioSource[type] = null;
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
