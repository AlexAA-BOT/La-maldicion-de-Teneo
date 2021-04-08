using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{

    [HideInInspector] public enum Items { HEALTHPOTION, STAMINAPOTION, MONEY };
    [HideInInspector] public Items item;

    [SerializeField] private int money = 0;
    [SerializeField] private int numHealthPotions = 0;
    [SerializeField] private int numStaminaPotions = 0;
    [SerializeField] private int maxItems = 4;
    [Space]
    [SerializeField] private float healthPotionRecover = 25.0f;
    [SerializeField] private float staminaPotionRecover = 30.0f;
    [Space]
    [SerializeField] private GameObject bestiario = null;
    private bool useHealthPotion = false;
    private bool useStaminaPotion = false;

    private bool openBestiario = false;
    private bool isBestiarioOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        TakePotion();
        BestiarioState();
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
            case (Items.STAMINAPOTION):
                numStaminaPotions++;
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
            case (Items.STAMINAPOTION):
                return (maxItems > numStaminaPotions);
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
            case (Items.STAMINAPOTION):
                return (numStaminaPotions);
            default:
                return 0;
        }
    }

    public int GetItemsMaxQuantity() { return maxItems; }

    private void Inputs()
    {
        if (Input.GetButtonDown("Health-Pot"))
        {
            useHealthPotion = true;
        }
        else if(Input.GetButtonDown("Stamina-Pot"))
        {
            useStaminaPotion = true;
        }

        if(Input.GetButtonDown("Bestiario"))
        {
            openBestiario = true;
        }

    }

    private void TakePotion()
    {
        if(useHealthPotion && numHealthPotions > 0)
        {
            gameObject.GetComponent<Player_Attack>().AddHealth(healthPotionRecover);
            numHealthPotions--;
            useHealthPotion = false;
        }
        else if(useStaminaPotion && numStaminaPotions > 0)
        {
            gameObject.GetComponent<Player_Attack>().AddStamina(staminaPotionRecover);
            numStaminaPotions--;
            useStaminaPotion = false;
        }
    }

    private void BestiarioState()
    {
        if(openBestiario)
        {
            if (isBestiarioOpen)
            {
                bestiario.SetActive(false);
                isBestiarioOpen = false;
                openBestiario = false;
            }
            else
            {
                bestiario.SetActive(true);
                isBestiarioOpen = true;
                openBestiario = false;
            }
        }
    }

}
