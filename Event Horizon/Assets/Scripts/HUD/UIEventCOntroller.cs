using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


public class UIEventCOntroller : MonoBehaviour
{
    public GameObject PauseMenu, playerOne, playerTwo, playerThree, playerFour, gameOver, missionSuccess;
    Rewired.Player player1;
    public static Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        //if (player1.GetButtonDown("Play"))
        //{
        //    Pause();
        //}

        if (playerOne.activeSelf == false && playerTwo.activeSelf == false && playerThree.activeSelf == false && playerFour.activeSelf == false)
        {
            print("Game Over");
            GameOver();  
        }

        if (missionSuccess.activeSelf)
        {
            MissionSuccess();
        }
        
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        foreach (GameObject o in players.Values)
        {
            o.gameObject.SetActive(true);
        }
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void MissionSuccess()
    {
        foreach (GameObject o in players.Values)
        {
            o.gameObject.SetActive(true);
        }
        //missionSuccess.SetActive(true);
        Time.timeScale = 0;
    }
}
