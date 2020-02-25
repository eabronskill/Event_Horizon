using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    public float healingPercentage;

    public void use()
    {
        player.curHealth += player.maxHealth * healingPercentage;
        if (player.curHealth > player.maxHealth)
        {
            player.curHealth = player.maxHealth;
        }
        Destroy(this.gameObject);
    }
}
