using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class False_Boss_AI : MonoBehaviour
{

    [Header("Health")]
    [SerializeField]private int health = 100;
    private int maxHealth = 100;

    //Enemy ID
    //public int enemyType;

    //[Range(0f, 10f)]
    [Header("Movement")]
    [SerializeField] private float startSpeed = 4.0f;
    [SerializeField] private int direction = 1;
    [SerializeField] private float viewDistance = 46.0f;
    private float speed = 0.0f;
    private bool isFacingRight = true;
    //Variables tiempo de giro
    private float visionTime = 0.0f;  //Tiempo que tarda el enemigo en girarse para atras
    private float visionTimeCoolDown = 0.0f;
    private bool playerBehind = false;
    //Variables del Dash
    private bool dashMode = false;
    private float dashTime = 0.5f;
    private float dashTimeCoolDown = 0.0f;

    [Header("Attack")]
    [SerializeField] private int damageAttack = 10;
    [SerializeField] private Transform Attack_Point = null;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private float height = 1.7f;
    [SerializeField] private float startAttack = 0.36f;
    [SerializeField] private float endAttack = 0.64f;
    [SerializeField] private float startAttackContinue = 1.0f;
    [SerializeField] private float endAttackContinue = 1.86f;
    [SerializeField] private float endAttackAnimation = 2.07f;
    [SerializeField] private float startSecondAttack = 0.64f;
    [SerializeField] private float endSecondAttack = 1.57f;
    [SerializeField] private float endSecondAttackAnimation = 2.07f;
    private bool enemyAttackCheck1 = false;  //Para saber cuando el enemigo ataca y no se mueva o cambie de direccion
    private bool enemyAttackCheck2 = false;
    private int attackType = 0;  //Tipo de ataque que hará el enemigo
    private bool enemyAttackDecided = false;
    private bool enemyAttack2Col = false;
    private bool attackOneTime = false;
    //Variables tiempo de ataque
    //private float coolDownTime = 0.0f;
    private float timerAttack = 0.0f;

    [Header("GameObjects")]
    [SerializeField]private GameObject enemyDead = null;
    private GameObject player = null;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer = 0;
    [SerializeField] private LayerMask ground = 0;

    //Componentes
    private Animator m_animator = null;
    private Rigidbody2D m_rigidBody2D = null;

    private Quaternion quaternionDirection = new Quaternion();
    
    //Vector2
    private Vector2 enemyDirectionRight = Vector2.right;
    private Vector2 enemyDirectionLeft = Vector2.left;
    private Vector2 enemyPlatformRight = new Vector2(1, -1);
    private Vector2 enemyPlatformLeft = new Vector2(-1, -1);
    private Vector2 enemyPlatformDown = Vector2.down;
    private Vector2 enemyAttackColRight = new Vector2(1, 1);
    private Vector2 enemyAttackColLeft = new Vector2(-1, 1);
    private Vector2 enemyVision = Vector2.zero;

    //Boss_Start bossAnimation;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        quaternionDirection.Set(0, 180, 0, 1);

        //type = enemyType;

        //bossAnimation = FindObjectOfType<Boss_Start>();
    }

    private void Update()
    {
        ChangeAttackDirection(direction);
        AnimationMovement();
        IsFacingRight();
        EnemyTurnAround();
        GetDamage(0);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        enemyVision = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + height, this.gameObject.transform.position.z);

        //Codigo para evitar caerse por los bordes
        RaycastHit2D hitPlatformRight = Physics2D.Raycast(this.gameObject.transform.position, enemyPlatformRight, 2.0f, ground);
        RaycastHit2D hitPlatformLeft = Physics2D.Raycast(this.gameObject.transform.position, enemyPlatformLeft, 2.0f, ground);
        RaycastHit2D hitPlatformDown = Physics2D.Raycast(this.gameObject.transform.position, enemyPlatformDown, 2.0f, ground);
        if (hitPlatformDown && hitPlatformLeft.collider == false && hitPlatformDown.collider.gameObject.tag == "Ground" && direction < 0)
        {
            direction = 1;
        }
        else if (hitPlatformDown && hitPlatformRight.collider == false && hitPlatformDown.collider.gameObject.tag == "Ground" && direction > 0)
        {
            direction = -1;
        }

        //Codigo para poder ver al jugador y moverse
        RaycastHit2D hitRight = Physics2D.Raycast(enemyVision, enemyDirectionRight, viewDistance, LayerMask.GetMask("Player"));
        RaycastHit2D hitLeft = Physics2D.Raycast(enemyVision, enemyDirectionLeft, viewDistance, LayerMask.GetMask("Player"));
        if (hitRight && hitRight.collider.gameObject.tag == "Player" && direction > 0 && !enemyAttackCheck1)
        {
            speed = startSpeed;
        }
        else if (hitLeft && hitLeft.collider.gameObject.tag == "Player" && direction < 0 && !enemyAttackCheck1)
        {
            speed = startSpeed;
        }
        else
        {
            //Hacer patron aleatorio de movimiento recordatorio
            speed = 0f;
        }

        //Codigo para para cuando el jugador este muy cerca
        RaycastHit2D playerHitRight = Physics2D.Raycast(enemyVision, Vector2.right, 1.4f, LayerMask.GetMask("Player"));
        RaycastHit2D playerHitLeft = Physics2D.Raycast(enemyVision, Vector2.left, 1.4f, LayerMask.GetMask("Player"));

        if (playerHitRight && playerHitRight.collider.gameObject.tag == "Player" && direction > 0 && !enemyAttackCheck2)
        {
            speed = 0.0f;
        }
        else if (playerHitLeft && playerHitLeft.collider.gameObject.tag == "Player" && direction < 0 && !enemyAttackCheck2)
        {
            speed = 0.0f;
        }

        //Controla la direccion del enemigo
        m_rigidBody2D.velocity = new Vector2(direction * speed, 0);
        
        EnemyAttack();
        
    }

    void EnemyAttack()
    {
        RaycastHit2D playerHitRight = Physics2D.Raycast(enemyVision, Vector2.right, 1.4f, LayerMask.GetMask("Player"));
        RaycastHit2D playerHitLeft = Physics2D.Raycast(enemyVision, Vector2.left, 1.4f, LayerMask.GetMask("Player"));
        if (health > maxHealth / 2)
        {
            if ((playerHitRight && playerHitRight.collider.gameObject.tag == "Player" && direction > 0) || enemyAttackCheck1)
            {
                if (timerAttack <= 0.0f)  //Se activa la animacion de ataque
                {
                    timerAttack += Time.deltaTime;
                    m_animator.SetTrigger("Attack");
                    enemyAttackCheck1 = true;
                }
                else if ((timerAttack >= startAttack && timerAttack <= endAttack) || (timerAttack >= startAttackContinue && timerAttack <= endAttackContinue))  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                {
                    attack();
                    timerAttack += Time.deltaTime;
                }
                else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                {
                    timerAttack = 0.0f;
                    enemyAttackCheck1 = false;
                    attackOneTime = false;
                }
                else
                {
                    timerAttack += Time.deltaTime;
                }
            }
            else if ((playerHitLeft && playerHitLeft.collider.gameObject.tag == "Player" && direction < 0) || enemyAttackCheck1)
            {
                if (timerAttack <= 0.0f)  //Se activa la animacion de ataque
                {
                    timerAttack += Time.deltaTime;
                    m_animator.SetTrigger("Attack");
                    enemyAttackCheck1 = true;
                }
                else if ((timerAttack >= startAttack && timerAttack <= endAttack) || (timerAttack >= startAttackContinue && timerAttack <= endAttackContinue))  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                {
                    attack();
                    Debug.Log("Ataca");
                    timerAttack += Time.deltaTime;
                }
                else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                {
                    timerAttack = 0.0f;
                    enemyAttackCheck1 = false;
                    attackOneTime = false;
                }
                else
                {
                    timerAttack += Time.deltaTime;
                }
            }
            else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
            {
                timerAttack = 0.0f;
                enemyAttackCheck1 = false;
            }
        }
        else if (health <= maxHealth / 2)
        {
            if (!enemyAttackDecided)
            {
                attackType = Random.Range(1, 11);
                enemyAttackDecided = true;
            }
            if ((playerHitRight && playerHitRight.collider.gameObject.tag == "Player" && direction > 0) || enemyAttackCheck1)
            {
                if (attackType <= 6)
                {
                    if (timerAttack <= 0.0f)  //Se activa la animacion de ataque
                    {
                        timerAttack += Time.deltaTime;
                        m_animator.SetTrigger("Attack");
                        enemyAttackCheck1 = true;
                    }
                    else if ((timerAttack >= startAttack && timerAttack <= endAttack) || (timerAttack >= startAttackContinue && timerAttack <= endAttackContinue))  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                    {
                        attack();
                        timerAttack += Time.deltaTime;
                    }
                    else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                    {
                        timerAttack = 0.0f;
                        enemyAttackCheck1 = false;
                        enemyAttackDecided = false;
                        attackOneTime = false;
                    }
                    else
                    {
                        timerAttack += Time.deltaTime;
                    }
                }
                else
                {
                    if (timerAttack < 1)  //Se activa la animacion de ataque
                    {
                        timerAttack += Time.deltaTime;
                        m_animator.SetTrigger("Spin-Attack");
                        enemyAttackCheck2 = true;
                    }
                    else if (timerAttack >= startSecondAttack && timerAttack <= endSecondAttack)  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                    {
                        attack();
                        timerAttack += Time.deltaTime;
                        Debug.Log("Ataca");
                    }
                    else if (timerAttack >= endSecondAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                    {
                        timerAttack = 0.0f;
                        enemyAttackCheck2 = false;
                        enemyAttackDecided = false;
                        attackOneTime = false;
                    }
                    else
                    {
                        timerAttack += Time.deltaTime;
                    }
                }

            }
            else if ((playerHitLeft && playerHitLeft.collider.gameObject.tag == "Player" && direction < 0) || enemyAttackCheck1)
            {
                if (attackType <= 6)
                {
                    if (timerAttack <= 0.0f)  //Se activa la animacion de ataque
                    {
                        timerAttack += Time.deltaTime;
                        m_animator.SetTrigger("Attack");
                        enemyAttackCheck1 = true;
                    }
                    else if ((timerAttack >= startAttack && timerAttack <= endAttack) || (timerAttack >= startAttackContinue && timerAttack <= endAttackContinue))  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                    {
                        attack();
                        timerAttack += Time.deltaTime;
                    }
                    else if (timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                    {
                        timerAttack = 0.0f;
                        enemyAttackCheck1 = false;
                        enemyAttackDecided = false;
                        attackOneTime = false;
                    }
                    else
                    {
                        timerAttack += Time.deltaTime;
                    }
                }
                else
                {
                    if (timerAttack <= 0.0f)  //Se activa la animacion de ataque
                    {
                        timerAttack += Time.deltaTime;
                        m_animator.SetTrigger("Spin-Attack");
                        enemyAttackCheck2 = true;
                    }
                    else if (timerAttack >= startSecondAttack && timerAttack <= endSecondAttack)  //Enemigo esta en el momento en que hace el corte y es ahi cuando el Player puede recivir daño
                    {
                        attack();
                        timerAttack += Time.deltaTime;
                    }
                    else if (timerAttack >= endSecondAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
                    {
                        timerAttack = 0.0f;
                        enemyAttackCheck2 = false;
                        enemyAttackDecided = false;
                        attackOneTime = false;
                    }
                    else
                    {
                        timerAttack += Time.deltaTime;
                    }
                }
            }
            else if (timerAttack >= endSecondAttackAnimation || timerAttack >= endAttackAnimation)  //Se termina de realizar todo el ataque PD: el numero puede variar
            {
                timerAttack = 0.0f;
                enemyAttackCheck2 = false;
                enemyAttackDecided = false;
            }
        }
    }

    //Detecta al jugador si esta dentro del collider trigger
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            enemyAttack2Col = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            enemyAttack2Col = false;
        }
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

    //Comprueba que el Player este en el area de ataque y ejecuta una funcion para que reciba daño
    void attack()
    {
        playerLayer = LayerMask.GetMask("Player");
        Collider2D[] objectsInEnemyAttack = Physics2D.OverlapCircleAll(Attack_Point.position, attackRange, playerLayer);
        foreach (Collider2D colliders in objectsInEnemyAttack)
        {
            //Funcion de recibir daño del jugador
            //Tambien se puede añadir funcion para rom,per objetos del escenario
            if (colliders.gameObject.tag == "Player" && !attackOneTime)
            {
                if (colliders.GetComponent<Player_Attack>().defendState == true && colliders.GetComponent<Player_Movement>().IsFacingLeft() == isFacingRight)
                {
                    colliders.GetComponent<Player_Attack>().GetStamina(damageAttack);
                    attackOneTime = true;
                }
                else
                {
                    colliders.GetComponent<Player_Attack>().GetDamage(damageAttack * 2);
                }
            }

        }
        if (enemyAttack2Col && GetComponent<Player_Attack>() != null)
        {
            //Player recibe daño
            player.GetComponent<Player_Attack>().GetDamage(damageAttack);
            enemyAttack2Col = false;

        }

    }

    //Recibe daño del jugador (Player)
    public void GetDamage(int playerDamage)
    {
        health -= playerDamage;

        if (health <= 0)
        {
            if (direction < 0)
            {
                Instantiate(enemyDead, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), quaternionDirection);
            }
            else if (direction > 0)
            {
                Instantiate(enemyDead, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
            }

            //Añadir Boss al bestiario
            Data_Control.instance.SetFalseBossCount();
            ///////////////////////////
            ///////////////////////////

            Destroy(this.gameObject);
            //Destruccion del enemigo
        }

    }

    void AnimationMovement()
    {
        if (dashMode)
        {
            if (dashTimeCoolDown <= 0.0f)
            {
                m_animator.SetTrigger("Dash");
                dashTimeCoolDown += Time.deltaTime;
            }
            else if (dashTimeCoolDown >= dashTime)
            {
                dashMode = false;
                dashTimeCoolDown = 0.0f;
            }
            else
            {
                dashTimeCoolDown += Time.deltaTime;
            }

        }
        
        m_animator.SetFloat("Speed", Mathf.Abs(m_rigidBody2D.velocity.x));
        
    }

    void EnemyTurnAround()
    {
        visionTime += Time.deltaTime;
        if (transform.position.x < player.transform.position.x && direction < 0 && !enemyAttackCheck1 && !enemyAttackCheck2)
        {
            if (!playerBehind)
            {
                visionTimeCoolDown = visionTime;
                playerBehind = true;
            }
            else if (visionTime - visionTimeCoolDown >= 5.0f)
            {
                direction = 1;
                speed = startSpeed;
                playerBehind = false;
            }
        }
        else if (transform.position.x > player.transform.position.x && direction > 0 && !enemyAttackCheck1 && !enemyAttackCheck2)
        {
            if (!playerBehind)
            {
                visionTimeCoolDown = visionTime;
                playerBehind = true;
            }
            else if (visionTime - visionTimeCoolDown >= 5.0f)
            {
                direction = -1;
                speed = startSpeed;
                playerBehind = false;
            }
        }
        else if (visionTime - visionTimeCoolDown >= 5.0f)
        {
            playerBehind = false;
        }
    }

    private void IsFacingRight()
    {
        if (direction == 1)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    void OnDrawGizmosSelected() //Dibuja el area de ataque del enemigo luego hay que eliminarlo para los tests
    {
        Gizmos.DrawWireSphere(Attack_Point.position, attackRange);
    }
}
