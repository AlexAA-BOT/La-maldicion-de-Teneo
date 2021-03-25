using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud_QuantityPotions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI item = null;
    [SerializeField] Player_Inventory.Items itemID = Player_Inventory.Items.HEALTHPOTION;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        item.text = player.GetComponent<Player_Inventory>().GetItemQuantity(itemID).ToString() + '/' + player.GetComponent<Player_Inventory>().GetItemsMaxQuantity();
    }

    // Update is called once per frame
    void Update()
    {
        item.text = player.GetComponent<Player_Inventory>().GetItemQuantity(itemID).ToString() + '/' + player.GetComponent<Player_Inventory>().GetItemsMaxQuantity();
    }
}
