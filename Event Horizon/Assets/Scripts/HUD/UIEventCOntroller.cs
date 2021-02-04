using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class UIEventCOntroller : MonoBehaviour
{
    public GameObject PauseMenu, playerOne, playerTwo, playerThree, playerFour, gameOver, missionSuccess;
    Rewired.Player player1;
    public static Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    public GameObject tut;
    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOne.activeSelf == false && playerTwo.activeSelf == false && playerThree.activeSelf == false && playerFour.activeSelf == false)
        {
            GameOver();
            if (tut)
            {
                tut.SetActive(false);
            }
        }

        if (missionSuccess.activeSelf)
        {
            MissionSuccess();
        }
        
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
        Time.timeScale = 0;
    }
}
