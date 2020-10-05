using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_browser : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Ui1;

    [FMODUnity.EventRef]
    public string UiPlay;


    [FMODUnity.EventRef]
    public string MenuMusic;

    FMOD.Studio.EventInstance EventInstance;
    FMOD.Studio.EventInstance EventInstance2;

    private void Start()
    {
       
        EventInstance2 = FMODUnity.RuntimeManager.CreateInstance(MenuMusic);//создаёт контейнер для семпла
        EventInstance2.start();//Проигрывает этот контейнер
        
         
    }

    
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
    public void Menu_music_fadeout()
    {
       
        EventInstance2.setParameterByName("Menu_Fade", 1f, false);
        Debug.Log("Fade");
        EventInstance2.release();
    }
}
