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
    public PlayerInputManager inputManager;

    public InputActionAsset action;

    [SerializeField] Transform startPosition;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarted)
            MainMenuPanel.SetActive(false);


    }

    public void SwitchCharacter(PlayerInput input)
    {


        inputManager.playerPrefab = characters[inputManager.playerCount];
        characters[inputManager.playerCount].GetComponent<PlayerInput>().actions = action;


    }

}
