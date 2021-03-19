using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_Script : MonoBehaviour
{
    private GameObject player = null;
    private Transform parent = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<Player_Inventory>().AddItem(Player_Inventory.Items.MONEY);
            Destroy(parent.gameObject);
        }
    }

}
