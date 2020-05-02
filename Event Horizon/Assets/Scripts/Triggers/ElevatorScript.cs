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
    private bool tank, soldier, rogue, engineer;
   
    public List<GameObject> enemies;
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
    void FixedUpdate()
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
                text.GetComponent<TextMeshPro>().text = "Press 'A' to Activate Elevator";
                front.gameObject.SetActive(true);
                inside.color = Color.green;
                phase2 = false;
                phase3 = true;
            }
            


            bool done = true;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    done = false;
                }

            }
            if (done)
            {
                print("done!");
                phase2 = false;
                phase3 = true;
                text.GetComponent<TextMeshPro>().text = "Press 'A' to Activate Elevator";
                front.gameObject.SetActive(true);
                inside.color = Color.green;
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
               
                // Activate all the enemies
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().active = true;
                }

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
                    tank = true;
                    player1 = col.GetComponent<TankInput>().player;
                    if (player1.GetButtonDown("Interact"))
                    {
                        col.GetComponent<TankInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Soldier Controller"))
                {
                    soldier = true;
                    player2 = col.GetComponent<SoldierInput>().player;
                    if (player2.GetButtonDown("Interact"))
                    {
                        col.GetComponent<SoldierInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Engineer Controller"))
                {
                    engineer = true;
                    player3 = col.GetComponent<TechnicianInput>().player;
                    if (player3.GetButtonDown("Interact"))
                    {
                        col.GetComponent<TechnicianInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Rogue Controller"))
                {
                    rogue = true;
                    player4 = col.GetComponent<rogueInput>().player;
                    if (player4.GetButtonDown("Interact"))
                    {
                        col.GetComponent<rogueInput>().victoryCanvas.SetActive(true);
                    }
                }
            }
        }
        
    }
}
