using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;//, jumpForce;
    [SerializeField] Transform groundCheck;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool isJumping;
    private bool jumpKeyHeld;


    float inputX;

    MultipleTargetCamera CameraController;

    bool isTurning = false;
    bool turnLeft = false;

    bool ableToMove = true;

    Vector3 spawnPoint;

    Camera cam;

    public float gravityMultiplier;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = UnityEngine.Camera.main;

        spawnPoint = transform.position;

        CameraController = GameObject.Find("Main Camera").GetComponent<MultipleTargetCamera>();

        CameraController.AddPlayer(transform);

            
    }

    private void Update()
    {
        if (!ableToMove)
            return;

        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);

        //if (!isGrounded)
            //rb.velocity += new Vector2(0, -9.81f * gravityMultiplier);

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
                float jumpForce = CalculateJumpForce(Physics2D.gravity.magnitude, 5.0f);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(Vector2.up * jumpForce * rb.mass, ForceMode2D.Impulse);

            }
        }
        else
        {
            jumpKeyHeld = false;
        }
    }

     //JUUHJJOOHH....
    private void FixedUpdate()
    {
        if (isJumping)
        {
            if (!jumpKeyHeld && Vector2.Dot(rb.velocity, Vector2.up) > 0)
            {
                rb.AddForce(new Vector2(0, 9.81f) * rb.mass);
            }
        }
    }

    public static float CalculateJumpForce(float gravityStreght, float jumpHeight)
    {
        return Mathf.Sqrt(2 * gravityStreght * jumpHeight);
    }


    IEnumerator OutOfBounds()
    {
        Debug.Log("back");
        yield return new WaitForSeconds(1);
        Debug.Log("BACK");
        yield return new WaitForSeconds(1);
        Debug.Log("BACK!!!");
        yield return new WaitForSeconds(1);
        ableToMove = false;
        StartCoroutine(Spawn());
        Die();
    }

    IEnumerator Spawn()
    {

        yield return new WaitForSeconds(2);
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
            Debug.Log("NewCheckpoint");
        }
        if(other.tag == "KillZone")
        {
            ableToMove = false;
            StartCoroutine(Spawn());
            Die();
        }
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
