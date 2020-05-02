using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    public float healingPercentage;
    public GameObject text;

    public void Start()
    {
        text.SetActive(false);
    }

    public void use()
    {
        player.curHealth += player.maxHealth * healingPercentage;
        if (player.curHealth > player.maxHealth)
        {
            player.curHealth = player.maxHealth;
        }
        Destroy(this.gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.SetActive(false);
        }
    }
}
