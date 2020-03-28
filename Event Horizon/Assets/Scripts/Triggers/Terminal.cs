﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Terminal : MonoBehaviour
{
    private Rewired.Player player;
    public GameObject lockedDoor;
    public Light light;
    private bool activated = false;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.name.Equals("Tank Controller"))
            {
                player = col.GetComponent<TankInput>().player;
                if (player.GetButtonDown("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                }
            }
            if (col.gameObject.name.Equals("Soldier Controller"))
            {
                player = col.GetComponent<SoldierInput>().player;
                if (player.GetButtonDown("Interact") && !activated)
                {
                    lockedDoor.GetComponent<LockedDoor>().activated++;
                    activated = true;
                    light.color = Color.green;
                }
            }
            //if (col.gameObject.name.Equals("Engineer Controller"))
            //{
            //    player = col.GetComponent<EngineerInput>().player;
            //    if (player.GetButtonDown("Interact") && !activated)
            //    {
            //        lockedDoor.GetComponent<LockedDoor>().activated++;
            //        activated = true;
            //        light.color = Color.green;
            //    }
            //}
            //if (col.gameObject.name.Equals("Rogue Controller"))
            //{
            //    player = col.GetComponent<RogueInput>().player;
            //    if (player.GetButtonDown("Interact") && !activated)
            //    {
            //        lockedDoor.GetComponent<LockedDoor>().activated++;
            //        activated = true;
            //        light.color = Color.green;
            //    }
            //}
        }
    }
}
