using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameObject player = null;
    
    [SerializeField] private string nextLevel = null;
    [SerializeField] private string lastLevel = null;
    [SerializeField] private string thirdLevel = null;

    [SerializeField] private GameObject collNextLev = null;
    [SerializeField] private bool verticalNextLevExit = false;
    [SerializeField] private GameObject collLastLev = null;
    [SerializeField] private bool verticalLastLevExit = false;

    bool collisionNext = false;
    bool collisionLast = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(collNextLev != null)
        {
            collisionNext = collNextLev.GetComponent<Scene_Exit>().exit;
        }
        
        if(collNextLev != null)
        {
            collisionLast = collLastLev.GetComponent<Scene_Exit>().exit;
        }
        

        if(collisionNext)
        {
            SceneManager.LoadScene(nextLevel);
            if(verticalNextLevExit)
            {
                if (player.transform.position.y > 0.0f)
                {
                    player.transform.position = new Vector3(player.transform.position.x, -9.0f, player.transform.position.z); // y = -11.0f -> cambiado para quye salga arriba
                    //player.GetComponent<Player_Movement>().GetRigidBody().AddForce(new Vector2(player.GetComponent<Player_Movement>().GetRigidBody().velocity.x, 100.0f), ForceMode2D.Impulse);
                }
                else
                {
                    player.transform.position = new Vector3(player.transform.position.x, 10.0f, player.transform.position.z);
                }
            }
            else
            {
                if(player.transform.position.x > 0.0f)
                {
                    player.transform.position = new Vector3(-23.0f, player.transform.position.y, player.transform.position.z);
                }
                else
                {
                    player.transform.position = new Vector3(23.0f, player.transform.position.y, player.transform.position.z);
                }

            }
            Data_Control.instance.SavePlayerPos(player.transform.position);
        }
        else if(collisionLast)
        {
            SceneManager.LoadScene(lastLevel);
            if (verticalLastLevExit)
            {
                if (player.transform.position.y > 0.0f)
                {
                    player.transform.position = new Vector3(player.transform.position.x, -9.0f, player.transform.position.z);
                    player.GetComponent<Player_Movement>().GetRigidBody().AddForce(new Vector2(player.GetComponent<Player_Movement>().GetRigidBody().velocity.x, 10.0f), ForceMode2D.Impulse);
                }
                else
                {
                    player.transform.position = new Vector3(player.transform.position.x, 10.0f, player.transform.position.z);
                }
            }
            else
            {
                if (player.transform.position.x > 0.0f)
                {
                    player.transform.position = new Vector3(-23.0f, player.transform.position.y, player.transform.position.z);
                }
                else
                {
                    player.transform.position = new Vector3(23.0f, player.transform.position.y, player.transform.position.z);
                }

            }
            
        }

    }


}
