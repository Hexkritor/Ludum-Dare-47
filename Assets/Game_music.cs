﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_music : MonoBehaviour
{


    [FMODUnity.EventRef]
    public string CoreMusic2;

    FMOD.Studio.EventInstance EventInstance;
    public Paly_core_music sw;

    

    private void Start()
    {

        print(PlayerPrefs.GetInt("fadeMusic").ToString());

        if (PlayerPrefs.GetInt("fadeMusic")==0)
        {
            EventInstance = FMODUnity.RuntimeManager.CreateInstance(CoreMusic2);//создаёт контейнер для семпла
            EventInstance.start();//Проигрывает этот контейнер
            //EventInstance.release();//Удаляет этот контейнер

            sw.AmbInstance.setParameterByName("Dungeon_fade", 1f, false);
            sw.AmbInstance.release();
        }
        
    }

    private void OnDestroy()
    {
        EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


}
