using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAbilities : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canSetSpikes;
    public float spikeCD;

    public bool canSetMine;
    public bool mineSet;
    public float mineCD;

    public float mineTimeRemaining;
    public float spikeTimeRemaining;

    private float mineSetTime;
    private float spikeSetTime;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSetMine)
        {
            mineTimeRemaining = mineSetTime + mineCD - Time.time;
        }

        if (!canSetSpikes)
        {
           spikeTimeRemaining = spikeSetTime + spikeCD - Time.time;
        }
    }

    public void setSpikes()
    {
        canSetSpikes = false;
        spikeSetTime = Time.time;
        Invoke("resetSpikes", spikeCD);

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
        spikeSetTime = Time.time;
        Invoke("resetMine", mineCD);
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
