using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TheRealBoss_Event : MonoBehaviour
{
    [HideInInspector] public bool startEvent = false;
    private float timer = 0.0f;
    [SerializeField] private float timeToStartMusic = 5.0f;
    private bool isMusic = false;
    [SerializeField] private float timeToStartBoss = 8.0f;
    [SerializeField] private AudioSource m_audioSource = null;
    [SerializeField] private AudioClip bossEntry = null;
    [SerializeField] private GameObject bossAnimation = null;

    [Header("Boss-HealtBar")]
    [SerializeField] private Slider healthBar = null;
    [SerializeField] private Image healtBar_Fill = null;
    [SerializeField] private Image healtBar_Borders = null;
    [SerializeField] private TextMeshProUGUI text = null;
    [Space]
    private float timerHealthBar = 0.0f;
    private bool realBossDead = false;
    [SerializeField] GameObject gameController = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startEvent)
        {
            if(timer >= timeToStartBoss && bossAnimation != null)
            {
                bossAnimation.GetComponent<Animator>().SetTrigger("Start-Enter");
                HealthBarAppear();
                timer += Time.deltaTime;
            }
            else if(timer >= timeToStartMusic && !isMusic)
            {
                m_audioSource.Stop();
                m_audioSource.PlayOneShot(bossEntry);
                isMusic = true;
                timer += Time.deltaTime;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if(realBossDead)
        {
            RestBossHealthBar();
        }

    }

    private void HealthBarAppear()
    {
        if(timerHealthBar >= 255.0f)
        {
            healtBar_Borders.color = new Color(healtBar_Borders.color.r, healtBar_Borders.color.g, healtBar_Borders.color.b, 255.0f);
            healtBar_Fill.color = new Color(healtBar_Fill.color.r, healtBar_Fill.color.g, healtBar_Fill.color.b, 255.0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 255.0f);
        }
        else
        {
            timerHealthBar += Time.deltaTime;
            healtBar_Borders.color = new Color(healtBar_Borders.color.r, healtBar_Borders.color.g, healtBar_Borders.color.b, timerHealthBar);
            healtBar_Fill.color = new Color(healtBar_Fill.color.r, healtBar_Fill.color.g, healtBar_Fill.color.b, timerHealthBar);
            text.color = new Color(text.color.r, text.color.g, text.color.b, timerHealthBar);
        }
    }

    public void RestBossHealthBar()
    {
        if (healthBar.value > 0.0f)
            healthBar.value -= Time.deltaTime;
        else
        {
            healthBar.value = 0.0f;
            gameController.GetComponent<GameController>().EndDemo();
        }
            

        realBossDead = true;
    }

}
