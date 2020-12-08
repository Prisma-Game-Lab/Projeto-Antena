using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicAudio;
    public AudioMixer effectsAudio;

   public void SetEffectsAudio (float volume)
    {
        effectsAudio.SetFloat("volume", volume);
    }

    public void SetMusicAudio(float volume)
    {
        musicAudio.SetFloat("volume", volume);
    }
}
