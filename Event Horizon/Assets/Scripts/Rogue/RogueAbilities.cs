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

    [HideInInspector] public float mineSetTime;
    [HideInInspector] public float spikeSetTime;

    public GameObject spikes;
    private GameObject mine;
    public GameObject minePrefab;
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
            mineTimeRemaining = mineSetTime - Time.time;
        }

        if (!canSetSpikes)
        {
           spikeTimeRemaining = spikeSetTime - Time.time;
        }
    }

    public void setSpikes()
    {
        canSetSpikes = false;
        spikeSetTime = Time.time + spikeCD;
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
        Invoke("resetMine", mineCD);
        mineSet = true;

        //actually put in mine
        Vector3 mineLoc = transform.position;
        mineLoc.y += .3f;
        mineLoc += transform.forward * 2;
        mine = Instantiate(minePrefab, mineLoc, transform.rotation);
    }

    public void detonate()
    {
        print("boom!");
        mine.SendMessage("explode");
        Destroy(mine);
        mineSet = false;
        mineSetTime = Time.time + mineCD;
    }

    private void resetMine()
    {
        canSetMine = true;
    }
}
