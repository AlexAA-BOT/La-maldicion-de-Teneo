using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud_Money : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI money = null;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        money.text = "Money: " + player.GetComponent<Player_Inventory>().GetMoney();
    }

    // Update is called once per frame
    void Update()
    {
        money.text = player.GetComponent<Player_Inventory>().GetMoney().ToString();
    }
}
