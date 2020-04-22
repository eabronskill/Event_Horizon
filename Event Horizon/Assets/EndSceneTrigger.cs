﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneTrigger : MonoBehaviour
{
    Rewired.Player player;
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.name.Equals("Tank Controller"))
            {
                player = col.GetComponent<TankInput>().player;
                if (player.GetButton("Interact"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            if (col.gameObject.name.Equals("Soldier Controller"))
            {
                player = col.GetComponent<SoldierInput>().player;
                if (player.GetButton("Interact"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            if (col.gameObject.name.Equals("Engineer Controller"))
            {
                player = col.GetComponent<TechnicianInput>().player;
                if (player.GetButton("Interact"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            if (col.gameObject.name.Equals("Rogue Controller"))
            {
                player = col.GetComponent<rogueInput>().player;
                if (player.GetButton("Interact"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}
