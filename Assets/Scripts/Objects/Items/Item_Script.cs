using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    private GameObject player = null;
    [SerializeField] private Transform parent = null;
    [SerializeField] private Player_Inventory.Items itemID = Player_Inventory.Items.MONEY;

    [Header("Coins ID")]
    [SerializeField] private int zone = 1;
    [SerializeField] private int room = 1;
    [SerializeField] private int coinID = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player_Inventory>().AddItem(itemID);
            Data_Control.instance.SetCoinState(zone, room, coinID, true);
            Destroy(parent.gameObject);
        }
    }

}
