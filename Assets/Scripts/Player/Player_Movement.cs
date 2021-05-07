using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Inputs
    private float horizontalMove = 0f;
    private bool interactButtonInput = false;
    //private bool attackButton = false;  ////Mirar si se usa
    private bool jmpBtn = false;
    private bool jmpBtnDown = false;
    private bool rollBtn = false;


    [Header("Movement")]
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private int maxNumJumps = 2;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float jumpVelocity = 15;
    [SerializeField] private float fallVelocity = -10.0f;
    [SerializeField] private float jumpGravityPercentage = 0.7f;
    [SerializeField] private float rollSpeed = 10.0f;
    [SerializeField] private float dashSpeed = 15.0f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private bool isFacingLeft = true;
    private bool oneWayPlatform = false;
    private float startDashTime = 0.0f;


    [Header("Transforms")]
    [SerializeField] private Transform m_GroundCheck = null;

    private Rigidbody2D m_Rigidbody2D = null;
    private bool m_Grounded = true;  //Cambiar a private
    public int totalJumps;  //Cambiar a private
    private Vector2 leftSide = new Vector2(1, 0);
    private Vector2 rightSide = new Vector2(-1, 0);
    private bool doubleJump = false;
    private Vector2 dashDirection = Vector2.zero;
    private bool dashDirectionDecided = false;
    private Animator m_Animator = null;

    [HideInInspector] public enum State { NORMAL, DODGEROLL};
    [HideInInspector] public State state;

    [Header("Layers")]
    [SerializeField] private LayerMask m_WhatIsGround = 0;
    [Space]
    [SerializeField] private float gravityScale = 10.0f;
    [SerializeField] private float k_GroundedRadius = 0.2f;
    [SerializeField] private GameObject shop = null;
    private bool canEnterShop = false;
    //[SerializeField] private GameObject shopEntrance = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject anotherPlayer = GameObject.FindGameObjectWithTag("Player");
        if(anotherPlayer != null && anotherPlayer != this.gameObject)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        totalJumps = maxNumJumps;
        state = State.NORMAL;
    }

    private void Update()
    {
        if(!this.gameObject.GetComponent<Player_Attack>().GetPlayerLiveState())
        {
            ControlInputs();

            OpenShop();
            m_Animator.SetFloat("Velocity_Falling", m_Rigidbody2D.velocity.y);
        }
            
        if(shop == null)
        {
            shop = GameObject.Find("Canvas").transform.Find("Shop").gameObject;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.gameObject.GetComponent<Player_Attack>().GetPlayerLiveState())
        {
            Collider2D[] collidersGround = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersGround.Length; i++)
            {
                if (collidersGround[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    m_Animator.SetBool("InAir", !m_Grounded);
                    totalJumps = maxNumJumps;
                    doubleJump = false;
                }
            }



            Salto();
            if (state != State.DODGEROLL)
                Movement();

            ControlGravity();
            Dash();
        }
    }

    void Salto()
    {
        //RaycastHit2D downCol = Physics2D.Raycast(this.gameObject.transform.position, colGround, 0.05f, floorLayer);
        //RaycastHit2D stairsCol = Physics2D.Raycast(this.gameObject.transform.position, colGround, 0.45f, floorLayer);

        if (jmpBtn/*Input.GetButtonDown("Jump")*/ /*&& totalJumps > 0*/)
        {
            if(m_Grounded)
            {
                m_Animator.SetTrigger("Jump");
                m_Rigidbody2D.AddForce(new Vector2(m_Rigidbody2D.velocity.x, jumpForce), ForceMode2D.Impulse);

                m_Grounded = false;
                m_Animator.SetBool("InAir", !m_Grounded);
                doubleJump = true;
                jmpBtnDown = false;
            }
            else if(jmpBtnDown && totalJumps - 1 > 0 && doubleJump)
            {
                m_Rigidbody2D.velocity = (new Vector2(m_Rigidbody2D.velocity.x, jumpVelocity));
                totalJumps--;
                jmpBtnDown = false;
            }
            
        }
        //if (m_Grounded)
        //{
            
        //}
    }

    void Movement()
    {
        if(horizontalMove > 0 && isFacingLeft)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if(horizontalMove < 0 && !isFacingLeft)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        if(!this.gameObject.GetComponent<Player_Attack>().GetDefendState())
        {
            m_Rigidbody2D.velocity = new Vector2(horizontalMove * runSpeed, m_Rigidbody2D.velocity.y);
        }
        
        m_Animator.SetFloat("Velocity", Mathf.Abs(m_Rigidbody2D.velocity.x));

        //if (Input.GetKey(KeyCode.D))
        //{
        //    m_Rigidbody2D.velocity = new Vector2(runSpeed, m_Rigidbody2D.velocity.y);
        //    this.gameObject.transform.localScale = rightSide;
        //    keyReleased = false;

        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    m_Rigidbody2D.velocity = new Vector2(-runSpeed, m_Rigidbody2D.velocity.y);
        //    this.gameObject.transform.localScale = leftSide;
        //    keyReleased = false;
        //}

        //if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D)))
        //{
        //    keyReleased = true;
        //}

        //if (keyReleased)
        //{
        //    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);

        //}
    }

    private void ControlGravity()
    {
        if (jmpBtn && m_Rigidbody2D.velocity.y > 0)
        {
            m_Rigidbody2D.gravityScale = gravityScale * jumpGravityPercentage;
        }

        else if (m_Rigidbody2D.velocity.y <= fallVelocity)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, fallVelocity);
        }

        else
        {
            m_Rigidbody2D.gravityScale = gravityScale;
        }
    }

    private void ControlInputs()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        interactButtonInput = Input.GetButton("Interact");
        jmpBtn = Input.GetButton("Jump");
        oneWayPlatform = Input.GetButton("Down");

        //jmpBtnDown = Input.GetButtonDown("Jump");
        if (Input.GetButtonDown("Jump"))
        {
            jmpBtnDown = true;
        }

        if(Input.GetButtonDown("Roll") && m_Grounded)
        {
            rollBtn = true;
        }


    }

    private void ChangeTotalJumps(int newTotalJumps)
    {
        maxNumJumps = newTotalJumps;
        totalJumps = maxNumJumps;
    }

    private void DodgeRoll()
    {
        if(rollBtn)
        {
            m_Rigidbody2D.AddForce(new Vector2(rollSpeed * horizontalMove, m_Rigidbody2D.velocity.y), ForceMode2D.Impulse);
            m_Rigidbody2D.velocity = new Vector2(horizontalMove * runSpeed * 30, m_Rigidbody2D.velocity.y);
            rollBtn = false;
        }
    }

    private void Dash()
    {
        //(horizontalMove > 0.0f || horizontalMove < 0.0f)
        if (rollBtn && state == State.NORMAL && m_Grounded && Mathf.Abs(m_Rigidbody2D.velocity.x) > 0.0f)
        {
            state = State.DODGEROLL;

            if (!dashDirectionDecided)
            {
                m_Animator.SetTrigger("Roll");
                dashDirection = new Vector2(horizontalMove * dashSpeed, m_Rigidbody2D.velocity.y);
                dashDirectionDecided = true;
            }

            m_Rigidbody2D.velocity = dashDirection;

            if(startDashTime >= dashTime)
            {
                rollBtn = false;
                state = State.NORMAL;
                startDashTime = 0.0f;
                dashDirectionDecided = false;
            }
            else
            {
                startDashTime += Time.deltaTime;
            }

        }
        else if(rollBtn && !m_Grounded)
        {
            rollBtn = false;
            state = State.NORMAL;
            startDashTime = 0.0f;
            dashDirectionDecided = false;
        }
        else if(rollBtn)
        {
            rollBtn = false;
            startDashTime = 0.0f;
            state = State.NORMAL;
            dashDirectionDecided = false;
        }
        
    }

    private void OpenShop()
    {
        if(interactButtonInput && canEnterShop/*/shopEntrance.GetComponent<Shop_Entrance>().canEnterShop*/)
        {
            shop.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public bool IsFacingLeft() { return isFacingLeft; }

    public Rigidbody2D GetRigidBody() { return m_Rigidbody2D; }

    public bool GetOneWayPlatformState() { return oneWayPlatform; }

    public void SetCanEnterShop(bool state)
    {
        canEnterShop = state;
    }

    void OnDrawGizmosSelected()
    {
        if (m_GroundCheck == null)
            return;
        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
        //Gizmos.DrawWireSphere(this.gameObject.transform.position, 0.1f);
    }

}
