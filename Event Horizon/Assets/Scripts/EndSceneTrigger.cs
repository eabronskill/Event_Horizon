using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneTrigger : MonoBehaviour
{
    Rewired.Player player;
    public List<Light> lights;
    public List<GameObject> enemies;
    public GameObject text;

    private bool phaseBefore = true;
    private bool phaseAfter = false;

    private void Start()
    {
        text.SetActive(false);
    }


    void FixedUpdate()
    {
        if (phaseBefore)
        {
            bool done = true;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    done = false;
                }

            }
            if (done)
            {
                phaseBefore = false;
                phaseAfter = true;
            }
        }
        else if (phaseAfter)
        {
            text.GetComponent<TextMeshPro>().text = "Press 'A' to escape!";

            foreach (Light light in lights)
                light.color = Color.white;
        }
    }


    void OnTriggerStay(Collider col)
    {
        if (phaseAfter)
        {
            if (col.gameObject.tag == "Player")
            {
                if (col.gameObject.name.Equals("Tank Controller"))
                {
                    player = col.GetComponent<TankInput>().player;
                    if (player.GetButton("Interact"))
                    {
                        col.GetComponent<TankInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Soldier Controller"))
                {
                    player = col.GetComponent<SoldierInput>().player;
                    if (player.GetButton("Interact"))
                    {
                        col.GetComponent<SoldierInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Engineer Controller"))
                {
                    player = col.GetComponent<TechnicianInput>().player;
                    if (player.GetButton("Interact"))
                    {
                        col.GetComponent<TechnicianInput>().victoryCanvas.SetActive(true);
                    }
                }
                if (col.gameObject.name.Equals("Rogue Controller"))
                {
                    player = col.GetComponent<rogueInput>().player;
                    if (player.GetButton("Interact"))
                    {
                        col.GetComponent<rogueInput>().victoryCanvas.SetActive(true);
                    }
                }
            }
        }
    }
}
