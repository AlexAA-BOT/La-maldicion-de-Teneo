using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{

    [HideInInspector] public enum Items { HEALTPOTION, MONEY };
    [HideInInspector] public Items item;

    [SerializeField] private int money = 0;
    [SerializeField] private int numHealthPotions = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Items _item)
    {
        switch(_item)
        {
            case (Items.MONEY):
                money++;
                break;

            case (Items.HEALTPOTION):
                numHealthPotions++;
                break;
        }
    }

}
