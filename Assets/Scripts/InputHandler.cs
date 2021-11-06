using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Player player;
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform.position, transform.rotation).GetComponent<Player>();


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
