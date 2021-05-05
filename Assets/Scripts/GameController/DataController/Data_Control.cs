using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Control : MonoBehaviour
{
    [HideInInspector] public static Data_Control instance = null;

    private Vector3 playerPos = new Vector3(-23.0f, 4.8f, 0.0f);

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if (instance != this)
    //        Destroy(gameObject);

    //    DontDestroyOnLoad(gameObject);
    //}
    //public static NomScript instance;
    //NomScript.instace.Funcio();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void SavePlayerPos(Vector3 newPos)
    {
        playerPos = newPos;
    }

    public Vector3 GetPlayerPos() { return playerPos; }

}
