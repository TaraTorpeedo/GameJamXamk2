using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] bool isBeforeWorm;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,0,0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(0.5f,100,0));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (isBeforeWorm)
            {
                GameObject.Find("mato").GetComponent<wormAI>().ResetWorm();
            }
        }
    }

}
