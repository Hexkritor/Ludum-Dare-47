using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class slider_sfx : MonoBehaviour
{
    /// <summary>
    /// Наш скрипт универсальный, мы можем его закидывать на несколько одинаковых слайдеров громкости
    /// </summary>
    public string vcaName; // Название VCA-ручки
    private UnityEngine.UI.Slider ourSlider; // приватная переменная для доступа к слайдеру
    private FMOD.Studio.VCA vcaController; // переменная для доступа к установки и получению громкости с фмод
    private float vcaVolume; // в эту переменную записывается значение громкости из VCA




    void Start()
    {
        ourSlider = gameObject.GetComponent<UnityEngine.UI.Slider> (); // переменной слайдера добавляем доступ к компоненту слайдера
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName); // даем доступ к VCA по имени, которое мы пишем в компоненте
        vcaController.getVolume(out vcaVolume); //  передаем значение громкости шины VCA в переменную vcaVolume
        ourSlider.value = vcaVolume; // 
    }

    /// <summary>
    /// Мы можем менять громкость каждый фрейм, но правельнее будет менять громкость только в случае ее изменения (функцию ValueChange)
    /// </summary>
    /// 

    //void Update()
    //{
    //    vcaController.getVolume(out vcaVolume);
    //    vcaController.setVolume(ourSlider.value);
    //}

    public void VCAVolumeChange () // Создаем новую функцию, которую нужно обязательно подключить в компоненте слайдера On Value Changed
    {
        vcaController.setVolume(ourSlider.value); // устанавливаем громкость шины VCA в такое же значение, как значение нашего слайдера
    }
}
