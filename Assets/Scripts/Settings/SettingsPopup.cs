using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    //linkage
    public AudioMixer mixer;
    //ui linkage
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider soundVolume;
    public Button menuButton;

    public void GetParam(string value, Slider slider)
    {
        float volume;
        mixer.GetFloat(value, out volume);
        slider.value = volume;
    }

    public void SetVolumeSound(string value)
    {
        switch (value)
        {
            case "MasterVolume":
                mixer.SetFloat(value, Mathf.Max(-80, 20f * Mathf.Log10(masterVolume.value)));
                break;
            case "MusicVolume":
                mixer.SetFloat(value, Mathf.Max(-80, 20f * Mathf.Log10(musicVolume.value)));
                break;
            case "SoundVolume":
                mixer.SetFloat(value, Mathf.Max(-80, 20f * Mathf.Log10(soundVolume.value)));
                break;
        }
    }

    void Start()
    {
        GetParam("MasterVolume", masterVolume);
        GetParam("MusicVolume", musicVolume);
        GetParam("SoundVolume", soundVolume);
    }

    public void ShowToMenuButton()
    {
        menuButton.gameObject.SetActive(true);
    }

    public void Close()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume.value);
        Destroy(gameObject);
    }

    public void ToMenu(string gameScene)
    {
        SceneManager.LoadScene(gameScene);
    }
}
