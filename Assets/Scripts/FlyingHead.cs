using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHead : MonoBehaviour
{
    private void Start()
    {
        int force;
        force = Random.Range(500, 1000);
        StartCoroutine(destroy());
        GetComponent<Rigidbody2D>().AddForce(transform.right * force);
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
