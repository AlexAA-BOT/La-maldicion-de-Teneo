using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop_System : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject noMoneyWind = null;
    [SerializeField] private GameObject noSpaceWind = null;
    [SerializeField] private GameObject player = null;
    //private GameObject[] items = null;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI money = null;


    // Start is called before the first frame update
    void Start()
    {
        noMoneyWind.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        //items = GameObject.FindGameObjectsWithTag("Price");
        money.text = "Money: " + player.GetComponent<Player_Inventory>().GetMoney();
    }

    // Update is called once per frame
    void Update()
    {
        money.text = "Money: " + player.GetComponent<Player_Inventory>().GetMoney();
    }

    public void OpenNoSpaceWind()
    {
        noSpaceWind.SetActive(true);
    }

    public void CloseNoSpaceWind()
    {
        noSpaceWind.SetActive(false);
    }

    public void OpenNoMoneyWind()
    {
        noMoneyWind.SetActive(true);
    }

    public void CloseNoMoneyWind()
    {
        noMoneyWind.SetActive(false);
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

}
