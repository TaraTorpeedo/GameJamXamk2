using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public bool gameIsStarted = false;
    public GameObject MainMenuPanel;

    int index = 0;
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    PlayerInputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        //index = Random.RandomRange(0, characters.Count);
        //inputManager.playerPrefab = characters[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarted)
            MainMenuPanel.SetActive(false);


    }

    public void SwitchCharacter(PlayerInput input)
    {
        //index = Random.Range(0, characters.Count);
       // inputManager.playerPrefab = characters[index];

        Debug.Log(characters);

        inputManager.playerPrefab = characters[inputManager.playerCount];

    }

}
