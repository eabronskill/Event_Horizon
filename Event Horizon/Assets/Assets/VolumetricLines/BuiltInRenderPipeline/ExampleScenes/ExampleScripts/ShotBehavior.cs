using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

    public Vector3 target;
    public GameObject collisionExplosion;
    public float speed;
    
	// Update is called once per frame
	void FixedUpdate ()
    {
        float step = speed * Time.deltaTime;

        if (target != null)
        {
            //if (transform.position == target)
            //{
            //    explode();
            //    return;
            //}
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
	}

    public void setTarget(Vector3 target)
    {
        this.target = target;
    }

    void explode()
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }

    void OnCollision(Collider other)
    {

    }
}
