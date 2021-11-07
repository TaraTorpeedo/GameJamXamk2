using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormAI : MonoBehaviour
{
    bool isWake = false;
    bool dontFollow = true;
    Animator anim;

    public float speed;
    GameObject  target;

    public TaustaTriggerer trigger;

    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWake)
            return;

        if(!dontFollow)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

        if(transform.position.x > 1500)
        {
            dontFollow = true;
        }


    }
    public IEnumerator WakeTheWorm()
    {
        yield return new WaitForSeconds(3);
        target = trigger.target;
        isWake = true;
    }

    public void ResetWorm()
    {
        transform.position = startPos;
        isWake = false;
    }
}
