using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paly_core_music : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string CoreMusic;

    [FMODUnity.EventRef]
    public string Menu_music;

    FMOD.Studio.EventInstance EventInstance;

   public void Music_start()
    {
        EventInstance = FMODUnity.RuntimeManager.CreateInstance(CoreMusic);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
        EventInstance.release();//Удаляет этот контейнер
    }
    
}
