using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject player = null;
    private bool exitGame = false;
    private bool invincibility = false;
    [SerializeField] private float timeToRestart = 4.0f;
    private float timerRestart = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        inputs();
        if(exitGame)
        {
            Application.Quit();
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
        exitGame = Input.GetButton("ESC");

        if(Input.GetButtonDown("Invincibility"))
        {
            invincibility = true;
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

}
