using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAbilities : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canSetSpikes;
    private float spikeCooldown;

    public bool canSetMine;
    public bool mineSet;
    private float mineCooldown;

    public GameObject spikes;
    public GameObject mine;
    public GameObject grenade;
    //Enemy enemy;

    void Start()
    {
        canSetSpikes = true;
        //enemy = new Enemy();
        //enemy.mineExplosionEvent += detonate;
        canSetMine = true;
        mineSet = false;
        spikeCooldown = 3.0f;
        mineCooldown = 3.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setSpikes()
    {
        canSetSpikes = false;
        Invoke("resetSpikes", spikeCooldown);

        //actually put in spikes
        Instantiate(spikes, transform.position, transform.rotation);
    }

    private void resetSpikes()
    {
        canSetSpikes = true;
    }

    public void setMine()
    {
        canSetMine = false;
        Invoke("resetMine", mineCooldown);
        mineSet = true;
        //actually put in mine
        Vector3 mineLoc = transform.position;
        mineLoc.y += .3f;
        mineLoc += transform.forward * 2;
        mine = Instantiate(mine, mineLoc, transform.rotation);
    }

    public void detonate()
    {
        print("boom!");
        mine.SendMessage("explode");
        Destroy(mine);
        mineSet = false;
    }

    private void resetMine()
    {
        canSetMine = true;
    }
}
