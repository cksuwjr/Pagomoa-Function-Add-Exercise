using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float JumpForce = 400f;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;

    const float GroundedRadius = .2f;
    public bool isGround;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private LineRenderer line;

    public Vector3 direction = Vector2.zero;
    private Vector2 m_Velocity = Vector2.zero;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }
    private void Update()
    {
        //DetectDig();
    }
    private void FixedUpdate()
    {
        bool wasGrounded = isGround;
        isGround = false;

        Collider2D collider = Physics2D.OverlapCircle(GroundCheck.position, GroundedRadius, WhatIsGround);
        
        if (collider)
        {
            isGround = true;
            if (!wasGrounded)
            {
                anim.SetBool("Jump", false);
                OnLandEvent.Invoke();
            }
        }
        
    }

    public void Move(float direction, float speed, bool jump)
    {
        
        if (isGround)
        {

        }
        Vector3 targetVelocity = new Vector2(direction * speed, rb.velocity.y);

        if (direction != 0)
        {
            this.direction = new Vector3(direction, 0);
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        
        if(direction != 0)
            transform.localScale = new Vector3(direction, 1, 1);


        if (isGround && jump)
        {
            anim.SetBool("Jump", true);
            isGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, JumpForce));
        }
    }
    /*
    public void DetectDig()
    {
        Vector3 DigPoint = transform.position + direction;
        if (Input.GetAxisRaw("Horizontal") == 0)
            DigPoint = transform.position + Vector3.down;

        Collider2D overColliderd = Physics2D.OverlapCircle(DigPoint, 0.01f, WhatIsGround);
        if (overColliderd != null)
        {
            Ground brick = overColliderd.GetComponent<Ground>();
            Vector3Int cellPosition = Vector3Int.zero;
            if (brick)
                cellPosition = brick.tilemap.WorldToCell(DigPoint);


            if (!brick.tilemap.HasTile(cellPosition)) { line.enabled = false; return; } // 없으면 리턴


            Vector3 Adjust_Z = Vector3.zero; 
            //Adjust_Z = Vector3.back; //활성화시 블럭을 감싸게 할 수 있다.
            line.SetPosition(0, cellPosition + new Vector3(0, 0) + Adjust_Z);
            line.SetPosition(1, cellPosition + new Vector3(1, 0) + Adjust_Z);
            line.SetPosition(2, cellPosition + new Vector3(1, 1) + Adjust_Z);
            line.SetPosition(3, cellPosition + new Vector3(0, 1) + Adjust_Z);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (overColliderd != null)
            {
                overColliderd.transform.GetComponent<Ground>().Digged(DigPoint);
            }
        }
    }
    */
}
