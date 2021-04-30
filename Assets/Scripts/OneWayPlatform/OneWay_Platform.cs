using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay_Platform : MonoBehaviour
{

    private PlatformEffector2D oneWayPlatform = null;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        oneWayPlatform = GetComponent<PlatformEffector2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(player.GetComponent<Player_Movement>().GetOneWayPlatformState())
        {
            oneWayPlatform.rotationalOffset = 180.0f;
        }
        else
        {
            oneWayPlatform.rotationalOffset = 0.0f;
        }
    }


}
