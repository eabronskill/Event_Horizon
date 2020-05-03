using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class SoldierInput : Player
{
    private SoldierAbilities abilities;

    /// <summary>
    /// ID of the player who is controlling this character.
    /// </summary>
    public int playerID;

    // Attacking vars
    public GameObject attackPoint;
    public GameObject bulletPrefab;
    private float hammerTimer = 0;
    private float strapTimer = 0;
    public float fireRate;
    public Sword sword;
    
    // Ability vars
    [HideInInspector]
    public float baseFireRate;
    [HideInInspector]
    public bool cnsmAmmo = true;
    [HideInInspector]
    public bool canUseGrenade = true;
    [HideInInspector]
    public bool canUseRF = true;
    private float grenadeTimer = 0.65f;

    // For Brett
    public float rapidFireCD;
    public float grenadeCD;

    //sounds
    public AudioSource gunshot;
    public AudioSource powerup;
    public AudioSource melee;

    void Awake()
    {
        curAmmo = maxAmmo;
        curClip = maxClip;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //if (testing)
        //{
        //    player = ReInput.players.GetPlayer(0);
        //    MultipleTargetCamera.targets.Add(this.gameObject);

        //    playerID = 1;
        //    controller = GetComponent<CharacterController>();
        //    if (UIEventCOntroller.players.Count == 0)
        //    {
        //        UIEventCOntroller.players.Add("Soldier", this.gameObject);
        //    }
        //    if (SceneManager.GetActiveScene().name == "Level1")
        //    {
        //        Tutotrial.players.Add(this.gameObject);
        //    }
        //}
        if (ChS_Controller.finalSelection.ContainsKey("Soldier Icon"))
        {
            player = ReInput.players.GetPlayer(ChS_Controller.finalSelection["Soldier Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            
            playerID = player.id;
            controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Soldier", this.gameObject);
            }
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Tutotrial.players.Add(this.gameObject);
            }

        }
        else
        {
            this.gameObject.SetActive(false);
        }

        baseFireRate = fireRate;
        abilities = this.gameObject.GetComponent<SoldierAbilities>();
    }

    // Update is called once per frame
    new void Update()
    {
        rapidFireCD = abilities.rapidFireTimeRemaining;
        grenadeCD = abilities.grenadeTimeRemaining;
        // Call the Player FixedUpdate method.
        base.Update();

        // Shooting
        if (canShooty)
        {
            if (player.GetButton("Shoot") && Time.time >= strapTimer && base.curClip > 0)
            {
                strapTimer = Time.time + fireRate;
                shot = true;
                Instantiate(bulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
                
                if (cnsmAmmo)
                {
                    base.curClip--;
                    gunshot.Play();
                }
            }
            if (player.GetButton("Shoot") && base.curClip > 0)
            {
                base.curMoveSpeed = movementSpeed * 0.5f;

            }
            else
            {
                base.curMoveSpeed = movementSpeed;
            }
        }
        

        // Ability 1: Rapid Fire
        if (player.GetButton("Ability1") && canUseRF)
        {
            // Call SoldierAbilities Script
            usedAbility1 = true;
            abilities.rapidFire();
            powerup.Play();
            
        }

        // Ability 2: Grenade 
        if (player.GetButtonDown("Ability2") && abilities.canUseGrenade)
        {
            // Call TankAbilities Script
            playerAnimator.SetTrigger("Grenade");
           // grenadeTimer = Time.time + grenadeCD;
            //abilities.Invoke("grenadeToss", 0.65f);
            abilities.grenadeToss();
            usedAbility2 = true;
        }
    }

    new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    /// <summary>
    /// Used by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public new void takeDamage(float damage)
    {
        base.takeDamage(damage);
    }

    new private void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
    
}
