using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D playerCollider;

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

    Vector3 spawnPoint;

    Camera cam;

    GameManager gm;


    void Start()
    {
        gm = GameObject.Find("PlayerManager").GetComponent<GameManager>();
        gm.gameIsStarted = true;

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
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
            Run();
            isTurning = true;
            turnLeft = false;
            //transform.localScale = Vector3.one;
        }

        else if (rb.velocity.x < 0f)
        {
            Run();
            isTurning = true;
            turnLeft = true;
            //transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        else
            Idle();

        if (isTurning)
        {
            //isTurning = false;
            if(turnLeft)
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, -1, Time.deltaTime * 10), 1, 1);
            else
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 1, Time.deltaTime * 10), 1, 1);
        }

        if (rb.velocity.y != 0)
        {

            Jump();
        }
        if (!ableToMove)
            return;

        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);



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
        inputX = value.ReadValue<Vector2>().x;
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

    IEnumerator Spawn()
    {

        yield return new WaitForSeconds(2);

        //Return
        transform.position = spawnPoint;
        ableToMove = true;
        Idle();
    }

    #region Animations
    void ResetAnimation()
    {
        anim.SetBool("isLookUp", false);
        anim.SetBool("isRun", false);
        anim.SetBool("isJump", false);
    }
    public void Idle()
    {
        ResetAnimation();
        anim.SetTrigger("idle");
    }
    public void Attack()
    {
        ResetAnimation();
        anim.SetTrigger("attack");
    }
    public void TripOver()
    {
        ResetAnimation();
        anim.SetTrigger("tripOver");
    }
    public void Hurt()
    {
        ResetAnimation();
        anim.SetTrigger("hurt");
    }
    public void Die()
    {
        ResetAnimation();
        anim.SetTrigger("die");
    }
    public void LookUp()
    {
        ResetAnimation();
        anim.SetBool("isLookUp", true);
    }
    public void Run()
    {
        ResetAnimation();
        anim.SetBool("isRun", true);

    }
    public void Jump()
    {
        ResetAnimation();
        anim.SetBool("isJump", true);

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
            StartCoroutine(Spawn());
            Die();
        }

        if(other.tag == "FinishLine")
        {
            ableToMove = false;
            StartCoroutine(Voitto());
        }
    }

    IEnumerator Voitto()
    {
        Idle();
        yield return new WaitForSeconds(3);
        Debug.Log("Kävele");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "CameraBounds")
        {
            ableToMove = false;
            StartCoroutine(Spawn());
            Die();
        }
    }

}
