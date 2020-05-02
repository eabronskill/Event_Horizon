using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Terminal : MonoBehaviour
{
    private Rewired.Player player;
    public GameObject lockedDoor;
    public Light light;
    private bool activated = false;

    public GameObject directions;
    public AudioSource open;
    private void Start()
    {
        directions.SetActive(false);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            directions.SetActive(true);

            if (col.gameObject.name.Equals("Tank Controller"))
            {
                player = col.GetComponent<TankInput>().player;
                if (player.GetButton("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                    open.Play();
                }
            }
            if (col.gameObject.name.Equals("Soldier Controller"))
            {
                player = col.GetComponent<SoldierInput>().player;
                if (player.GetButton("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                    open.Play();
                }
            }
            if (col.gameObject.name.Equals("Engineer Controller"))
            {
                player = col.GetComponent<TechnicianInput>().player;
                if (player.GetButton("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                    open.Play();
                }
            }
            if (col.gameObject.name.Equals("Rogue Controller"))
            {
                player = col.GetComponent<rogueInput>().player;
                if (player.GetButton("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                    open.Play();
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            directions.SetActive(false);
        }
    }
}
