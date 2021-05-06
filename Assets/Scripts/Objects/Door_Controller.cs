using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2.0f;

    private bool openDoor = false;
    private Rigidbody2D m_rigidbody2D = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(openDoor)
        {
            m_rigidbody2D.velocity = new Vector2(0, speed);
        }
    }

    public void SetDoorOpen()
    {
        openDoor = true;
    }

}
