using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_Script : MonoBehaviour
{
    [Header("Key ID")]
    [SerializeField] private int k_ID = 0;
    private bool onKey = false;
    private bool interact = false;

    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<Player_Inventory>().GetKeyState(k_ID))
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        InputInteract();
        if (onKey && interact)
        {
            player.GetComponent<Player_Inventory>().SetKeyState(k_ID, true);
            Destroy(this.gameObject);
        }
        else
        {
            interact = false;
        }
    }

    private void InputInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            interact = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onKey = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onKey = false;
        }
    }
}
