using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 2f;
    private Vector3 movementVec;
    public ParticleSystem gunFlash;
    public ParticleSystem bloodEffect;
    public bool isEnemy = false;
    public float die = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        movementVec = transform.forward * speed;
        Invoke("destroyProjectile", die);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + movementVec);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (this.gameObject.tag == "Enemy Bullet")
        {
            if (coll.collider.tag == "Enemy")
            {
                return;
            }
            else
            {
                destroyProjectile();
            }
        }
        else
        {
            if (coll.collider.tag == "Enemy")
            {
                if (gunFlash != null && !isEnemy)
                {
                    bloodEffect = Instantiate(gunFlash, transform.position, transform.rotation);
                    bloodEffect.Play();
                }
                destroyProjectile();
            }
            else if (coll.collider.tag == "Player")
            {
                return;
            }
            else
            {
                destroyProjectile();
            }

        }
    }

    public void destroyProjectile()
    {
        Destroy(this.gameObject);
    }

}
