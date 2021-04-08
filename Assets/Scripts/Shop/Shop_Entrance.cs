using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Entrance : MonoBehaviour
{
    [HideInInspector] public bool canEnterShop = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canEnterShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canEnterShop = false;
        }
    }

}
