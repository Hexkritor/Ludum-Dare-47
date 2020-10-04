using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleTypeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int counter;
    private float timer;
    private Color color = new Color(1,1,1,0);

    public void SetText(string shownText)
    {
        text.text = shownText;
        StopAllCoroutines();
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        counter = 0;
        text.maxVisibleCharacters = 0;
        while (true)
        {
            if (counter < text.textInfo.characterCount)
            {
                color.a = 1;
                counter += 1;
                timer = 4.0f;
            }
            else 
            {
                timer -= Time.fixedDeltaTime;
                color.a = (timer >= 1) ? 1 : timer;
            }
            text.maxVisibleCharacters = counter % (text.textInfo.characterCount + 1);
            text.color = color;
            yield return new WaitForFixedUpdate();
        }
    }

}
