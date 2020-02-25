using UnityEngine;

public class Player : MonoBehaviour
{
    // Stat vars
    public float movementSpeed;
    public float health;
    public float armor;
    public float ammo;

    // Aiming vars
    private Plane mousePlane;
    private Ray cameraRay;
    private float intersectionDistance = 0f;
    
    public void FixedUpdate()
    {
        // Movement
        Vector3 movementVec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        GetComponent<Rigidbody>().AddForce(movementVec * movementSpeed);

        // Aiming 
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePlane.SetNormalAndPosition(Vector3.up, transform.position);
        if (mousePlane.Raycast(cameraRay, out intersectionDistance))
        {
            Vector3 hitPoint = cameraRay.GetPoint(intersectionDistance);
            transform.LookAt(hitPoint);
        }
    }
    
}
