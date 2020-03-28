using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LockedDoor : MonoBehaviour
{
    public int activated;
    public int requiredActivations;
    public Transform leftDest, rightDest;
    public GameObject left, right;
    public Light doorLight;
    private bool open = false;
    private Rewired.Player player;
    
    void Update()
    {
        if (open)
        {
            left.transform.position = Vector3.Lerp(left.transform.position, leftDest.position, 0.01f);
            right.transform.position = Vector3.Lerp(right.transform.position, rightDest.position, 0.01f);
        }
        if (activated >= requiredActivations)
        {
            doorLight.color = Color.green;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (activated >= requiredActivations)
        {

            if (col.gameObject.tag == "Player")
            {
                if (col.gameObject.name.Equals("Tank Controller"))
                {
                    player = col.GetComponent<TankInput>().player;
                    if (player.GetButtonDown("Interact"))
                    {
                        open = true;
                    }
                }
                if (col.gameObject.name.Equals("Soldier Controller"))
                {
                    player = col.GetComponent<SoldierInput>().player;
                    if (player.GetButtonDown("Interact"))
                    {
                        open = true;
                    }
                }
                //if (col.gameObject.name.Equals("Engineer Controller"))
                //{
                //    player = col.GetComponent<EngineerInput>().player;
                //    if (player.GetButtonDown("Interact"))
                //    {
                //        open = true;
                //    }
                //}
                //if (col.gameObject.name.Equals("Rogue Controller"))
                //{
                //    player = col.GetComponent<RogueInput>().player;
                //    if (player.GetButtonDown("Interact"))
                //    {
                //        open = true;
                //    }
                //}
            }
        }
    }
}
