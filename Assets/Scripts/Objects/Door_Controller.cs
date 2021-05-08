using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door_Controller : MonoBehaviour
{
    [HideInInspector] public enum DoorID { NONE, SHOP_1 };
    public DoorID doorID = DoorID.NONE;

    [Header("Door ID")]
    [SerializeField] private int ID = 0;

    [Header("Key ID")]
    [SerializeField] private int k_ID = 0;

    [Header("Text")]
    [SerializeField] private TextMeshPro text = null;

    [Header("Movement")]
    [SerializeField] private float speed = 2.0f;
    [Space]
    [SerializeField] private bool needAKey = false;
    private bool interact = false;
    private bool openDoor = false;
    private bool onDoor = false;
    private Rigidbody2D m_rigidbody2D = null;
    private GameObject player = null;
    private float yPos = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        yPos = this.gameObject.transform.position.y;
    }

    private void Update()
    {
        InputInteract();
        if(needAKey && onDoor && interact && player.GetComponent<Player_Inventory>().GetKeyState(k_ID))
        {
            if (Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.CLOSE)
            {
                Data_Control.instance.SetDoorState_Z1(ID, Data_Control.DoorState.OPEN_FIRST_TIME);
            }
            interact = false;
        }
        else
        {
            interact = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GetDoorState();
    }

    private void InputInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            interact = true;
        }
    }

    private void GetDoorState()
    {
        if(Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.OPEN_FIRST_TIME || openDoor) // Cambiar el 0 por ID
        {
            m_rigidbody2D.velocity = new Vector2(0, speed);
            Data_Control.instance.SetDoorState_Z1(ID, Data_Control.DoorState.OPEN);
            openDoor = true;
            if (this.gameObject.transform.position.y >= yPos + 4.0f)
            {
                openDoor = false;
                m_rigidbody2D.velocity = new Vector2(0, 0);
            }
        }
        else if(Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.OPEN && !openDoor && this.gameObject.transform.position.y < yPos + 4.0f)
        {
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 4.0f);
        }
    }

    public void SetDoorOpen()
    {
        openDoor = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onDoor = true;
            if(needAKey && Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.CLOSE)
            {
                text.enabled = true;
                if(!player.GetComponent<Player_Inventory>().GetKeyState(k_ID))
                    text.text = "You need a key";
                else
                    text.text = "Press E to open the door";
            }
            else if(needAKey && Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.OPEN)
            {
                text.enabled = false;
            }
                
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onDoor = false;
            text.enabled = false;
        }
    }

}
