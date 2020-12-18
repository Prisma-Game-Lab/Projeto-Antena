using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicAudio;
    public AudioMixer effectsAudio;

    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider sensibilitySlider;

    public Text sensibilityValue;
    public CinemachineFreeLook cinemachine;

    private void Start()
    {
        float sfxVolume;
        float musicVolume;
        float sensibility;

        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sensibility = PlayerPrefs.GetFloat("sensibility");

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

        if (sensibility != null && cinemachine != null) {
            cinemachine.m_XAxis.m_MaxSpeed = sensibility;
            sensibilityValue.text = sensibility.ToString("F2");
            sensibilitySlider.value = sensibility;
        }
    }

    public void SetSensibilityValue()
    {
        sensibilityValue.text = sensibilitySlider.value.ToString("F2");
        cinemachine.m_XAxis.m_MaxSpeed = sensibilitySlider.value;
        PlayerPrefs.SetFloat("sensibility", sensibilitySlider.value);
        PlayerPrefs.Save();
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
