using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        if (ChS_Controller._singlePlayer) text.GetComponent<TextMeshPro>().text = "'E':\nHealing";
    }

    public void use()
    {
        player._curHealth += player._maxHealth * healingPercentage;
        if (player._curHealth > player._maxHealth)
        {
            player._curHealth = player._maxHealth;
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
