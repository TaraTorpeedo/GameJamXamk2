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

    //string tekstinpatka = "Once upon a time there was a group of childrens who lost in to the woods.\n It's resemble me of this forest.\nFour brave children swore an oath that they will do everything they can\n to find all the childrens who has lost or have been abandoned here. \nDeep in to the woods hide some scary secrets.\nLike these lost childrens we also live our unfinished adventures. \nSo let our new adventure beging.";
    string tekstinpatka = "a";

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
