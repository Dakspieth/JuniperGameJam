using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public InputAction moveControl;
    public float jumpForce;
    public float jumpTime; // how long to stay in air while holding jump
        float baseJumpTime;
        bool readJump = true;
    public float coyoteTime;
        float baseCoyoteTime;
    public float jumpBufferTime;
        float baseJumpBufferTime;
    public InputAction jumpControl;
    public float sprintSpeedMult;
    public InputAction sprintControl;
        [HideInInspector]
        public InputAction sprintControlStored;
    [HideInInspector]
    public Rigidbody2D rb;
    float moveDir;
    bool groundCollided;
    
    public bool jetpack;
    public Animator animator;
    [HideInInspector]
    public bool noRunningInHall;
    SpinToWin stw;
    [HideInInspector]
    public bool seriousSamPaperCollected;
    [HideInInspector]
    public GameObject seriousSamPaper = null;

    public int jumps = 0;
        [HideInInspector]
        public int maxJumps = int.MaxValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseJumpTime = jumpTime;
        baseCoyoteTime = coyoteTime;
        baseJumpBufferTime = jumpBufferTime;
        sprintControlStored = sprintControl;
        sprintControl.ChangeBinding(0).Erase();
        stw = GameObject.FindWithTag("Spinner").GetComponent<SpinToWin>();
        seriousSamPaper = GameObject.FindWithTag("SeriousPaper");
    }

    void OnEnable()
    {
        moveControl.Enable();
        jumpControl.Enable();
        sprintControl.Enable();
    }
    void OnDisable()
    {
        moveControl.Disable();
        jumpControl.Disable();
        sprintControl.Disable();
    }
        
    void FixedUpdate()
    {
        groundCollided = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2),
                                              new Vector2(Mathf.Abs(transform.localScale.x)-0.02f, 0.25f), 0f, LayerMask.GetMask("Ground"));
    
        if(groundCollided) {
            coyoteTime = baseCoyoteTime;
        } else {
            coyoteTime -= coyoteTime > -1 ? Time.deltaTime : 0;
        }

        if(jumpControl.ReadValue<float>() == 1 && readJump && !groundCollided) {
            jumpBufferTime = baseJumpBufferTime;
        }

        moveDir = moveControl.ReadValue<float>(); // left right

        transform.localScale = new Vector2(moveDir != 0 ? moveDir*Mathf.Abs(transform.localScale.x) : transform.localScale.x, transform.localScale.y);
        

    // instead of grounded bool + press jump button
        if(coyoteTime>0 && (jumpBufferTime>0 || (jumpControl.ReadValue<float>() == 1 && readJump))) {
            jumpTime = baseJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumps++;
            if(jumps > maxJumps)
            {
                StartCoroutine("You Ran Out of Jumps");
            }
        } else if(!groundCollided && jumpControl.ReadValue<float>() == 1 && jumpTime > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if(jumpControl.ReadValue<float>() == 1) {
            readJump = false;
        } else {
            readJump = true;
        }

        jumpTime -=  jetpack ? 0f : Time.deltaTime;
        jumpBufferTime -= Time.deltaTime;

        if(groundCollided) {
            jumpBufferTime = -1;
        }
        if(groundCollided)
        {
            animator.SetBool("ground", true);
        } else
        {
            animator.SetBool("ground", false);
            animator.SetTrigger("jump");
        }
        
        if(groundCollided && Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }
        
        Vector2 moveSpeedDir = new Vector2(moveDir * moveSpeed * (sprintControl.ReadValue<float>() != 0 ? sprintSpeedMult : 1), rb.linearVelocity.y);
        animator.speed = sprintControl.ReadValue<float>() != 0 ? 1.5f : 1f;
        rb.linearVelocity = moveSpeedDir;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "TheHall" && noRunningInHall && sprintControl.ReadValue<float>() != 0)
        {
            StartCoroutine(stw.spinOverTime("You Ran in the Hall"));
        }
        if(col.tag == "SeriousPaper")
        {
            seriousSamPaperCollected = true;
            seriousSamPaper.SetActive(false);
        }
        if(col.tag == "Boss" && seriousSamPaperCollected)
        {
            print("win");
        }

    }

}
