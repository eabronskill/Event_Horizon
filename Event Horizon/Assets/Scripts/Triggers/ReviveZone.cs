using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveZone : MonoBehaviour
{
    public List<Transform> spawns;
    public List<GameObject> characters;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            print("Entereed");
            if (col.gameObject.GetComponent<Player>().player.GetButtonDown("Interact"))
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].gameObject.activeSelf == false)
                    {
                        print(characters[i].gameObject.name);
                        characters[i].gameObject.SetActive(true);
                        characters[i].gameObject.transform.position = spawns[i].position;
                    }
                }
            }
        }
    }
}
