using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

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
        if (CharacterSelectController._singlePlayer) directions.GetComponent<TextMeshPro>().text = "Press 'E' to Activate.";
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" && !activated)
        {
            col.gameObject.GetComponent<Player>()._cantUse = true;

            directions.SetActive(true);

            if (col.gameObject.name.Equals("Tank Controller"))
            {
                player = col.GetComponent<TankInput>()._player;
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
                player = col.GetComponent<SoldierInput>()._player;
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
                player = col.GetComponent<TechnicianInput>()._player;
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
                player = col.GetComponent<rogueInput>()._player;
                if (player.GetButton("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                    open.Play();
                }
            }
        }
        else if (col.gameObject.tag == "Player" && activated)
        {
            col.gameObject.GetComponent<Player>()._cantUse = false;
            directions.SetActive(false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            directions.SetActive(false);
            col.gameObject.GetComponent<Player>()._cantUse = false;
        }
    }
}
