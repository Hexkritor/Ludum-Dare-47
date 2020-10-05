using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_sound : MonoBehaviour
{
    
    [FMODUnity.EventRef]
    public string swsordAtt;

    [FMODUnity.EventRef]
    public string DthSound;

    FMOD.Studio.EventInstance EventInstance;

    public void Sword_att()
    {
        EventInstance = FMODUnity.RuntimeManager.CreateInstance(swsordAtt);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
        EventInstance.release();//Удаляет этот контейнер
    }
    public void Play_Death_Sound()
    {
        EventInstance = FMODUnity.RuntimeManager.CreateInstance(DthSound);//создаёт контейнер для семпла
        EventInstance.start();//Проигрывает этот контейнер
        EventInstance.release();//Удаляет этот контейнер
    }
}
