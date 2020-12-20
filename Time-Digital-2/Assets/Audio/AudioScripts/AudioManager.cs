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
        Proximidade,
        SafeSpot
    }

    /*private static Dictionary<SoundType, int> soundRequest = new Dictionary<SoundType, int> {
        {SoundType.Morte , 0},
        {SoundType.Perseguicao , 0}
    };*/

    public static AudioManager sharedInstance;


    private Dictionary<SoundType, int> soundRequest;
    private Dictionary<SoundType, AudioSource> soundCurrentAudioSource;
    private Dictionary<SoundType, AudioMixerSnapshot> soundSnapshot;
  

    public float SFXtransitionDuration;
    public float musicTransitionDuration;
    public AudioMixer sfxMixer;
    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot persegMorte;
    public AudioMixerSnapshot proximidade;
    public AudioMixerSnapshot safeSpot;

    private AudioMixerSnapshot currentSnapshot;

    Dictionary<AudioMixerSnapshot, int> snapshotPriority;


    //music

     public enum MusicType
    {
        Menu,
        Play,
        Final,
        Tema
    }

    public AudioMixerSnapshot musicNormal;
    public AudioMixerSnapshot musicBaixo;

    private AudioSource musicAudio;

    public AudioSource musicAudioMenu;
    public AudioSource musicAudioPlay;
    public AudioSource musicAudioFinal;
    public AudioSource musicAudioTema;

    private Dictionary<MusicType, AudioSource> musicAudioSource;


    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


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
            [SoundType.Proximidade] = proximidade,
            [SoundType.SafeSpot] = safeSpot
        };

        snapshotPriority = new Dictionary<AudioMixerSnapshot, int>{
            [normal] = 4,
            [persegMorte] = 2,
            [proximidade] = 3,
            [safeSpot] = 1
        };

        musicAudioSource = new Dictionary<MusicType, AudioSource> {
            {MusicType.Menu ,musicAudioMenu},
            {MusicType.Play , musicAudioPlay},
            {MusicType.Final , musicAudioFinal},
            {MusicType.Tema , musicAudioTema}
        };

    }


    //private static AudioSource perseguicaoAudioSource = null;

    public void PlayRequest(AudioSource audioSource, SoundType type)
    {
        soundRequest[type] ++;
        if (soundRequest[type] == 1)
        {
            //Debug.Log("Play");
            if (soundSnapshot.ContainsKey(type)){
                //soundSnapshot[type].TransitionTo(transitionDuration);
                SetSnapshot(soundSnapshot[type]);
            }
            else{
                Debug.Log("SoundType sem Snapshot Correspondente!");
                //normal.TransitionTo(transitionDuration);
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
            //normal.TransitionTo(transitionDuration); 
            SetSnapshot(normal);

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

    private void SetSnapshot(AudioMixerSnapshot snap)
    {
        if (currentSnapshot == null)
        {
            currentSnapshot = snap;
            return;
        }

        if (snapshotPriority.ContainsKey(snap))
        {
            if (snapshotPriority[snap] < snapshotPriority[currentSnapshot])
            {
                currentSnapshot = snap;
            }
        }
    }

    private void FixedUpdate() {
        //Debug.Log(musicAudio);

        if (currentSnapshot != null)
        {
            if (currentSnapshot != normal)
                musicBaixo.TransitionTo(musicTransitionDuration);
            else
                musicNormal.TransitionTo(musicTransitionDuration);
            currentSnapshot.TransitionTo(SFXtransitionDuration);
            currentSnapshot = null;
        }
    }

    
    public void ChangeMusic(MusicType music){
        if (musicAudio != null)
            musicAudio.Stop();
        musicAudio = musicAudioSource[music];//AudioSource.Instantiate(music);
        musicAudio.Play();
        Debug.Log("Play Music");
    }

}
