using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy_AI : MonoBehaviour
{

    //EnemyID
    public Enemy_AI.EnemyID enemyID = Enemy_AI.EnemyID.GREENSKELETON;

    [Header("Health")]
    [SerializeField] private int enemyHealth = 100;


    //Movement
    [Header("Movement")]
    [SerializeField] private float runSpeed = 1.5f;
    [SerializeField] private float distanceFromGround = 2.0f;
    private float speed = 0.0f;
    private bool actualFly = true;
    private float flyTime = 0.0f;
    private int flyType = 0;
    private float flyTimeCoolDown = 0.0f;
    private int flyDirectionRand = 0;
    private int flyDirection = 1;
    private Rigidbody2D m_rigidbody2D = null;
    private bool isFacingRight = false;
    private bool goBack = false;
    [SerializeField] private float goBackCoolDown = 0.8f;
    private float goBackTime = 0.0f;

    //Attack
    private bool enemyAttackCheck = false;
    private bool hurtAnimation = false;
    [Header("Attack")]
    [SerializeField] private float distanceToAttack = 1.0f;
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

    //Floats
    private float distance = 0.0f;

    //Vector2
    private Vector3 enemyAttackColRight = Vector3.zero;
    private Vector3 enemyAttackColLeft = Vector3.zero;

    private Vector2 heading = Vector2.zero;
    private Vector2 direction = Vector2.right;

    private Vector2 enemyPlatformDown = Vector2.down;

    [Header("GameObjects")]
    [SerializeField] private GameObject gameObjectMoney = null;
    [SerializeField] private bool dropMoney = true;  //// Eliminar a futuro
    private GameObject bestiarioCount = null;
    private GameObject player = null;

    [Header("Layers")]
    [SerializeField] private LayerMask ground = 0;
    [SerializeField] private LayerMask playerMask = 0;
    [Space]

    //EnemyCollider
    private FlyingEnemy_Collider enemyCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponentInChildren<FlyingEnemy_Collider>();
        enemyAttackColRight = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        enemyAttackColLeft = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        bestiarioCount = GameObject.FindGameObjectWithTag("BestiarioCount");
        speed = runSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        IsFacingRight();
        GetDamage(0);
    }

    private void FixedUpdate()
    {
        EnemyMovement();
        EnemyAttack();
    }

    ////Movement
    private void EnemyMovement()
    {
        //Controla la direccion del enemigo
        heading = player.transform.position - this.gameObject.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        //Colision con el suelo
        RaycastHit2D hitPlatformDown = Physics2D.Raycast(this.gameObject.transform.position, enemyPlatformDown, distanceFromGround, ground);

        //Movimientos del enemigo
        if (goBack && !hurtAnimation)
        {
            speed = runSpeed;
            if (hitPlatformDown)
            {
                m_rigidbody2D.velocity = (new Vector2(direction.x, -1.0f) * speed * -1);
            }
            else
            {
                m_rigidbody2D.velocity = (new Vector2(direction.x, 0.0f) * speed * -1);
            }

            if(goBackTime >= goBackCoolDown)
            {
                goBackTime = 0.0f;
                goBack = false;
            }
            else
            {
                goBackTime += Time.deltaTime;
            }

        }
        else if (enemyCollider.playerDetected && !enemyAttackCheck && !hurtAnimation)
        {
            speed = runSpeed;
            m_rigidbody2D.velocity = (direction * speed);
            ChangeAttackDirection(direction);
        }
        else if (!enemyAttackCheck && !hurtAnimation)
        {
            if (hitPlatformDown)
            {
                m_rigidbody2D.velocity = (Vector2.up * speed);
            }
            else
            {
                m_rigidbody2D.velocity = new Vector2(speed * flyDirection, 0);
                RandomDirection();
                ChangeAttackDirection();
            }
        }
    }

    private void StopMovement()
    {
        m_rigidbody2D.velocity = (direction * 0.0f);
    }

    private void IsFacingRight()
    {
        if (flyDirection == 1)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    private void RandomDirection()
    {
        flyTime += Time.deltaTime;
        if (actualFly)
        {
            flyType = Random.Range(1, 6);
            flyTimeCoolDown = flyTime;
            flyDirectionRand = Random.Range(0, 2);
            actualFly = false;
            Debug.Log("Random");
        }
        switch (flyType)
        {
            case 1:
                if (flyTime - flyTimeCoolDown >= 1.0f)
                {
                    speed = 0;
                    flyType = 0;
                    flyTimeCoolDown = flyTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 2:
                if (flyTime - flyTimeCoolDown >= 2.0f)
                {
                    speed = 0;
                    flyType = 0;
                    flyTimeCoolDown = flyTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 3:
                if (flyTime - flyTimeCoolDown >= 3.0f)
                {
                    speed = 0;
                    flyType = 0;
                    flyTimeCoolDown = flyTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 4:
                if (flyTime - flyTimeCoolDown >= 4.0f)
                {
                    speed = 0;
                    flyType = 0;
                    flyTimeCoolDown = flyTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            case 5:
                if (flyTime - flyTimeCoolDown >= 5.0f)
                {
                    speed = 0;
                    flyType = 0;
                    flyTimeCoolDown = flyTime;
                }
                else
                {
                    speed = runSpeed;
                }
                break;
            default:
                if (flyTime - flyTimeCoolDown >= 3.0f)
                {
                    actualFly = true;
                }
                break;
        }
        switch (flyDirectionRand)
        {
            case 0:
                flyDirection = 1;
                break;
            case 1:
                flyDirection = -1;
                break;
        }
    }

    ////Attack
    private void ChangeAttackDirection()
    {
        switch (flyDirection)
        {
            case 1:
                this.gameObject.transform.localScale = enemyAttackColRight;
                break;
            case -1:
                this.gameObject.transform.localScale = enemyAttackColLeft;
                break;
        }
    }

    private void ChangeAttackDirection(Vector2 currentDirection)
    {
        if(currentDirection.x > 0.0f)
        {
            this.gameObject.transform.localScale = enemyAttackColRight;
        }
        else
        {
            this.gameObject.transform.localScale = enemyAttackColLeft;
        }
    }

    private void EnemyAttack()
    {
        if (distance <= distanceToAttack || enemyAttackCheck)
        {
            if (timerAttack <= 0.0f && !hurtAnimation)  //Se activa la animacion de ataque
            {
                //Animator.SetTrigger("Attack");
                timerAttack += Time.deltaTime;
                enemyAttackCheck = true;
            }
            else if (timerAttack >= startAttack && timerAttack <= endAttack && !hurtAnimation)  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
            {
                Attack();
                timerAttack += Time.deltaTime;
                StopMovement();
                Debug.Log("Ataca");
            }
            else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
            {
                timerAttack = 0.0f;
                enemyAttackCheck = false;
                attackOneTime = false;
                goBack = true;
            }
            else
            {
                timerAttack += Time.deltaTime;
            }
        }
    }

    void Attack()
    {
        Collider2D[] objectsInEnemyAttack = Physics2D.OverlapCircleAll(attack_Point.position, attackRange, playerMask);
        foreach (Collider2D colliders in objectsInEnemyAttack)
        {
            if (colliders.gameObject.tag == "Player" && !attackOneTime)
            {
                if (colliders.GetComponent<Player_Attack>().defendState == true && colliders.GetComponent<Player_Movement>().IsFacingLeft() == isFacingRight)
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

    public void GetDamage(int playerDamage)
    {
        if (!hurtAnimation) enemyHealth -= playerDamage;

        if (enemyHealth <= 0)
        {
            Debug.Log("Enemigo muerto");
            int numRandom = Random.Range(1, 100);
            if (numRandom <= 50 && dropMoney)
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

    void OnDrawGizmosSelected() //Dibuja el area de ataque del enemigo luego hay que eliminarlo para los tests
    {
        Gizmos.DrawWireSphere(attack_Point.position, attackRange);
    }

}
