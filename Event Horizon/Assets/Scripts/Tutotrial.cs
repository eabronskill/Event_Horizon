using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutotrial : MonoBehaviour
{
    /* Phase 1 will be completed when all players move around
     * Phase 2 will be completed when all players use their abilities and shoot/melee
     * Phase 3 will be killing the enemies that spawn
     * Then the door opens.
     */
    public Transform leftDest, rightDest;
    public GameObject left, right;
    public Light light;
    private bool open = false;
    [HideInInspector]
    public static List<GameObject> players;
    public GameObject parent;
    private bool phase1, phase2, phase3 ;
    private List<GameObject> enemies;
    private bool activated = false;
    public GameObject textParent;
    public GameObject text;
    public GameObject health1, health2, health3, health4, ammo1, ammo2, ammo3, ammo4;
    private bool itemSpawn = false;
    public AudioSource sound;
    private bool hasPlayed = false;

    private void Awake()
    {
        light.color = Color.red;
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        phase1 = phase2 = phase3 = true;
        if (parent != null)
        {
            foreach (Enemy o in parent.GetComponentsInChildren<Enemy>())
            {
                enemies.Add(o.gameObject);
                print("Enemies added to list.");
            }

            
        }
    }

    void Update()
    {
        if (open)
        {
            textParent.SetActive(false);
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
            if (!sound.isPlaying && !hasPlayed)
            {
                sound.Play();
                hasPlayed = true;
            }
        }
        if (!phase3)
        {
            if (!itemSpawn) {
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                health4.SetActive(true);
                ammo1.SetActive(true);
                ammo2.SetActive(true);
                ammo3.SetActive(true);
                ammo4.SetActive(true);
                itemSpawn = true;
            }
           
            text.GetComponent<Text>().text = "Use A to pick up Items, Press A again to Use, and B to Drop an Equipped Item";
        }
        else if (phase1)
        {
            bool trigger = true;
            Player pl;
            foreach (GameObject p in players)
            {
                pl = p.GetComponent<Player>();
                if (!pl.moved)
                {
                    trigger = false;
                }
            }
            if (trigger)
            {
                phase1 = false;
            }
        }
        else if (phase2)
        {
            text.GetComponent<Text>().fontSize = 70;
            text.GetComponent<Text>().text = "Use RT to Shoot, LT to Melee, RB for Ability 1, and LB for Ability 2";
            bool trigger = true;
            Player pl;
            foreach (GameObject p in players)
            {
                pl = p.GetComponent<Player>();
                if (!pl.shot || !pl.meleed || !pl.usedAbility1 || !pl.usedAbility2)
                {
                    trigger = false;
                }
            }
            if (trigger)
            {
                phase2 = false;
            }
        }
        else if (phase3)
        {
            text.GetComponent<Text>().fontSize = 100;
            text.GetComponent<Text>().text = "Eliminate all Enemies in Area";
            if (!activated)
            {
                foreach (GameObject p in enemies)
                {
                    p.GetComponent<Enemy>().active = true;
                }
                activated = true;
            }
            bool trigger = true;
            foreach (GameObject p in enemies)
            {
                if (p != null)
                {
                    trigger = false;
                }
            }
            if (trigger)
            {
                phase3 = false;
                light.color = Color.green;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (!phase3 && col.gameObject.tag == "Player")
        {
            open = true;
        }
    }
}
