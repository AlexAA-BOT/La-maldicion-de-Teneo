using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{

    //EnemyID
    [HideInInspector] public enum EnemyID { GREENSKELETON, GOBLIN, OBSERVER };
    public EnemyID enemyID = EnemyID.GREENSKELETON;

    [Header("Health")]
    [SerializeField] private int enemyHealth = 100;


    //Movement
    [Header("Movement")]
    [SerializeField] private float runSpeed = 1.5f;
    private float speed = 0.0f;
    [HideInInspector] public int direction = 1;
    private bool fall = false;
    private bool actualWalk = true;
    private float walkTime = 0.0f;
    private int walkType = 0;
    private float walkTimeCoolDown = 0.0f;
    private int walkDirectionRand = 0;
    private Rigidbody2D m_rigidbody2D = null;
    private bool isFacingRight = false;

    //Attack
    private bool enemyAttackCheck = false;
    private bool hurtAnimation = false;
    [Header("Attack")]
    [SerializeField] private float distanceToAttack = 1.5f;
    [SerializeField] private float startAttack = 0.0f;
    [SerializeField] private float endAttack = 2.0f;
    [SerializeField] private float endAttackAnimation = 3.0f;
    [SerializeField] private Transform attack_Point = null;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private int damageAttack = 20;
    [SerializeField] private float hurtCoolDown = 1.8f;
    [Space]
    private float timerAttack = 0.0f;
    private float hurtTime = 0.0f;
    private bool attackOneTime = false;


    //Vector2
    private Vector2 enemyVision = Vector2.zero;
    private Vector2 enemyCenter = Vector2.zero;
    private Vector2 enemyPlatformLeft = new Vector2(-1, -1);
    private Vector2 enemyPlatformRight = new Vector2(1, -1);
    private Vector2 enemyPlatformDown = Vector2.down;
    private Vector2 enemyDirectionRight = Vector2.right;
    private Vector2 enemyDirectionLeft = Vector2.left;

    private Vector3 enemyAttackColRight = Vector3.zero;
    private Vector3 enemyAttackColLeft = Vector3.zero;

    [SerializeField] private float height = 1.0f;
    [SerializeField] private GameObject gameObjectMoney = null;
    [SerializeField] private bool dropMoney = true;  //// Eliminar a futuro

    [Header("Layers")]
    [SerializeField] private LayerMask platform = 0;
    [SerializeField] private LayerMask playerMask = 0;
    [Space]

    //GameObjects
    private GameObject bestiarioCount = null;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAttackColRight = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        enemyAttackColLeft = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        bestiarioCount = GameObject.FindGameObjectWithTag("BestiarioCount");
    }

    private void Update()
    {
        IsFacingRight();
        GetDamage(0);
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
        EnemyAttack();

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
        else if (!enemyAttackCheck && !hurtAnimation)
        {
            randomDirection();
        }
        else
        {
            speed = 0.0f;
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
        //Vector2 playerRight = new Vector2(1, 0);   //--- Codigo para eliminar
        //Vector2 playerLeft = new Vector2(-1, 0);   //--- Codigo para eliminar

        LayerMask playerMask = LayerMask.GetMask("Player");
        RaycastHit2D playerHitRight = Physics2D.Raycast(enemyVision, Vector2.right, distanceToAttack, playerMask);
        RaycastHit2D playerHitLeft = Physics2D.Raycast(enemyVision, Vector2.left, distanceToAttack, playerMask);

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

    private void IsFacingRight()
    {
        if(direction == 1)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    ////-----AttacK functions

    void EnemyAttack()
    {
        //Vector2 playerRight = new Vector2(1, 0);   //--- Codigo a eliminar
        //Vector2 playerLeft = new Vector2(-1, 0);   //--- Codigo a eliminar

        RaycastHit2D playerHitRight = Physics2D.Raycast(enemyVision, Vector2.right, distanceToAttack, playerMask);
        RaycastHit2D playerHitLeft = Physics2D.Raycast(enemyVision, Vector2.left, distanceToAttack, playerMask);
        if ((playerHitRight && playerHitRight.collider.gameObject.tag == "Player" && direction > 0) || enemyAttackCheck)
        {
            if (timerAttack <= 0.0f && !hurtAnimation)  //Se activa la animacion de ataque
            {
                //Animator.SetTrigger("Attack");
                timerAttack += Time.deltaTime;
                enemyAttackCheck = true;
            }
            else if (timerAttack >= startAttack && timerAttack <= endAttack && !hurtAnimation)  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
            {
                attack();
                timerAttack += Time.deltaTime;
                Debug.Log("Ataca");
            }
            else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
            {
                timerAttack = 0.0f;
                enemyAttackCheck = false;
                attackOneTime = false;
            }
            else
            {
                timerAttack += Time.deltaTime;
            }

        }
        else if ((playerHitLeft && playerHitLeft.collider.gameObject.tag == "Player" && direction < 0) || enemyAttackCheck)
        {
            if (timerAttack <= 0.0f && !hurtAnimation)  //Se activa la animacion de ataque
            {
                //Animator.SetTrigger("Attack");
                timerAttack += Time.deltaTime;
                enemyAttackCheck = true;
            }
            else if (timerAttack >= startAttack && timerAttack <= endAttack && !hurtAnimation)  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
            {
                attack();
                timerAttack += Time.deltaTime;
                Debug.Log("Ataca");
            }
            else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
            {
                timerAttack = 0.0f;
                enemyAttackCheck = false;
                attackOneTime = false;
            }
            else
            {
                timerAttack += Time.deltaTime;
            }
        }

        if(hurtAnimation)
        {
            timerAttack = 0.0f;
            enemyAttackCheck = false;
        }

    }

    void attack()
    {
        Collider2D[] objectsInEnemyAttack = Physics2D.OverlapCircleAll(attack_Point.position, attackRange, playerMask);
        foreach (Collider2D colliders in objectsInEnemyAttack)
        {
            if (colliders.gameObject.tag == "Player" && !attackOneTime)
            {
                if(colliders.GetComponent<Player_Attack>().defendState == true && colliders.GetComponent<Player_Movement>().IsFacingLeft() == isFacingRight)
                {
                    colliders.GetComponent<Player_Attack>().GetStamina(damageAttack);
                    attackOneTime = true;
                }
                else
                {
                    colliders.GetComponent<Player_Attack>().GetDamage(damageAttack);
                }
            }
        }
    }

    //Recibe daño del jugador (Player)
    public void GetDamage(int playerDamage)
    {
        if(!hurtAnimation) enemyHealth -= playerDamage;

        if (enemyHealth <= 0)
        {
            Debug.Log("Enemigo muerto");
            int numRandom = Random.Range(1, 100);
            if(numRandom <= 50 && dropMoney)
            {
                Instantiate(gameObjectMoney, this.transform);
            }
            dropMoney = false;
            bestiarioCount.GetComponent<Bestiario_Count>().AddToDeathCount(enemyID);
        }
        else
        {
            if (playerDamage > 0 || hurtAnimation)
            {
                if (hurtTime <= 0)
                {
                    //Animator.SetTrigger("Hurt");
                    hurtAnimation = true;
                    hurtTime += Time.deltaTime;
                }
                else if (hurtTime >= hurtCoolDown)
                {
                    hurtAnimation = false;
                    hurtTime = 0.0f;
                }
                else
                {
                    hurtTime += Time.deltaTime;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attack_Point == null)
            return;
        Gizmos.DrawWireSphere(attack_Point.position, attackRange);
        //Gizmos.DrawWireSphere(this.gameObject.transform.position, 0.1f);
    }

}
