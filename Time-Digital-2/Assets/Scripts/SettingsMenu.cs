using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicAudio;
    public AudioMixer effectsAudio;

    public Slider sfxSlider;
    public Slider musicSlider;
 

    private void Start()
    {
        float sfxVolume;
        float musicVolume;

        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");

        if(sfxVolume != null)
        {
            effectsAudio.SetFloat("volume", sfxVolume);
            sfxSlider.value = sfxVolume;
        }

        if (musicVolume != null)
        {
            musicAudio.SetFloat("volume", musicVolume);
            musicSlider.value = musicVolume;
        }
    }

    public void SetEffectsAudio (float volume)
    {
        effectsAudio.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicAudio(float volume)
    {
        musicAudio.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }
}
