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
    public float temaTransitionDuration;
    public float menuTransitionDuration;
    public float finalTransitionDuration;
    public float otherTransitionDuration;
    public AudioMixer sfxMixer;
    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot persegMorte;
    public AudioMixerSnapshot proximidade;
    public AudioMixerSnapshot safeSpot;

    private AudioMixerSnapshot currentSnapshot;

    Dictionary<AudioMixerSnapshot, int> snapshotPriority;

    public AudioSource uiSelect;
    public AudioSource uiBack;


    //music

     public enum MusicType
    {
        Menu,
        Play,
        Final,
        Tema
    }

    public AudioMixerSnapshot temaNormal;
    public AudioMixerSnapshot temaBaixo;
    public AudioMixerSnapshot menu;
    public AudioMixerSnapshot play;
    public AudioMixerSnapshot final;

    public AudioSource musicAudioMenu;
    public AudioSource musicAudioPlay;
    public AudioSource musicAudioFinal;
    public AudioSource musicAudioTema;

    public bool hasPlayedFinal = false;

    private Dictionary<MusicType, AudioSource> musicAudioSource;
    private Dictionary<MusicType, AudioMixerSnapshot> musicAudioSnapshot;


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

        musicAudioSnapshot = new Dictionary<MusicType, AudioMixerSnapshot> {
            {MusicType.Menu ,menu},
            {MusicType.Play , play},
            {MusicType.Final , final},
            {MusicType.Tema , temaNormal}
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
            if (!hasPlayedFinal)
            {
                if (currentSnapshot != normal)
                    temaBaixo.TransitionTo(temaTransitionDuration);
                else
                    temaNormal.TransitionTo(temaTransitionDuration);
            }
            currentSnapshot.TransitionTo(SFXtransitionDuration);
            currentSnapshot = null;
        }
    }

    
    public void ChangeMusic(MusicType music){

        /*foreach (MusicType type in Enum.GetValues(typeof(MusicType)))
        {
            AudioSource audio = musicAudioSource[type];
            if (audio.isPlaying)
            {
                //fade
                musicAudioSnapshot[type].
            }
        }*/

        AudioSource musicAudio = musicAudioSource[music];
        if (musicAudio.isPlaying)
            musicAudio.Stop();
        musicAudio.time = 0.0f;

        float duration = otherTransitionDuration;
        if (music == MusicType.Final)
        {
            duration = finalTransitionDuration;
            hasPlayedFinal = true;
        }
        else if (music == MusicType.Menu)
            duration = menuTransitionDuration;
        else if (music == MusicType.Play)
            hasPlayedFinal = false;

        musicAudioSnapshot[music].TransitionTo(duration);
        musicAudio.Play();
        //fadeIn
    }

    public void UISelect()
    {
        if (uiSelect != null)
            uiSelect.Play();
    }

    public void UIBack()
    {
        if (uiBack != null)
            uiBack.Play();
    }

}
