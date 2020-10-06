using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TeleTypeArray : MonoBehaviour
{
    public TeleTypeText[] teleTypes;

    public Paly_core_music sw;

    public void SetText(int index, string text)
    {
        if (index >= 0 && index < teleTypes.Length)
        {
            teleTypes[index].SetText(text);
        }
    }

    public void SetFirstText(string text)
    {
        SetText(0, text);
    }
    public void SetSecondText(string text)
    {
        SetText(1, text);
    }

    public void LoadScene()
    {
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("Game");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PlayerPrefs.SetInt("fadeMusic", 0);
            sw.AmbInstance.setParameterByName("Dungeon_fade", 1f, false);
            sw.AmbInstance.release();
            print("Skip");
            LoadScene();
        }
           
    }
}
