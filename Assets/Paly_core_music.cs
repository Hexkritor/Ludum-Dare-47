using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paly_core_music : MonoBehaviour
{
   

    [FMODUnity.EventRef]
    public string CoreMusic;

    [FMODUnity.EventRef]
    public string Backambience;

    


    FMOD.Studio.EventInstance EventInstance;
    public FMOD.Studio.EventInstance AmbInstance;

    private void Start()
    {
        AmbInstance = FMODUnity.RuntimeManager.CreateInstance(Backambience);//создаёт контейнер для семпла
        AmbInstance.start();

        if (SceneManager.GetActiveScene().name == "Cutscene")
        {
            PlayerPrefs.SetInt("fadeMusic", 0);
        }

    }

    public void Music_start()
    {
        PlayerPrefs.SetInt("fadeMusic", 1);

        EventInstance = FMODUnity.RuntimeManager.CreateInstance(CoreMusic);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
       

        AmbInstance.setParameterByName("Dungeon_fade", 1f, false);
        AmbInstance.release();
    }
    public void Music_stop()
    {
        EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EventInstance.release();

    }
}
