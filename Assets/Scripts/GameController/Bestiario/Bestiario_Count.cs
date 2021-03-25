using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bestiario_Count : MonoBehaviour
{
    private int greenSkeleton = 0;
    private int goblin = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToDeathCount(Enemy_AI.EnemyID _enemyID)
    {
        switch(_enemyID)
        {
            case (Enemy_AI.EnemyID.GREENSKELETON):
                greenSkeleton++;
                break;
            case (Enemy_AI.EnemyID.GOBLIN):
                goblin++;
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
            default:
                return 0;
        }
    }

}
