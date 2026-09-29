using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField] private float fallLimit = -3f;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < fallLimit) { 

            if(TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
            }
            transform.position = startPosition;
        }
    }
}
