using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaustaTriggerer : MonoBehaviour
{
    [SerializeField] GameObject parent;
    bool isDone = false;
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

                isDone = true;
                parent.GetComponent<Animator>().SetBool("Animate", true);

                if(parent.GetComponent<AudioSource>() != null)
                    parent.GetComponent<AudioSource>().Play();
            }
        }
    }
}
