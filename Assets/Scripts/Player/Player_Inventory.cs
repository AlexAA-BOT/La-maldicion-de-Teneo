using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{

    [HideInInspector] public enum Items { HEALTHPOTION, MONEY };
    [HideInInspector] public Items item;

    [SerializeField] private int money = 0;
    [SerializeField] private int numHealthPotions = 0;
    [SerializeField] private int maxItems = 4;

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

            case (Items.HEALTHPOTION):
                numHealthPotions++;
                break;
        }
    }

    public int GetMoney() { return money; }
    public void RestMoney(int moneyToRest)
    {
        money -= moneyToRest;
    }

    public bool IsMaxQuantity(Items _item)
    {
        switch (_item)
        {
            case (Items.HEALTHPOTION):
                return (maxItems > numHealthPotions);
            default:
                return false;
        }
    }

    public int GetItemQuantity(Items _item)
    {
        switch (_item)
        {
            case (Items.HEALTHPOTION):
                return (numHealthPotions);
            default:
                return 0;
        }
    }

}
