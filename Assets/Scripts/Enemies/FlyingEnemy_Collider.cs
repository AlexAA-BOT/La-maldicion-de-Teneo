using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy_Collider : MonoBehaviour
{
    public bool playerDetected = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerDetected = true;
        }
        //else
        //{
        //    playerDetected = false;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = false;
        }
    }
}
