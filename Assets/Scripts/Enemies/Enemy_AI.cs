using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    [SerializeField] private float runSpeed;
    private float speed = 0.0f;
    private int direction = 1;
    private bool fall = false;
    private bool actualWalk = true;
    private float walkTime = 0.0f;
    private int walkType;
    private float walkTimeCoolDown = 0.0f;
    private int walkDirectionRand;
    private Rigidbody2D m_rigidbody2D;

    //Attack
    private bool enemyAttackCheck = false;
    private bool hurtAnimation = false;

    //Vector2
    private Vector2 enemyVision;
    private Vector2 enemyCenter;
    private Vector2 enemyPlatformLeft = new Vector2(-1, -1);
    private Vector2 enemyPlatformRight = new Vector2(1, -1);
    private Vector2 enemyPlatformDown = Vector2.down;
    private Vector2 enemyDirectionRight = Vector2.right;
    private Vector2 enemyDirectionLeft = Vector2.left;

    private Vector3 enemyAttackColRight;
    private Vector3 enemyAttackColLeft;

    [SerializeField] private float height;

    [Header("Layers")]
    [SerializeField] private LayerMask platform;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAttackColRight = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        enemyAttackColLeft = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyVision = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + height, this.gameObject.transform.position.z);
        enemyCenter = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f);

        RaycastHit2D hitPlatformRight = Physics2D.Raycast(enemyCenter, enemyPlatformRight, 3.0f, platform);
        RaycastHit2D hitPlatformLeft = Physics2D.Raycast(enemyCenter, enemyPlatformLeft, 3.0f, platform);
        RaycastHit2D hitPlatformDown = Physics2D.Raycast(this.gameObject.transform.position, enemyPlatformDown, 2.0f, platform);

        if (hitPlatformDown && hitPlatformLeft.collider == false && hitPlatformDown.collider.gameObject.tag == "Ground" && direction < 0)
        {
            direction = 1;
            fall = true;
        }
        else if (hitPlatformDown && hitPlatformRight.collider == false && hitPlatformDown.collider.gameObject.tag == "Ground" && direction > 0)
        {
            direction = -1;
            fall = true;
        }

        SeePlayer();

    }

    private void SeePlayer()
    {
        //Codigo para poder ver al jugador
        LayerMask mask = LayerMask.GetMask("Player");
        RaycastHit2D hitRight = Physics2D.Raycast(enemyVision, enemyDirectionRight, 6.0f, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(enemyVision, enemyDirectionLeft, 6.0f, mask);
        if (hitRight && hitRight.collider.gameObject.tag == "Player" && direction > 0 && !enemyAttackCheck && !hurtAnimation)
        {
            //Debug.Log("enemigo ve personaje derecha");
            speed = runSpeed;
        }
        else if (hitLeft && hitLeft.collider.gameObject.tag == "Player" && direction < 0 && !enemyAttackCheck && !hurtAnimation)
        {
            //Debug.Log("enemigo ve personaje izquierda");
            speed = runSpeed;
        }
        else if (!enemyAttackCheck)
        {
            randomDirection();
        }

        //Codigo para poder ver al jugador por detras
        hitRight = Physics2D.Raycast(enemyVision, enemyDirectionRight, 3.5f, mask);
        hitLeft = Physics2D.Raycast(enemyVision, enemyDirectionLeft, 3.5f, mask);
        if (hitRight && hitRight.collider.gameObject.tag == "Player" && direction < 0 && !enemyAttackCheck && !hurtAnimation)
        {
            direction = 1;
        }
        else if (hitLeft && hitLeft.collider.gameObject.tag == "Player" && direction > 0 && !enemyAttackCheck && !hurtAnimation)
        {
            direction = -1;
        }

        //Codigo para para cuando el jugador este muy cerca
        Vector2 playerRight = new Vector2(1, 0);
        Vector2 playerLeft = new Vector2(-1, 0);

        LayerMask playerMask = LayerMask.GetMask("Player");
        RaycastHit2D playerHitRight = Physics2D.Raycast(enemyVision, playerRight, 1.5f, playerMask);
        RaycastHit2D playerHitLeft = Physics2D.Raycast(enemyVision, playerLeft, 1.5f, playerMask);

        if (playerHitRight && playerHitRight.collider.gameObject.tag == "Player" && direction > 0)
        {
            speed = 0.0f;
        }
        else if (playerHitLeft && playerHitLeft.collider.gameObject.tag == "Player" && direction < 0)
        {
            speed = 0.0f;
        }

        //Controla la direccion del enemigo
        m_rigidbody2D.velocity = new Vector2(direction * speed, 0);
        ChangeAttackDirection(direction);

    }

    //Cambiara la colisión de la bola de derecha a izquierda segun hacia donde este mirando
    void ChangeAttackDirection(int currentDirection)
    {
        switch (currentDirection)
        {
            case 1:
                this.gameObject.transform.localScale = enemyAttackColRight;
                break;
            case -1:
                this.gameObject.transform.localScale = enemyAttackColLeft;
                break;
        }
    }

    void randomDirection()
    {
        walkTime += Time.deltaTime;
        if (actualWalk)
        {
            walkType = Random.Range(1, 6);
            walkTimeCoolDown = walkTime;
            walkDirectionRand = Random.Range(0, 2);
            actualWalk = false;
            //Debug.Log("Random");
        }
        switch (walkType)
        {
            case 1:
                if (walkTime - walkTimeCoolDown >= 1.0f)
                {
                    speed = 0;
                    walkType = 0;
                    walkTimeCoolDown = walkTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 2:
                if (walkTime - walkTimeCoolDown >= 2.0f)
                {
                    speed = 0;
                    walkType = 0;
                    walkTimeCoolDown = walkTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 3:
                if (walkTime - walkTimeCoolDown >= 3.0f)
                {
                    speed = 0;
                    walkType = 0;
                    walkTimeCoolDown = walkTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 4:
                if (walkTime - walkTimeCoolDown >= 4.0f)
                {
                    speed = 0;
                    walkType = 0;
                    walkTimeCoolDown = walkTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 5:
                if (walkTime - walkTimeCoolDown >= 5.0f)
                {
                    speed = 0;
                    walkType = 0;
                    walkTimeCoolDown = walkTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            default:
                if (walkTime - walkTimeCoolDown >= 3.0f)
                {
                    actualWalk = true;
                    fall = false;
                }
                break;
        }
        switch (walkDirectionRand)
        {
            case 0:
                if (!fall)
                {
                    direction = 1;
                }
                break;
            case 1:
                if (!fall)
                {
                    direction = -1;
                }
                break;
        }
    }


}
