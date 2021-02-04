using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveZone : MonoBehaviour
{
    public List<Transform> spawns;
    public List<GameObject> characters;
    public GameObject text;

    private void Start()
    {
        if (ChS_Controller._singlePlayer) text.SetActive(false);
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player>()._player.GetButtonDown("Interact") && !ChS_Controller._singlePlayer)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].gameObject.activeSelf == false)
                    {
                        characters[i].gameObject.SetActive(true);
                        characters[i].gameObject.transform.position = spawns[i].position;
                    }
                }
            }
        }
    }
}
