using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int playerEnergy = 100;
    [SerializeField] private float timeWithInvencibility = 4.0f;
    [SerializeField] private float timeWithInvencibilityStamina = 0.2f;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private int playerDamage = 20;
    [SerializeField] private float timeBetweenAttack;

    private float timerAttack = 0.0f;
    
    private bool attackBtn = false;
    private bool defendBtn = false;
    [HideInInspector] public bool defendState = false;
    private bool invencibility = false;
    private float invencibilityTime = 0.0f;
    private bool invencibilityStamina = false;
    private float invencibilityTimeStamina = 0.0f;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        defendBtn = Input.GetButton("Defend");

        if (Input.GetButtonDown("Attack"))
        {
            attackBtn = true;
        }

        if(defendBtn && playerEnergy > 0)
        {
            defendState = true;
        }
        else
        {
            defendState = false;
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerAttack <= 0.0f)
        {
            if (attackBtn)
            {
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

    void Attack()//para hacer daño
    {
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            switch (enemy.gameObject.tag)
            {
                case ("GreenSkeleton"):
                    enemy.GetComponent<Enemy_AI>().GetDamage(playerDamage);
                    Debug.Log(enemy.tag);
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
            }
            else if (invencibilityTime >= timeWithInvencibility)
            {
                invencibility = false;
                invencibilityTime = 0.0f;
            }
            else
            {
                invencibilityTime += Time.deltaTime;
            }
        }
        else if (enemyDamage > 0.0f && gameObject.GetComponent<Player_Movement>().state != Player_Movement.State.DODGEROLL)
        {
            invencibility = true;
            playerHealth -= enemyDamage;
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Player is Dead");
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

}
