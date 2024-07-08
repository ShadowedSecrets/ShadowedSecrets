using UnityEngine;

public class GuardianEye : MonoBehaviour
{
    public Transform target;
    public float speed = 5f; 
    public float rotateSpeed = 200f;
    public float damage = 10f;

    void Start()
    {
       
    }

    void Update()
    {
        if (target == null)
        {
            
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.forward).y;
        transform.Rotate(0, rotateAmount * rotateSpeed * Time.deltaTime, 0);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            
            Debug.Log("Missile hit the target");
            
            Destroy(gameObject);
        }
    }

    public void Initialize(Transform targetTransform, float missileSpeed)
    {
        target = targetTransform;
        speed = missileSpeed;
    }
}

