using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Control : MonoBehaviour
{
    [HideInInspector] public static Data_Control instance = null;

    [HideInInspector] public enum DoorState { CLOSE, OPEN_FIRST_TIME, OPEN };

    ////Player Data
    private Vector3 playerPos = new Vector3(-23.0f, 4.8f, 0.0f);

    ////Door Data zone 1
    public DoorState[] doorsStates_z1 = { DoorState.CLOSE, DoorState.CLOSE, DoorState.CLOSE };

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

    public DoorState GetDoorState_Z1(int ID)
    {
        return doorsStates_z1[ID];
    }

    public void SetDoorState_Z1(int ID, DoorState newState)
    {
        doorsStates_z1[ID] = newState;
    }

    public Vector3 GetPlayerPos() { return playerPos; }

}
