using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float playerHealth = 100;
    [SerializeField] private float playerEnergy = 100;
    [SerializeField] private float timeWithInvencibility = 4.0f;
    [SerializeField] private float timeWithInvencibilityStamina = 0.2f;
    [SerializeField] private float timeStaminaRecovery = 0.5f;
    [SerializeField] private float recoveryEnergy = 15.0f;
    [SerializeField] private float invencibilityTransparency = 0.5f;
    //private bool reloadEnergy = false;  //// Mirar si se usa

    [Header("Attack")]
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private int playerDamage = 20;
    [SerializeField] private float timeBetweenAttack = 0.0f;

    private float timerAttack = 0.0f;

    private bool playerIsDead = false;
    private bool attackBtn = false;
    private bool defendBtn = false;
    [HideInInspector] public bool defendState = false;
    private bool invencibility = false;
    private float invencibilityTime = 0.0f;
    private bool invencibilityStamina = false;
    private float invencibilityTimeStamina = 0.0f;
    private float timerStaminaReload = 0.0f;
    private SpriteRenderer mySprite = null;
    private Animator m_Animator = null;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!playerIsDead)
            Inputs();

        if(defendBtn && playerEnergy > 0)
        {
            defendState = true;
        }
        else
        {
            defendState = false;
        }

        m_Animator.SetBool("Defend", defendState);

        ReloadStamina();
        GetStamina(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerAttack <= 0.0f)
        {
            if (attackBtn)
            {
                m_Animator.SetTrigger("Attack");
                Attack();
                timerAttack += Time.deltaTime;
            }
        }
        else if(timerAttack >= timeBetweenAttack)
        {
            timerAttack = 0.0f;
        }
        else
        {
            timerAttack += Time.deltaTime;
        }

        GetDamage(0);

    }

    private void Inputs()
    {
        defendBtn = Input.GetButton("Defend");

        if(Input.GetButtonDown("Defend"))
        {
            m_Animator.SetTrigger("RiseShield");
        }

        if (Input.GetButtonDown("Attack"))
        {
            attackBtn = true;
        }
    }

    private void Attack()//para hacer daño
    {
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            switch (enemy.gameObject.tag)
            {
                case ("Enemy"):
                    enemy.GetComponent<Enemy_AI>().GetDamage(playerDamage);
                    break;
                case ("FlyingEnemy"):
                    enemy.GetComponent<FlyingEnemy_AI>().GetDamage(playerDamage);
                    break;

            }
        }

        attackBtn = false;

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        //Gizmos.DrawWireSphere(this.gameObject.transform.position, 0.1f);
    }

    public void GetDamage(int enemyDamage)
    {
        if (invencibility)
        {
            if (invencibilityTime <= 0.0f)
            {
                invencibilityTime += Time.deltaTime;
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, invencibilityTransparency);
            }
            else if (invencibilityTime >= timeWithInvencibility)
            {
                invencibility = false;
                invencibilityTime = 0.0f;
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1.0f);
            }
            else
            {
                invencibilityTime += Time.deltaTime;
            }
        }
        else if (playerHealth > 0.0f && enemyDamage > 0.0f && gameObject.GetComponent<Player_Movement>().state != Player_Movement.State.DODGEROLL)
        {
            invencibility = true;
            playerHealth -= enemyDamage;
        }

        if (playerHealth <= 0 && !playerIsDead)
        {
            invencibility = false;
            invencibilityTime = 0.0f;
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1.0f);
            playerIsDead = true;
            m_Animator.SetTrigger("Dead");
            gameObject.tag = "Untagged";
        }
    }

    public void GetStamina(int enemyDamage)
    {
        if (invencibilityStamina)
        {
            if (invencibilityTimeStamina <= 0.0f)
            {
                invencibilityTimeStamina += Time.deltaTime;
            }
            else if (invencibilityTimeStamina >= timeWithInvencibilityStamina)
            {
                invencibilityStamina = false;
                invencibilityTimeStamina = 0.0f;
            }
            else
            {
                invencibilityTimeStamina += Time.deltaTime;
            }
        }
        else if (enemyDamage > 0.0f && gameObject.GetComponent<Player_Movement>().state != Player_Movement.State.DODGEROLL)
        {
            invencibilityStamina = true;
            playerEnergy -= enemyDamage;
        }

    }

    private void ReloadStamina()
    {
        if(playerEnergy < 100.0f)
        {
            if(timerStaminaReload >= timeStaminaRecovery)
            {
                if(!defendBtn)
                {
                    playerEnergy += recoveryEnergy * Time.deltaTime;
                }
            }
            else
            {
                timerStaminaReload += Time.deltaTime;
            }
        }
        else
        {
            timerStaminaReload = 0.0f;
            playerEnergy = 100.0f;
        }
    }

    public float GetHealth() { return playerHealth; }

    public float GetStamina() { return playerEnergy; }

    public bool GetDefendState() { return defendBtn; }

    public bool GetPlayerLiveState() { return playerIsDead; }

    public void AddHealth(float _health) { playerHealth += _health; if (playerHealth > 100.0f) playerHealth = 100.0f; }
    public void AddStamina(float _stamina) { playerEnergy += _stamina; if (playerEnergy > 100.0f) playerEnergy = 100.0f; }

}
