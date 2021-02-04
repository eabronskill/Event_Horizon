using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using TMPro;

public class ElevatorScript : MonoBehaviour
{
    public Light front, inside;
    Rewired.Player player1, player2, player3, player4;
   
    
    public GameObject text;

    private bool phase1 = true;
    private bool phase2 = false;
    private bool phase3 = false;

    //final horde functionality
    public float timer = 31f;

    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;

    public GameObject enemy;
    int offset;
    private float enemyTimer = 2f;
    private float UITimer;
    private int countdown;

    void Start()
    {
        inside.gameObject.SetActive(false);
        text.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (phase2)
        {
            if (Time.time < timer)
            {
                countdown = (int)(timer - Time.time);
                text.GetComponent<TextMeshPro>().text = "Defend: " + countdown + " Seconds";
                if (Time.time >= enemyTimer)
                {
                    Instantiate(enemy, spawn1.transform);

                    Instantiate(enemy, spawn2.transform);

                    Instantiate(enemy, spawn3.transform);

                    Instantiate(enemy, spawn4.transform);
                    enemyTimer = Time.time + 2f;
                }
            }
            else
            {
                inside.gameObject.SetActive(false);
                if (CharacterSelectController._singlePlayer)
                {
                    text.GetComponent<TextMeshPro>().text = "Press 'E' to Activate Elevator";
                }
                else text.GetComponent<TextMeshPro>().text = "Press 'A' to Activate Elevator";
                front.gameObject.SetActive(true);
                inside.color = Color.green;
                phase2 = false;
                phase3 = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (phase1)
        {
            if (col.gameObject.tag == "Player")
            {
                front.gameObject.SetActive(false);
                inside.gameObject.SetActive(true);
                text.SetActive(true);
               

                phase1 = false;
                phase2 = true;
                timer += Time.time;
                enemyTimer += Time.time;
            }
        }
        
    }

    void OnTriggerStay(Collider col)
    {
        if (phase3)
        {
            if (col.gameObject.tag == "Player")
            {
                if (col.gameObject.name.Equals("Tank Controller"))
                {
                    player1 = col.gameObject.GetComponent<TankInput>()._player;
                    if (player1.GetButton("Interact"))
                    {
                        col.gameObject.GetComponent<TankInput>()._victoryCanvas.SetActive(true);
                        phase3 = false;
                    }
                }
                if (col.gameObject.name.Equals("Soldier Controller"))
                {
                    player2 = col.gameObject.GetComponent<SoldierInput>()._player;
                    if (player2.GetButton("Interact"))
                    {
                        col.gameObject.GetComponent<SoldierInput>()._victoryCanvas.SetActive(true);
                        phase3 = false;
                    }
                }
                if (col.gameObject.name.Equals("Engineer Controller"))
                {
                    player3 = col.gameObject.GetComponent<TechnicianInput>()._player;
                    if (player3.GetButton("Interact"))
                    {
                        col.gameObject.GetComponent<TechnicianInput>()._victoryCanvas.SetActive(true);
                        phase3 = false;
                    }
                }
                if (col.gameObject.name.Equals("Rogue Controller"))
                {
                    player4 = col.gameObject.GetComponent<rogueInput>()._player;
                    if (player4.GetButton("Interact"))
                    {
                        col.gameObject.GetComponent<rogueInput>()._victoryCanvas.SetActive(true);
                        phase3 = false;
                    }
                }
            }
        }
    }
}
