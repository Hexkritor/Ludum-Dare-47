using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmUI : MonoBehaviour
{

    public Image hitWindow;
    public RhythmBar rhythmBar;
    public Animator leftPanel;
    public Animator rightPanel;

    [SerializeField]
    private float _baseBeatSize;
    [SerializeField]
    private float _baseBPM;
    [SerializeField]
    private float _hitRate;
    


    // Start is called before the first frame update
    void Start()
    {
        hitWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(_baseBeatSize * _hitRate, 32);
        leftPanel.SetFloat("speed", _baseBPM / 120);
        rightPanel.SetFloat("speed", _baseBPM / 120);
    }

}
