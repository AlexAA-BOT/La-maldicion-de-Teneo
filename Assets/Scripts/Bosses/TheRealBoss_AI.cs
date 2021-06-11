using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRealBoss_AI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float runSpeed = 1.5f;
    private Rigidbody2D m_rigidbody2D = null;
    private bool isFacingRight = false;

    //Animator
    private Animator m_animator = null;

    //Floats
    private float distance = 0.0f;

    //Vector2
    private Vector2 heading = Vector2.zero;
    private Vector2 direction = Vector2.zero;

    [Header("Attack")]
    [SerializeField] private Transform attack_Point = null;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private int damageAttack = 1;
    private bool attackOneTime = false;

    [Header("GameObjects")]
    [SerializeField] private GameObject enemyDead = null;
    private GameObject bestiarioCount = null;
    private GameObject player = null;

    [Header("Layers")]
    [SerializeField] private LayerMask playerMask = 0;
    [Space]

    //Audio
    private AudioSource m_audioSource = null;
    [SerializeField] private AudioClip wingFlap = null;
    [SerializeField] private AudioClip attackSound = null;
    [SerializeField] private AudioClip attackSoundShield = null;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        m_audioSource = GetComponent<AudioSource>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        IsFacingRight(direction);
        Attack();
    }

    private void EnemyMovement()
    {
        //Controla la direccion del enemigo
        heading = player.transform.position - this.gameObject.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        m_rigidbody2D.velocity = (direction * runSpeed);
        
    }

    private void IsFacingRight(Vector2 currentDirection)
    {
        if (currentDirection.x > 0.0f)
        {
            isFacingRight = true;
            if(this.transform.localScale.x > 0.0f)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
        else
        {
            isFacingRight = false;
            if (this.transform.localScale.x < 0.0f)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1.0f, this.transform.localScale.y, this.transform.localScale.z);
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
                    m_audioSource.PlayOneShot(attackSoundShield);
                    colliders.GetComponent<Player_Attack>().GetStamina(damageAttack);
                    attackOneTime = true;
                }
                else
                {
                    m_audioSource.PlayOneShot(attackSound);
                    attackOneTime = true;
                    colliders.GetComponent<Player_Attack>().GetDamage(damageAttack);
                }
            }
        }
    }

    public void GetDamage(int playerDamage)
    {
        //Evento de disminuir la barra de vida
        GameObject.Find("BossController").GetComponent<TheRealBoss_Event>().RestBossHealthBar();

        //Respawnear prefab de enemigo muerto
        if (direction.x < 0)
        {
            Instantiate(enemyDead, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 1.0f, 0.0f)));
        }
        else
        {
            Instantiate(enemyDead, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public void WingFlap()
    {
        m_audioSource.PlayOneShot(wingFlap);
    }

    void OnDrawGizmosSelected()
    {
        if (attack_Point == null)
            return;
        Gizmos.DrawWireSphere(attack_Point.position, attackRange);
    }

}
