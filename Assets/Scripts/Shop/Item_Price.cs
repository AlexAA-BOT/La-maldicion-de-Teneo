using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item_Price : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject shop = null;
    [Space]
    [SerializeField] private int price = 0;
    [SerializeField] Player_Inventory.Items itemID = Player_Inventory.Items.HEALTHPOTION;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI quantity = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shop = GameObject.Find("Shop");
        quantity.text = "0/4";
    }

    // Update is called once per frame
    void Update()
    {
        quantity.text = player.GetComponent<Player_Inventory>().GetItemQuantity(itemID) + "/4";
    }

    public void BuyItem()
    {
        if(player.GetComponent<Player_Inventory>().GetMoney() >= price && player.GetComponent<Player_Inventory>().IsMaxQuantity(itemID))
        {
            player.GetComponent<Player_Inventory>().AddItem(itemID);
            player.GetComponent<Player_Inventory>().RestMoney(price);
        }
        else if(!player.GetComponent<Player_Inventory>().IsMaxQuantity(itemID))
        {
            shop.GetComponent<Shop_System>().OpenNoSpaceWind();
        }
        else
        {
            shop.GetComponent<Shop_System>().OpenNoMoneyWind();
        }
    }

}
