using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy_DeathFall : MonoBehaviour
{

    private Animator m_animator = null;
    private Vector2 groundCol =  Vector2.zero;
    [SerializeField] private LayerMask platform = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        groundCol = new Vector2(0.0f, -1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hitPlatformDown = Physics2D.Raycast(this.gameObject.transform.position, groundCol, 0.5f, platform);
        if (hitPlatformDown && hitPlatformDown.collider.gameObject.tag == "Ground")
        {
            m_animator.SetTrigger("HitFloor");
        }
    }
}
