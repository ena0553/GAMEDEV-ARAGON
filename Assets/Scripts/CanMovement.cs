using UnityEngine;

public class CanMovement : MonoBehaviour
{
    [SerializeField] float distance = 3f;
    [SerializeField] float speed = 3f;
    [SerializeField] float offset = 0.2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Sin(Time.time * speed + offset) * distance;
        transform.position = startPos + new Vector3(x, 0, 0);
    }
}
