using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTexts : MonoBehaviour
{
    bool started = false;
    public bool canPlay = false;
    public void StartWrite()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(delay());
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(writeText());
    }

    IEnumerator DialogyDealay()
    {
        yield return new WaitForSeconds(3);
        Liibalaaba.gameObject.SetActive(false);
        canPlay = true;
    }

    float TextDelay = 0.05f;

    string tekstinpatka = "Kaupungin rakensimme Sen kaksin asutimme\nNiin alkoi taistelu virusta vastaan\nElimme amputoiden Unelman adoptoiden\nPolio levitti ydintulehdustaan\n\nUuden valon mustio sai\nEnemmän omistin kai Jos monistin perinnön tekijöitäni";

    string currentText = "";

    public Text Liibalaaba;
    public IEnumerator writeText()
    {
        string fullText = "";

        fullText = tekstinpatka;

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            Liibalaaba.text = currentText;
            yield return new WaitForSeconds(TextDelay);
        }

        yield return new WaitForSeconds(0.5f);


        StartCoroutine(DialogyDealay());
    }
}
