using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public GameObject Head;
    public Transform LeftHand;
    public Transform RightHand;

    public float amount = 5;

    public void LeftTakeHead()
    {
        LeftHand.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    public void RightTakeHead()
    {
        RightHand.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

    }

    public void ThrowLeft()
    {

        LeftHand.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        Instantiate(Head, LeftHand.position, LeftHand.rotation);

    }
    public void ThrowRight()
    {

        RightHand.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        Instantiate(Head, RightHand.position, RightHand.rotation);

    }

}
