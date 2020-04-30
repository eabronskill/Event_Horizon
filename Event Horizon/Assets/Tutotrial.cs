using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool phase1, phase2, phase3 = true;
    private List<GameObject> enemies;

    private void Awake()
    {
        light.color = Color.red;
        players = new List<GameObject>();
        enemies = new List<GameObject>();
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
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
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
