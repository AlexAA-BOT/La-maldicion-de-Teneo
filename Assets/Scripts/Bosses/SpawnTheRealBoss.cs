using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTheRealBoss : MonoBehaviour
{
    [SerializeField] private GameObject theRealBoss = null;


    public void SpawnBoss()
    {
        Instantiate(theRealBoss, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.AngleAxis( 180, new Vector3(0, 1, 0)));
        Destroy(this.gameObject);
    }

}
