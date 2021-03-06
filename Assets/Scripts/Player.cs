using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rb;
    CapsuleCollider2D playerCollider;

    [SerializeField] float moveSpeed, jumpForce;
    [SerializeField] Transform groundCheck;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool isJumping;
    private bool jumpKeyHeld;

    public float gravityMultiplier = 0.01f;
    float inputX;

    MultipleTargetCamera CameraController;

    bool isTurning = false;
    bool turnLeft = false;

    bool ableToMove = true;
    bool isDead = false;

    bool run = false;

    Vector3 spawnPoint;

    Camera cam;

    GameManager gm;

    GameObject FinishLine;

    [SerializeField] float CharacterScale = 0.5f;

    void Start()
    {
        gameObject.transform.position = new Vector3(-5, -2f, 0);

        gm = GameObject.Find("PlayerManager").GetComponent<GameManager>();
        gm.gameIsStarted = true;

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        cam = UnityEngine.Camera.main;

        spawnPoint = transform.position;

        CameraController = GameObject.Find("Main Camera").GetComponent<MultipleTargetCamera>();
        CameraController.AddPlayer(transform);

            
    }

    private void Update()
    {
        if (!isGrounded)
        {
            isJumping = true;
            rb.velocity += new Vector2(0, -9.81f * gravityMultiplier);
        }
        else
            isJumping = false;


        if (rb.velocity.x > 0f)
        {
            if (rb.velocity.x > 10)
                Run();
            else
                Walk();
        }

        else if (rb.velocity.x < 0f)
        {
            if (rb.velocity.x < -10)
                Run();
            else
                Walk();

        }
        else
            Idle();


        if (isTurning)
        {
            if (turnLeft)
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, -CharacterScale, Time.deltaTime * 10), CharacterScale, CharacterScale);
            else
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, CharacterScale, Time.deltaTime * 10), CharacterScale, CharacterScale);
        }

        if (rb.velocity.y != 0)
        {

            Jump();
        }
        if (!ableToMove)
            return;

        if (run)
        {
            rb.velocity = new Vector2(inputX * moveSpeed * Time.deltaTime * 1.4f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(inputX * moveSpeed *Time.deltaTime, rb.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);

    }

    private void LateUpdate()
    {
        if (isDead)
        {
            Die();
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
            anim.SetBool("isDead", false);

    }

    private void FixedUpdate()
    {
        FinalCollisionCheck();
    }
    private void FinalCollisionCheck()
    {
        // Get the velocity
        Vector2 moveDirection = new Vector2(rb.velocity.x * Time.fixedDeltaTime, 0.2f);

        // Get bounds of Collider
        //var bottomRight = new Vector2(playerCollider.bounds.max.x, player.collider.bounds.max.y);
        //var topLeft = new Vector2(playerCollider.bounds.min.x, player.collider.bounds.min.y);
        var bottomRight = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.max.y);
        var topLeft = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y);

        // Move collider in direction that we are moving
        bottomRight += moveDirection;
        topLeft += moveDirection;

        // Check if the body's current velocity will result in a collision
        LayerMask mask = LayerMask.GetMask("Wall");
        if (Physics2D.OverlapArea(topLeft, bottomRight, mask))
        {
            // If so, stop the movement
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }


    public void Move(InputAction.CallbackContext value)
    {
        if (ableToMove)
        {

            inputX = value.ReadValue<Vector2>().x;
            if (!isDead)
            {
                if (value.ReadValue<Vector2>().x < 0)
                {
                    isTurning = true;
                    turnLeft = true;
                }
                else if (value.ReadValue<Vector2>().x > 0)
                {
                    isTurning = true;
                    turnLeft = false;
                }
            }
        }

    }
    public void Jump(InputAction.CallbackContext value)
    {
        if(value.performed && ableToMove)
        {
            jumpKeyHeld = true;
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            }
        }
        else
        {
            jumpKeyHeld = false;
        }
    }

    public void Boost(InputAction.CallbackContext value)
    {
        if (value.performed && ableToMove)
        {
            run = true;
        }
        else
        {
            
            run = false;
        }
        
    }

    IEnumerator Spawn()
    {
        isDead = true;
        yield return new WaitForSeconds(2);

        //Return
        isDead = false;
        if(gm.inputManager.playerCount > 1)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < players.Length; i++)
            {
                if(!players[i].GetComponent<Player>().isDead)
                    transform.position = players[i].transform.position;
                else
                    transform.position = spawnPoint;
            }
        }
        else
        {
            transform.position = spawnPoint;
        }
        ableToMove = true;
        Idle();
    }

    #region Animations
    void ResetAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdling", false);
        //anim.SetBool("isDead", false);
    }
    public void Idle()
    {
        GetComponent<AudioSource>().volume = 0;
        ResetAnimation();
        anim.SetBool("isIdling", true); 
    }

    public void Die()
    {
        ResetAnimation();
        anim.SetBool("isDead", true); 
    }
    public void Walk()
    {
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().pitch = 1f;
        ResetAnimation();
        anim.SetBool("isWalking", true);
    }
    public void Run()
    {
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().pitch = 1.4f;
        ResetAnimation();
        anim.SetBool("isRunning", true);

    }
    public void Jump()
    {
        GetComponent<AudioSource>().volume = 0;
        ResetAnimation();
        //anim.SetBool("isJumping", true);
        anim.SetBool("isWalking", true);
    }

    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "CheckPoint")
        {
            spawnPoint = transform.position;
        }
        if(other.tag == "KillZone")
        {
            ableToMove = false;
            if(!isDead)
                StartCoroutine(Spawn());

            Die();
        }

        if(other.tag == "FinishLine")
        {
            ableToMove = false;
            StartCoroutine(Voitto());
        }

        if (other.tag == "CameraBounds")
        {

            ableToMove = true;

        }

        if(other.tag == "LostChildTrigger")
        {
            ChildFound();
        }
    }

    IEnumerator Voitto()
    {
        Idle();
        yield return new WaitForSeconds(3);
        Debug.Log("K?vele");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "CameraBounds")
        {
            isDead = true;
            Debug.Log("Camera Bounds");
            ableToMove = false;
            if(isDead)
                StartCoroutine(Spawn());
            Die();
        }
    }

    void ChildFound()
    {
        ableToMove = false;
        //Transform LostChild;
        //LostChild = GameObject.Find("LostChild").transform;
        //transform.Translate(LostChild.position.x, LostChild.position.y, moveSpeed * Time.deltaTime);
        //StartCoroutine(TheEnd());
    }

    //IEnumerator TheEnd()
    //{
    //
    //    yield return new WaitForSeconds(3);
    //    //rb.velocity = new Vector2(0, rb.velocity.y);
    //    //T?h?n loppu tekstit
    //
    //
    //    yield return new WaitForSeconds(3);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    //
    //}

}
