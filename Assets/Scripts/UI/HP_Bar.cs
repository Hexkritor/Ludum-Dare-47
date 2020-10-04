using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image imageMask;
    [SerializeField]
    private int pixelsLeftOffset;
    [SerializeField]
    private int pixelsRightOffset;

    public void SetHP(int _hp)
    {
        float l = imageMask.GetComponent<RectTransform>().rect.width;
        imageMask.fillAmount = _hp / l + (pixelsLeftOffset / l);
    }
}
