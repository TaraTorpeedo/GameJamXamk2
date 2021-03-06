using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaustaTriggerer : MonoBehaviour
{
    [SerializeField] GameObject parent;
    public bool isDone = false;

    public GameObject target;

    public GameObject Canvas;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isDone)
            {


                if(parent.name == "sortuvat")
                {
                    GameObject[] RollingStones = GameObject.FindGameObjectsWithTag("RollingStones");
                    for(int i = 0; i < RollingStones.Length; i++)
                    {
                        RollingStones[i].GetComponent<Rigidbody2D>().simulated = true;
                    }
                    StartCoroutine(HideStones(RollingStones));

                    return;
                }

                if(parent.name == "LostChild")
                {
                    StartCoroutine(Canvas.GetComponent<StartTexts>().FinishText());
                }

                isDone = true;

                if (parent.name == "mato")
                {

                    StartCoroutine(parent.GetComponent<wormAI>().WakeTheWorm());
                    target = collision.gameObject;
                }


                parent.GetComponent<Animator>().SetBool("Animate", true);

                if(parent.GetComponent<AudioSource>() != null)
                    parent.GetComponent<AudioSource>().Play();
            }
        }
    }

    IEnumerator HideStones(GameObject[] RollingStones)
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < RollingStones.Length; i++)
        {
            RollingStones[i].gameObject.SetActive(false);
        }
    }
}
