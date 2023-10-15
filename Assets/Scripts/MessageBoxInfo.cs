using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxInfo : MonoBehaviour
{
    [SerializeField] GameObject TextControl;
    [SerializeField] Image TextControlBackGround;
    [SerializeField] Image Exclamation;
    [SerializeField] TMP_Text Text;

    private StringBuilder stringBuilder = new StringBuilder();

    private bool IsUsed = false;

    public void CreateMessageBox(Vector2 ControlPos,int FadeInOut, string[] text)
    {
        if (IsUsed) { return; }
        IsUsed = true;

        stringBuilder.Clear();
        TextControl.transform.localPosition = ControlPos;

        foreach (string s in text)
        {
            stringBuilder.AppendLine(s);
        }

        Text.text = stringBuilder.ToString();
        StartCoroutine(MessageBoxWaiter(FadeInOut));
    }

    IEnumerator MessageBoxWaiter(int seconds)
    {
        StartCoroutine(FadeImage(false));
        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeImage(true));
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                TextControlBackGround.color = new Color(1, 1, 1, i);
                Exclamation.color = new Color(1, 1, 1, i);
                Text.color = new Color(1, 1, 1, i);
                yield return null;
            }
            TextControl.transform.localPosition = new Vector2(0, 500);
            IsUsed = false;
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                TextControlBackGround.color = new Color(1, 1, 1, i);
                Exclamation.color = new Color(1, 1, 1, i);
                Text.color = new Color(1, 1, 1, i);
                yield return null;
            }

        }
    }

    public void PromptMSGBoxHide()
    {
        StartCoroutine(FadeImage(true));
    }
}
