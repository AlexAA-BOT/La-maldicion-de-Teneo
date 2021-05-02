using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    private GameObject player = null;
    [SerializeField] private Transform parent = null;
    [SerializeField] private Player_Inventory.Items itemID = Player_Inventory.Items.MONEY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player_Inventory>().AddItem(itemID);
            Destroy(parent.gameObject);
        }
    }

}
