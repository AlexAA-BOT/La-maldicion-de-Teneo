using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Entrance : MonoBehaviour
{
    private GameObject player = null;
    //[HideInInspector] public bool canEnterShop = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player_Movement>().SetCanEnterShop(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player_Movement>().SetCanEnterShop(false);
        }
    }

}
