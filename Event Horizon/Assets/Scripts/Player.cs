using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

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

    private CharacterController controller;

    // Canvas Vars
    public GameObject gameOverCanvas;
    public GameObject pauseMenu;
    public GameObject victoryCanvas;
    private static bool paused = false;
    private bool gameOver = false;
    private bool levelOne = true;

    public void setPaused(bool b)
    {
        paused = b;
    }

    public bool getPaused()
    {
        return paused;
    }
    

    public void Start()
    {
        
        controller = GetComponent<CharacterController>();
    }

    
    
    public void Update()
    {
        if (Time.timeScale == 1)
        {
            // Movement
            Vector3 movementVec = new Vector3(player.GetAxis("Move Horizontal"), 0f, player.GetAxis("Move Vertical"));
            GetComponent<Rigidbody>().AddForce(movementVec * curMoveSpeed * Time.deltaTime);

            // Rotation
            Vector3 rotateVec = new Vector3(0, Mathf.Atan2(player.GetAxis("Rotate Horizontal"), player.GetAxis("Rotate Vertical")) * 180 / Mathf.PI, 0);
            if (player.GetAxis("Rotate Horizontal") != 0 || player.GetAxis("Rotate Vertical") != 0)
            {
                transform.eulerAngles = rotateVec;

                // blending anims:
                // if (left l-stick)a - run left; within r-stick -75 to 14*
                //wasd
                // if (up l-stick)w - run forward; within r-stick -15 to 15*
                //wasd
                // if (right l-stick)d - run right; within r-stick 16 to 75*
                //wasd
                // if (down l-stick)s - backwards; within r-stick 76 - 180*
                //wasd

                //abilities stuff

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

        // HUD
        if (!gameOver)
        {
            if (player.GetButtonDown("Menu") && !pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);

                pauseMenu.GetComponent<PauseMenuController>().setPlayer(this.player);
            }
            else if (player.GetButtonDown("Menu") && pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                pauseMenu.GetComponent<PauseMenuController>().setPlayer(null);

            }
        }
        
        if (gameOverCanvas.activeSelf && player.controllers.joystickCount >= 1)
        {
            
            gameOver = true;
        }
        if (player.GetButtonDown("Interact") && gameOverCanvas.activeSelf && player.controllers.joystickCount >= 1)
        {
            
            MainMenuController.controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
            ChS_Model.idToCharacter = new System.Collections.Generic.Dictionary<int, ChS_Model.Character>();
            ChS_Controller.finalSelection = new System.Collections.Generic.Dictionary<string, int>();
            UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
            
        }
        if (player.GetButtonDown("Interact") && victoryCanvas.activeSelf && player.controllers.joystickCount >= 1)
        {
            Time.timeScale = 1;
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                
                levelOne = false;
                SceneManager.LoadScene("Level2");
            }
            else
            {
                UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
                MainMenuController.controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
                ChS_Model.idToCharacter = new System.Collections.Generic.Dictionary<int, ChS_Model.Character>();
                ChS_Controller.finalSelection = new System.Collections.Generic.Dictionary<string, int>();
                
                SceneManager.LoadScene("MainMenu");
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
