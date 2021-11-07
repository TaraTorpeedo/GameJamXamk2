using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTexts : MonoBehaviour
{
    bool started = false;
    public bool canPlay = false;

    [SerializeField] GameObject BlackPanel;
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
        string tekstinpatka = "Once upon a time there was a group of childrens who lost in to the woods.\n It's resemble me of this forest.\nFour brave children swore an oath that they will do everything they can\n to find all the childrens who has lost or have been abandoned here. \nDeep in to the woods hide some scary secrets.\nLike these lost childrens we also live our unfinished adventures. \nSo let our new adventure beging.";
        StartCoroutine(writeText(tekstinpatka,false));
    }

    IEnumerator DialogyDealay()
    {
        yield return new WaitForSeconds(3);
        Liibalaaba.gameObject.SetActive(false);
        canPlay = true;
    }

    float TextDelay = 0.05f;

    

    string currentText = "";

    public Text Liibalaaba;
    public IEnumerator writeText(string tekstinpatka, bool isEnd)
    {
        string fullText = "";

        fullText = tekstinpatka;

        if (isEnd)
        {
            Liibalaaba.text = "";
            Liibalaaba.gameObject.SetActive(true);
        }

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            Liibalaaba.text = currentText;
            yield return new WaitForSeconds(TextDelay);
        }

        yield return new WaitForSeconds(0.5f);

        if (!isEnd)
            StartCoroutine(DialogyDealay());
        else
        {
            BlackPanel.GetComponent<Animator>().SetBool("BlackIt", true);
            //Sammuta tausta ‰‰ni
            GameObject.Find("mastersound").SetActive(false);
            yield return new WaitForSeconds(3.5f);
            GetComponent<AudioSource>().Play();
            //Roll the credits
            yield return new WaitForSeconds(50);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        }
    }

    public IEnumerator FinishText()
    {
        yield return new WaitForSeconds(2);
        string tekstinpatka = "All roads lead to somewhere. We all feel lost sometimes.\nSo if you ever feel lost I belive that these lost childrens will find you.\nAnd keep you safe this time.";
        StartCoroutine(writeText(tekstinpatka,true));
    }
}
