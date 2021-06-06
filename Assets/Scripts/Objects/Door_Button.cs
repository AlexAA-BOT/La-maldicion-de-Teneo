﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door_Button : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshPro text = null;

    [Header("Door ID")]
    [SerializeField] private int ID = 0;
    private GameObject player = null;
    private bool onButton = false;
    private bool interact = false;

    //Audio
    private AudioSource m_AudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        InputInteract();
        if(onButton && interact)
        {
            if(Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.CLOSE)
            {
                m_AudioSource.Play();
                Data_Control.instance.SetDoorState_Z1(ID, Data_Control.DoorState.OPEN_FIRST_TIME);
                text.enabled = false;
            }
            interact = false;
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
            if(Data_Control.instance.GetDoorState_Z1(ID) == Data_Control.DoorState.CLOSE)
            {
                onButton = true;
                text.enabled = true;
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onButton = false;
            text.enabled = false;
        }
    }

}
