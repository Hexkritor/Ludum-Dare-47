using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_browser : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Ui1;

    [FMODUnity.EventRef]
    public string UiPlay;

    FMOD.Studio.EventInstance EventInstance;
    public void PlayUI1()
    {
        EventInstance = FMODUnity.RuntimeManager.CreateInstance(Ui1);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
        EventInstance.release();//Удаляет этот контейнер
    }

    public void PlayUIplay()
    {
        EventInstance = FMODUnity.RuntimeManager.CreateInstance(UiPlay);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
        EventInstance.release();//Удаляет этот контейнер
    }
}
