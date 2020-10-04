using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBar : MonoBehaviour
{
    public RectTransform rectTransform;
    private float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        rectTransform.anchoredPosition += Vector2.right * speed;
        if (speed > 0 && rectTransform.anchoredPosition.x >= 120 || speed < 0 && rectTransform.anchoredPosition.x <= -120)
            Destroy(gameObject);
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }
}
