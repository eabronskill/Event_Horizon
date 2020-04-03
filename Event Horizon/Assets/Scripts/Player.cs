using UnityEngine;
using Rewired;

public class Player : MonoBehaviour
{
    // Rewired Vars
    public Rewired.Player player; // Needs to be set by the Input script

    // Stat vars
    public float curMoveSpeed;
    public float movementSpeed;
    public float curHealth;
    public float maxHealth;
    public float curClip;
    public float maxClip;
    public float curAmmo;
    public float maxAmmo;
    private float dif;
    public bool dead = false;
    //animation for player
    public Animator playerAnimator;

    // Aiming vars
    private Plane mousePlane;
    private Ray cameraRay;
    private float intersectionDistance = 0f;

    // Item vars
    [HideInInspector]
    public Ammo ammoItem;
    [HideInInspector]
    public Healing healingItem;
    [HideInInspector]
    public bool hasAmmo = false;
    [HideInInspector]
    public bool hasHealing = false;
    private float bufferTimer = 0.5f;
    private bool cantUse = false;

    public bool testing = false;
    
    public void Update()
    {
        if (!testing)
        {
            // Movement
            Vector3 movementVec = new Vector3(player.GetAxis("Move Horizontal"), 0f, player.GetAxis("Move Vertical"));
            GetComponent<Rigidbody>().AddForce(movementVec * curMoveSpeed);

            // Rotation
            Vector3 rotateVec = new Vector3(0, Mathf.Atan2(player.GetAxis("Rotate Horizontal"), player.GetAxis("Rotate Vertical")) * 180 / Mathf.PI, 0);
            if (player.GetAxis("Rotate Horizontal") != 0 || player.GetAxis("Rotate Vertical") != 0)
            {
                transform.eulerAngles = rotateVec;
            }

            // Reloading
            if (curClip != maxClip && player.GetButtonDown("Reload"))
            {
                if (maxClip < curAmmo)
                {
                    dif = maxClip - curClip;
                    curClip = maxClip;
                }
                else
                {
                    dif = curAmmo - curClip;
                    curClip = curAmmo;
                }

                curAmmo -= dif;
            }

            // Items
            if (hasAmmo && player.GetButtonDown("Interact"))
            {
                ammoItem.use();
                hasAmmo = false;
            }
            else if (hasAmmo && player.GetButtonDown("DropItem"))
            {
                ammoItem.gameObject.SetActive(true);
                ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                ammoItem.player = null;
                ammoItem = null;
                hasAmmo = false;
            }
            if (hasHealing && player.GetButtonDown("Interact"))
            {
                healingItem.use();
                hasHealing = false;
            }
            else if (hasHealing && player.GetButtonDown("DropItem"))
            {
                healingItem.gameObject.SetActive(true);
                healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                healingItem.player = null;
                healingItem = null;
                hasHealing = false;
            }

            // Death
            if (curHealth <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
        else //FOR TESTING PURPOSES. ALLOWS USE OF KEYBOARD AND MOUSE. 
        {
            // Movement
            Vector3 movementVec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            GetComponent<Rigidbody>().AddForce(movementVec * curMoveSpeed);

            if ((Input.GetAxis("Horizontal") >= 0.3 && Input.GetAxis("Horizontal") <= -0.3) || (Input.GetAxis("Vertical") >= 0.3 && Input.GetAxis("Vertical") <= -0.3))
            {
                playerAnimator.SetBool("Running", true);
                print("running true");
               // playerAnimator.SetBool("Idle", false);
            }
            else
            {
                playerAnimator.SetBool("Running", false);
                print("running false");
               // playerAnimator.SetBool("Idle", true);
            }

         


            // Aiming 
            cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePlane.SetNormalAndPosition(Vector3.up, transform.position);
            if (mousePlane.Raycast(cameraRay, out intersectionDistance))
            {
                Vector3 hitPoint = cameraRay.GetPoint(intersectionDistance);
                transform.LookAt(hitPoint);
            }

            // Reloading
            if (curClip != maxClip && Input.GetKeyDown(KeyCode.R))
            {
                print("reloading");
                float dif;
                if (maxClip < curAmmo)
                {
                    dif = maxClip - curClip;
                    curClip = maxClip;
                }
                else
                {
                    dif = curAmmo - curClip;
                    curClip = curAmmo;
                }

                curAmmo -= dif;
            }

            if (!cantUse)
            {
                // Items
                if (hasAmmo && Input.GetKeyDown(KeyCode.E))
                {
                    ammoItem.use();
                    hasAmmo = false;
                }
                else if (hasAmmo && Input.GetKeyDown(KeyCode.F))
                {
                    ammoItem.gameObject.SetActive(true);
                    ammoItem.transform.position = new Vector3(transform.position.x, transform.localPosition.y + 1f, transform.position.z);
                    ammoItem.player = null;
                    ammoItem = null;
                    hasAmmo = false;
                }
                if (hasHealing && Input.GetKeyDown(KeyCode.E))
                {
                    healingItem.use();
                    hasHealing = false;
                }
                else if (hasHealing && Input.GetKeyDown(KeyCode.F))
                {
                    healingItem.gameObject.SetActive(true);
                    healingItem.transform.position = new Vector3(transform.position.x, transform.localPosition.y + 1f, transform.position.z);
                    healingItem.player = null;
                    healingItem = null;
                    hasHealing = false;
                }
            }
            

            // Death
            if (curHealth <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (bufferTimer < Time.time)
        {
            if (other.tag == "Ammo")
            {
                cantUse = true;
                if (player.GetButton("Interact") && !(hasAmmo || hasHealing))
                {
                    ammoItem = other.GetComponent<Ammo>();
                    ammoItem.player = this;
                    ammoItem.gameObject.SetActive(false);
                    hasAmmo = true;
                    bufferTimer = Time.time + 1f;
                    cantUse = false;
                }
                else if (player.GetButton("Interact") && hasHealing)
                {
                    healingItem.gameObject.SetActive(true);
                    healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                    healingItem.player = null;
                    healingItem = null;
                    hasHealing = false;


                    ammoItem = other.GetComponent<Ammo>();
                    ammoItem.player = this;
                    ammoItem.gameObject.SetActive(false);
                    hasAmmo = true;
                    bufferTimer = Time.time + 1f;
                    cantUse = false;
                }

            }
            if (other.tag == "Healing")
            {
                cantUse = true;
                if (player.GetButton("Interact") && !(hasAmmo || hasHealing))
                {
                    healingItem = other.GetComponent<Healing>();
                    healingItem.player = this;
                    healingItem.gameObject.SetActive(false);
                    hasHealing = true;
                    bufferTimer = Time.time + 1f;
                    cantUse = false;
                }
                else if (player.GetButton("Interact") && hasAmmo)
                {
                    ammoItem.gameObject.SetActive(true);
                    ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                    ammoItem.player = null;
                    ammoItem = null;
                    hasAmmo = false;

                    healingItem = other.GetComponent<Healing>();
                    healingItem.player = this;
                    healingItem.gameObject.SetActive(false);
                    hasHealing = true;
                    bufferTimer = Time.time + 1f;
                    cantUse = false;
                }
            }
        }
    }

    /// <summary>
    /// Used by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(float damage)
    {
        curHealth = curHealth - damage;
        if(curHealth < 0)
        {
            curHealth = 0;
            dead = true;
        }
    }

    public void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy Bullet")
        {
            takeDamage(30);
            print("Bullet Damage");
        }
    }
}
