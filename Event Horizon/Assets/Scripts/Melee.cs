using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject attackPoint;

    Vector3 fromPosition;
    Vector3 toPosition;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tankMelee()
    {
        fromPosition = attackPoint.transform.position;
        toPosition = Input.mousePosition;
        direction = toPosition - fromPosition;
    }

    public void soldierMelee()
    {

    }
}
