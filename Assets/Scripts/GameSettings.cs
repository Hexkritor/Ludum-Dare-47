using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //linkage
    public SettingsPopup settingsPopup;
    //varrialbes
    private SettingsPopup _settingsPopup;


    void Update()
    {
        if (Input.GetAxis("Cancel") == 1 && !_settingsPopup)
        { 
            _settingsPopup = Instantiate(settingsPopup); 
            _settingsPopup.ShowToMenuButton();
        }
    }
}
