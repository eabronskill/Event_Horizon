using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private List<GameObject> players;
    private GameObject target;
    private NavMeshAgent nav;
    /// <summary>
    /// Set in Inspecter to true if you want this enemy to be moving on start. Or, use triggers to make this
    /// enemy active.
    /// </summary>
    public bool active = false;

    // Stat vars
    public int currHP;
    public int rangedHP;
    public int meleeHP;
    private float initialSpeed;

    // Damage vars
    public float attackRange = 7f;
    public float attackSpeed = 1f;
    public bool melee;
    public bool ranged;
    private float damage;
    public float meleeDamage = 10f;
    public float rangedDamage = 30f;
    private float attackTimer = 0f;
    public float meleeAttackTime = 2f;
    public float rangedAttackTime = 4f;
    public Transform attackPoint;
    public GameObject bulletPrefab;

    // Animation Declairation
    public Animator enemyAnimator;

    // Target change vars
    private float changeTargetTimer = 0f;
    public float changeTargetTime = 5f;
    private bool rotating = false;
    
    // Death roll vars
    private Quaternion initRot;
    private Quaternion fallRot;
    private Vector3 deathLoc;
    private Vector3 bulletImpact;

    // Player interaction vars
    private float stunnedTimer = 0f;

    public bool canBeStunned = true;
    
    bool dead = false;
    float deathTimer = 0f;

    //sounds
    public AudioSource attacknoise;
    public AudioSource bulletHit;
    public AudioSource meleeHit;
    public AudioSource rangedAttack;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.activeSelf)
            {
                players.Add(p);
            }
        }
        print("Players assigned to enemy." + players.Count);
        
        nav = GetComponent<NavMeshAgent>();
        if (melee)
        {
            damage = meleeDamage;
            currHP = meleeHP;
            ranged = false;
        }
        else if (ranged)
        {
            damage = rangedDamage;
            currHP = rangedHP;
            melee = false;
        }

        initialSpeed = nav.speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        // If dead...
        if (dead && deathTimer > Time.time) 
        {
            transform.position = deathLoc;
            transform.localRotation = Quaternion.Lerp(fallRot, transform.localRotation, deathTimer - Time.time);
        }
        else 
        {
            
            if (Time.time > stunnedTimer && active)
            {
                if (target != null && !target.activeSelf)
                {
                    setPathClosestPlayer();
                }
                if (target != null && nav.remainingDistance - nav.stoppingDistance <= 0)
                {
                    transform.LookAt(target.transform.position);
                }
                // This enemy has been activated and has no path.
                if (active && !nav.hasPath)
                {
                    setPathClosestPlayer();
                    print("Setting Path");
                }
                // This melee enemy has not reached its destination and it is time to find the nearest enemy.
                else if (melee && nav.remainingDistance - nav.stoppingDistance > 0 && Time.time > changeTargetTimer)
                {
                    setPathClosestPlayer();
                    print("Resetting Path");
                }
                // This ranged enemy has not reached its destination and it is time to find the nearest enemy.
                else if (ranged && nav.remainingDistance - nav.stoppingDistance <= 0 && Time.time > changeTargetTimer)
                {
                    setPathClosestPlayer();
                    print("Resetting Path");
                }
                // This enemy has not reached its destination.
                else if (nav.remainingDistance - nav.stoppingDistance > 0)
                {
                    nav.isStopped = false;
                    nav.SetDestination(target.transform.position);
                    if (melee)
                    {
                        enemyAnimator.SetBool("Walking", true);
                    }
                }
                // This enemy has reached its destination.
                else if (nav.remainingDistance - nav.stoppingDistance <= 0 && Time.time > attackTimer)
                {
                    nav.isStopped = true;
                    // Play animation HERE:
                    Invoke("attack", attackSpeed);
                    if (melee)
                    {
                        attackTimer = Time.time + meleeAttackTime;

                        //TODO: impliment correctly
                        if (melee)
                        {
                            enemyAnimator.SetTrigger("Attack");
                        }
                    }
                    else if (ranged)
                    {
                        attackTimer = Time.time + rangedAttackTime;
                    }
                    
                }
            }
            else
            {
                nav.isStopped = true;
            }
        }
    }

    /// <summary>
    /// Looks through all the player objects and finds the closest one. Sets that player to be the target of
    /// this enemy.
    /// </summary>
    private void setPathClosestPlayer()
    {
        float smallestDist = 2000000;
        foreach (GameObject player in players)
        {
            if (!player.activeSelf)
            {
                continue;
            }
            Vector3 dist = transform.position - player.transform.position;
            if (dist.magnitude < smallestDist)
            {
                smallestDist = dist.magnitude;
                target = player;
            }
        }
        changeTargetTimer = Time.time + changeTargetTime;
        nav.SetDestination(target.transform.position);
        print(target.name);
    }

    /// <summary>
    /// Called when this enemy can attack.
    /// </summary>
    private void attack()
    {
        
        // Melee attack logic
        if (melee)
        {
            
            foreach (GameObject player in players)
            {
                Vector3 playerDir = transform.position - player.transform.position;
                float angle = Vector3.Angle(playerDir, transform.forward * -1);

                if (angle < 45f && playerDir.magnitude <= attackRange)
                {
                    if (!attacknoise.isPlaying)
                    {
                        attacknoise.Play();
                    }
                    if (player.gameObject.name.Equals("Tank Controller"))
                    {
                        player.GetComponent<TankInput>().takeDamage(damage);
                    }
                    if (player.gameObject.name.Equals("Soldier Controller"))
                    {
                        player.GetComponent<SoldierInput>().takeDamage(damage);
                    }
                    if (player.gameObject.name.Equals("Engineer Controller"))
                    {
                        player.GetComponent<TechnicianInput>().takeDamage(damage);
                    }
                    if (player.gameObject.name.Equals("Rogue Controller"))
                    {
                        player.GetComponent<rogueInput>().takeDamage(damage);
                    }
                }
            }
        }
        else if (ranged)
        {
            Instantiate(bulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            if (!rangedAttack.isPlaying)
            {
                rangedAttack.Play();
            }
        }
        nav.SetDestination(target.transform.position);
    }

    /// <summary>
    /// Used for the grenade. Instantly blows them up if they take enough damage.
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(int damage)
    {
        currHP = currHP - damage;
        if (currHP <= 0)
        {
            int num = Random.Range(0, 3);

            deathLoc = transform.position;
            initRot = transform.localRotation;
            fallRot = initRot;

            if (num == 0)
                fallRot.x -= .5f;
            else
                fallRot.x += .5f;

            if (num == 1)
                fallRot.z -= .5f;
            else
                fallRot.z += .5f;

            deathTimer = Time.time + 1f;
            dead = true;
            Destroy(this.gameObject.GetComponent<Collider>()); //this should destroy the collider as well
            Invoke("die", 1f);
        }
    }


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
         
           
            bulletHit.Play();
            
            
            currHP = currHP - 10;
            if (currHP <= 0)
            {
                bulletImpact = coll.transform.position;

                deathLoc = transform.position;
                initRot = transform.localRotation;
                fallRot = initRot;

                if (bulletImpact.x > 0)
                    fallRot.x -= .5f;
                else
                    fallRot.x += .5f;

                if (bulletImpact.z > 0)
                    fallRot.z -= .5f;
                else
                    fallRot.z += .5f;

                deathTimer = Time.time + 1f;
                dead = true;
                Destroy(this.gameObject.GetComponent<Collider>()); //this should destroy the collider as well
                Invoke("die", 1f);
            }
            
        }
        if (coll.gameObject.tag == "melee")
        {

            
            meleeHit.Play();


            currHP = currHP - 20;
            if (currHP <= 0)
            {
                bulletImpact = coll.transform.position;

                deathLoc = transform.position;
                initRot = transform.localRotation;
                fallRot = initRot;

                if (bulletImpact.x > 0)
                    fallRot.x -= .5f;
                else
                    fallRot.x += .5f;

                if (bulletImpact.z > 0)
                    fallRot.z -= .5f;
                else
                    fallRot.z += .5f;

                deathTimer = Time.time + 1f;
                dead = true;
                Destroy(this.gameObject.GetComponent<Collider>()); //this should destroy the collider as well
                Invoke("die", 1f);
            }

        }

        if (coll.gameObject.tag == "RogueShot")
        {


            bulletHit.Play();


            currHP = currHP - 50;
            if (currHP <= 0)
            {
                bulletImpact = coll.transform.position;

                deathLoc = transform.position;
                initRot = transform.localRotation;
                fallRot = initRot;

                if (bulletImpact.x > 0)
                    fallRot.x -= .5f;
                else
                    fallRot.x += .5f;

                if (bulletImpact.z > 0)
                    fallRot.z -= .5f;
                else
                    fallRot.z += .5f;

                deathTimer = Time.time + 1f;
                dead = true;
                Destroy(this.gameObject.GetComponent<Collider>()); //this should destroy the collider as well
                Invoke("die", 1f);
            }

        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SlowField")
        {
            nav.speed = nav.speed = 2.5f;
            Invoke("resetSpeed", 3f);
        }

       
        if (other.gameObject.tag == "Melee")
        {
            takeDamage(20);
        }
    }

    private void resetSpeed()
    {
        nav.speed = initialSpeed;
    }
    
    private void die()
    {
       
        Destroy(this.gameObject);
    }

    public void stun()
    {
        if (canBeStunned)
        {
            stunnedTimer = Time.time + 3;
            enemyAnimator.SetBool("Walking", false);
            takeDamage(20);
            meleeHit.Play();

        }

    }



}
