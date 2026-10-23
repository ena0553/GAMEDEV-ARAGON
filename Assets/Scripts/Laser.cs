using UnityEngine;

public class Laser : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float speed = 4f;

    private Vector3 target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        GetComponent<Collider>().isTrigger = true;
    }

    public void Init(Vector3 targetPosition, float randomHeight)
    {
        target = new Vector3(targetPosition.x, randomHeight, targetPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if(health != null)
            {
                health.TakeDamage(20);
            }
            
        }
    }
}
