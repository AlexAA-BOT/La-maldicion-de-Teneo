using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private float playerDamage = 20.0f;

    private float timerAttack = 0.0f;
    [SerializeField] private float timeBetweenAttack;

    private bool attackBtn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            attackBtn = true;
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
    }

    void Attack()//para hacer daño
    {
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.tag);
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


}
