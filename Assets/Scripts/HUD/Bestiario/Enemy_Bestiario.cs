﻿using System.Collections;
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
        count = bestiarioCount.GetComponent<Bestiario_Count>().ReturnDeathCount(enemyID);
        texts[2].text = "Count: " + count;
    }

    public void AddCount()
    {
        count++;
    }


}
