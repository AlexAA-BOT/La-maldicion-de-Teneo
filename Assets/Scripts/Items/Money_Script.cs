using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_Script : MonoBehaviour
{
    private GameObject player = null;
    [SerializeField] private Transform parent = null;

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
            player.GetComponent<Player_Inventory>().AddItem(Player_Inventory.Items.MONEY);
            Destroy(parent.gameObject);
        }
    }

}
