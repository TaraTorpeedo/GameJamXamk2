using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormAI : MonoBehaviour
{
    bool isWake = false;
    Animator anim;

    public float speed;
    GameObject  target;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWake)
            return;

        target = GameObject.FindWithTag("Player");
        //transform.Translate(new Vector2(speed * Time.deltaTime, speed / 2*Time.deltaTime));
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

    }
    public IEnumerator WakeTheWorm()
    {
        anim.SetBool("WakeUp", true);
        yield return new WaitForSeconds(2);
        isWake = true;
    }

}
