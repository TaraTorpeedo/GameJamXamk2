using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,0,0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(0.5f,100,0));
    }
}
