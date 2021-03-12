using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_System : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject noMoneyWin = null;
    [SerializeField] private GameObject player = null;
    private GameObject[] items = null;

    [Header("Text")]
    [SerializeField] private TextMesh money = null;


    // Start is called before the first frame update
    void Start()
    {
        noMoneyWin.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        items = GameObject.FindGameObjectsWithTag("Item_Shop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseNoMoneyWind()
    {
        noMoneyWin.SetActive(false);
    }

}
