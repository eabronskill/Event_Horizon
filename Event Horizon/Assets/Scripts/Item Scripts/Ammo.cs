using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    public float ammoPercentage = 0.2f;
    public GameObject text;

    public void Start()
    {
        text.SetActive(false);
        if (ChS_Controller._singlePlayer) text.GetComponent<TextMeshPro>().text = "'E':\nHealing";
    }
    public void use()
    {
        player._curAmmo += player._maxAmmo * ammoPercentage;
        if (player._curAmmo > player._maxAmmo)
        {
            player._curAmmo = player._maxAmmo;
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
