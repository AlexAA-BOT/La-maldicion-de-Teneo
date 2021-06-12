using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject player = null;
    private bool menuInGame = false;
    private bool invincibility = false;
    [SerializeField] private float timeToRestart = 4.0f;
    private float timerRestart = 0.0f;
    [SerializeField] private float timeToEndDemo = 4.0f;
    private float timerEndDemo = 0.0f;
    [SerializeField] private GameObject menu_InGame = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        inputs();
        if(menuInGame && menu_InGame != null)
        {
            if(menu_InGame.activeSelf)
            {
                Time.timeScale = 1.0f;
                menuInGame = false;
                menu_InGame.SetActive(false);
            }
            else
            {
                Time.timeScale = 0.0f;
                menuInGame = false;
                menu_InGame.SetActive(true);
                
            }
        }

        if(invincibility)
        {
            bool state = !player.GetComponent<Player_Attack>().GetInvincibility();
            player.GetComponent<Player_Attack>().SetInvincibility(state);
            invincibility = false;
        }

        RestartGame();

    }

    private void inputs()
    {
        if(Input.GetButtonDown("ESC"))
        {
            menuInGame = true;
        }

        if(Input.GetButtonDown("Invincibility"))
        {
            invincibility = true;
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("1-RoomSecret-Shop");
            player.transform.position = new Vector3(-23.0f, -2.3f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.GetComponent<Player_Inventory>().SetMoney(100);
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("1-Room-Boss");
            player.transform.position = new Vector3(-24.0f, 4.9f, 0.0f);
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            if(GameObject.FindGameObjectWithTag("FalseBoss") != null)
            {
                GameObject.FindGameObjectWithTag("FalseBoss").GetComponent<False_Boss_AI>().SetHealth(75);
            }
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (GameObject.FindGameObjectWithTag("FalseBoss") != null)
            {
                GameObject.FindGameObjectWithTag("FalseBoss").GetComponent<False_Boss_AI>().SetHealth(0);
            }
        }

    }

    private void RestartGame()
    {
        if(player.GetComponent<Player_Attack>().GetHealth() <= 0.0f)
        {
            if (timerRestart <= 0.0f)
            {
                timerRestart += Time.deltaTime;
                Data_Control.instance.RestartCoins_Z1();
            }
            else if (timerRestart >= timeToRestart)
            {
                Destroy(player);
                SceneManager.LoadScene("1-Room-1");
                timerRestart = 0.0f;
            }
            else
            {
                timerRestart += Time.deltaTime;
            }
        }
    }

    public void EndDemo()
    {
        if (timerEndDemo >= timeToEndDemo)
        {
            Data_Control.instance.RestartCoins_Z1();
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            SceneManager.LoadScene("End-Demo");
            timerEndDemo = 0.0f;
        }
        else
        {
            timerEndDemo += Time.deltaTime;
        }
    }

}
