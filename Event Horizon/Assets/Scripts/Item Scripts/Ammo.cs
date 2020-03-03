using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    public float ammoPercentage = 0.2f;

    public void use()
    {
        player.curAmmo += player.maxAmmo * ammoPercentage;
        if (player.curAmmo > player.maxAmmo)
        {
            player.curAmmo = player.maxAmmo;
        }
        Destroy(this.gameObject);
    }

}
