using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Control : MonoBehaviour
{
    //Bestiario count
    private int greenSkeleton = 0;
    private int goblin = 0;
    private int observer = 0;
    private int falseBoss = 0;
    private int theRealBoss = 0;

    [HideInInspector] public static Data_Control instance = null;

    [HideInInspector] public enum DoorState { CLOSE, OPEN_FIRST_TIME, OPEN };

    ////Player Data
    private Vector3 playerPos = new Vector3(-23.0f, 4.8f, 0.0f);

    ////Door Data zone 1
    public DoorState[] doorsStates_z1 = { DoorState.CLOSE, DoorState.CLOSE, DoorState.CLOSE };

    //////Coins
    ////Zone1
    [Header("Zone-1 Coins")]
    //Room1
    [SerializeField] private bool[] z1_Coins_1 = { false, false };
    //Room2
    [SerializeField] private bool[] z1_Coins_2 = { false, false, false, false };
    //Room3
    [SerializeField] private bool[] z1_Coins_3 = { false, false, false };
    //Room4
    [SerializeField] private bool[] z1_Coins_4 = { false, false };
    //Room5
    [SerializeField] private bool[] z1_Coins_5 = { false, false, false} ;
    //Room6
    [SerializeField] private bool[] z1_Coins_6 = { false, false, false , false };
    //Room7
    [SerializeField] private bool[] z1_Coins_7 = { false, false, false, false };
    //Room8
    [SerializeField] private bool[] z1_Coins_8 = { false, false, false };
    ////Zone2

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

    public void AddToDeathCount(Enemy_AI.EnemyID _enemyID)
    {
        switch (_enemyID)
        {
            case (Enemy_AI.EnemyID.GREENSKELETON):
                greenSkeleton++;
                break;
            case (Enemy_AI.EnemyID.GOBLIN):
                goblin++;
                break;
            case (Enemy_AI.EnemyID.OBSERVER):
                observer++;
                break;
        }
    }

    public int ReturnDeathCount(Enemy_AI.EnemyID _enemyID)
    {
        switch (_enemyID)
        {
            case (Enemy_AI.EnemyID.GREENSKELETON):
                return greenSkeleton;
            case (Enemy_AI.EnemyID.GOBLIN):
                return goblin;
            case (Enemy_AI.EnemyID.OBSERVER):
                return observer;
            default:
                return 0;
        }
    }

    public int GetFalseBossCount() { return falseBoss; }

    public void SetFalseBossCount() { falseBoss++; }

    public bool GetCoinsState(int zone, int room, int coinID)
    {
        switch(zone)
        {
            case 1:
                switch(room)
                {
                    case 1:
                        return z1_Coins_1[coinID];
                    case 2:
                        return z1_Coins_2[coinID];
                    case 3:
                        return z1_Coins_3[coinID];
                    case 4:
                        return z1_Coins_4[coinID];
                    case 5:
                        return z1_Coins_5[coinID];
                    case 6:
                        return z1_Coins_6[coinID];
                    case 7:
                        return z1_Coins_7[coinID];
                    case 8:
                        return z1_Coins_8[coinID];
                    default:
                        return false;
                }
            default:
                return false;
            //case 2:

            //    break;
        }
    }

    public void SetCoinState(int zone, int room, int coinID, bool newState)
    {
        switch (zone)
        {
            case 1:
                switch (room)
                {
                    case 1:
                        z1_Coins_1[coinID] = newState;
                        break;
                    case 2:
                        z1_Coins_2[coinID] = newState;
                        break;
                    case 3:
                        z1_Coins_3[coinID] = newState;
                        break;
                    case 4:
                        z1_Coins_4[coinID] = newState;
                        break;
                    case 5:
                        z1_Coins_5[coinID] = newState;
                        break;
                    case 6:
                        z1_Coins_6[coinID] = newState;
                        break;
                    case 7:
                        z1_Coins_7[coinID] = newState;
                        break;
                    case 8:
                        z1_Coins_8[coinID] = newState;
                        break;
                }
                break;
                //case 2:

                //    break;
        }
    }

    public Vector3 GetPlayerPos() { return playerPos; }

}
