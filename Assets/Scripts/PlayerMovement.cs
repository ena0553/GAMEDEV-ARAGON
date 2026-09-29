using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 8, rb.linearVelocity.z);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector3(horizontalInput * 5, rb.linearVelocity.y, verticalInput * 5);

        /*
        if (Input.GetKey("w")){
            rb.linearVelocity = new Vector3(-3, 0, 0);
        }

        if (Input.GetKey("s")){
            rb.linearVelocity = new Vector3(3, 0, 0);
        }

        if (Input.GetKey("d")){
            rb.linearVelocity = new Vector3(0, 0, 3);
        }

        if (Input.GetKey("a")){
            rb.linearVelocity = new Vector3(0, 0, -3);
        }
        */
    }
    
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }

}
