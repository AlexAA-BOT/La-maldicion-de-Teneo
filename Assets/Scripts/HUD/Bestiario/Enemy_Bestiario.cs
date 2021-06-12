using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy_Bestiario : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject[] images = null;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI[] texts = null;
    [Space]
    [SerializeField] private Enemy_AI.EnemyID enemyID = Enemy_AI.EnemyID.GREENSKELETON;
    [SerializeField] private enum BossID { FALSEBOSS, THEREALBOSS, NONE };
    [SerializeField] private BossID bossID = BossID.NONE;
    private int count = 0;
    private GameObject bestiarioCount = null;

    // Start is called before the first frame update
    void Start()
    {
        images[1].SetActive(false);
        texts[1].gameObject.SetActive(false);
        bestiarioCount = GameObject.FindGameObjectWithTag("BestiarioCount");
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyID != Enemy_AI.EnemyID.NONE)
            count = Data_Control.instance.ReturnDeathCount(enemyID);
        else
            switch(bossID)
            {
                case BossID.FALSEBOSS:
                    count = Data_Control.instance.GetFalseBossCount();
                    break;
                case BossID.THEREALBOSS:
                    count = Data_Control.instance.GetTheRealBossCount();
                    break;
            }
        texts[2].text = "Count: " + count;
        UpdateImages();
    }

    private void UpdateImages()
    {
        if(count > 0)
        {
            images[0].SetActive(false);
            images[1].SetActive(true);

            texts[0].gameObject.SetActive(false);
            texts[1].gameObject.SetActive(true);
        }
    }

    public void AddCount()
    {
        count++;
    }


}
