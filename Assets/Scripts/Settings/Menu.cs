using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //linkage
    public AudioMixer mixer;
    public string gameScene;
    //ui linkage
    public GameObject settingsPopup;

    void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1);
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("SoundVolume", 1);
        }
        mixer.SetFloat("MasterVolume", Mathf.Max(-80, 20f * Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume"))));
        mixer.SetFloat("MusicVolume", Mathf.Max(-80, 20f * Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))));
        mixer.SetFloat("SoundVolume", Mathf.Max(-80, 20f * Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume"))));
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void Settings()
    {
        Instantiate(settingsPopup);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
