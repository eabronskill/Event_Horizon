using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;


public class GameOverController : MonoBehaviour
{
    Rewired.Player player1;
    
    // Update is called once per frame
    void Update()
    {
        if (player1.GetButtonDown("Play"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
