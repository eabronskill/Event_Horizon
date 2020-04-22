using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


public class UIEventCOntroller : MonoBehaviour
{
    public GameObject PauseMenu, playerOne, playerTwo, playerThree, playerFour, gameOver, missionSuccess;
    Rewired.Player player1;
    // Start is called before the first frame update
    void Start()
    {
        
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
            GameOver();  
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
        Time.timeScale = 0f;
        gameOver.SetActive(true);
    }

    public void MissionSuccess()
    {
        Time.timeScale = 0f;
        missionSuccess.SetActive(true);
    }
}
