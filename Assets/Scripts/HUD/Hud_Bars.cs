using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_Bars : MonoBehaviour
{
    [HideInInspector] public enum BarsTypes { HEALTH, STAMINA };
    public BarsTypes bar = BarsTypes.HEALTH;

    [SerializeField] Slider slider = null;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        switch(bar)
        {
            case BarsTypes.HEALTH:
                slider.value = player.GetComponent<Player_Attack>().GetHealth() / 100;
                break;
            case BarsTypes.STAMINA:
                slider.value = player.GetComponent<Player_Attack>().GetStamina() / 100;
                break;

            default:
                break;
        }

        
    }
}
