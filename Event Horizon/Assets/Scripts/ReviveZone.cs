using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveZone : MonoBehaviour
{
    public List<Transform> spawns;
    public List<GameObject> characters;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            print("Entereed");
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].gameObject.activeSelf == false)
                {
                    print(characters[i].gameObject.name);
                    characters[i].gameObject.SetActive(true);
                    characters[i].gameObject.transform.position = spawns[i].position;
                    if (characters[i].gameObject.name.Equals("Tank Controller"))
                    {
                        characters[i].GetComponent<TankInput>().respawning();
                    }
                    if (characters[i].gameObject.name.Equals("Soldier Controller"))
                    {
                        characters[i].GetComponent<SoldierInput>().respawning();
                    }
                    //if (characters[i].gameObject.name.Equals("Engineer Controller"))
                    //{
                    //    characters[i].GetComponent<EngineerInput>().respawning();
                    //}
                    //if (characters[i].gameObject.name.Equals("Rogue Controller"))
                    //{
                    //    characters[i].GetComponent<RogueInput>().respawning();
                    //}
                }
            }
            //Destroy(gameObject);
        }
    }
}
