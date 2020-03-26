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
    private float bufferTimer = 0f;

    public bool testing = false;

    public void Awake()
    {
        curAmmo = maxAmmo;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;
        print("Awake");
    }

    void Start()
    {
        curAmmo = maxAmmo;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;
    }

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
                ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
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
                healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
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

            // Items
            if (hasAmmo && Input.GetKeyDown(KeyCode.E))
            {
                ammoItem.use();
                hasAmmo = false;
            }
            else if (hasAmmo && Input.GetKeyDown(KeyCode.F))
            {
                ammoItem.gameObject.SetActive(true);
                ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
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
                healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
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
    }

    public void OnTriggerStay(Collider other)
    {
        if (bufferTimer < Time.time)
        {
            if (other.tag == "Ammo")
            {
                if (player.GetButtonDown("Interact") && !(hasAmmo || hasHealing))
                {
                    ammoItem = other.GetComponent<Ammo>();
                    ammoItem.player = this;
                    ammoItem.gameObject.SetActive(false);
                    hasAmmo = true;
                    bufferTimer = Time.time + 1f;
                }
                else if (player.GetButtonDown("Interact") && hasHealing)
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
                }

            }
            if (other.tag == "Healing")
            {
                if (player.GetButtonDown("Interact") && !(hasAmmo || hasHealing))
                {
                    healingItem = other.GetComponent<Healing>();
                    healingItem.player = this;
                    healingItem.gameObject.SetActive(false);
                    hasHealing = true;
                    bufferTimer = Time.time + 1f;
                }
                else if (player.GetButtonDown("Interact") && hasAmmo)
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
                }
            }
        }
    }
}
