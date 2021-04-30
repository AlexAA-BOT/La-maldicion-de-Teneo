using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Exit : MonoBehaviour
{
    [HideInInspector] public bool exit = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            exit = true;
        }
    }

}
