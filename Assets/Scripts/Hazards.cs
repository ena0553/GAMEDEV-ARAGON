
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hazards : MonoBehaviour
{
    [Header("Route")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool pingPong = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private float arriveDistance = 0.2f;

    private Rigidbody body;
    private int index = 1;
    private int direction = 1;
    private float waitTimer;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
    }

    private void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            UnityEngine.Debug.LogWarning($"{name}: needs at least 2 waypoints.");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector3 target = waypoints[index].position;
        target.y = body.position.y;

        Vector3 toTarget = target - body.position;

        if (toTarget.magnitude <= arriveDistance)
        {
            waitTimer = waitTime;
            AdvanceWaypoint();
            return;
        }

        Vector3 moveDir = toTarget.normalized;

        Quaternion lookRotation = Quaternion.LookRotation(moveDir);
        body.MoveRotation(Quaternion.RotateTowards(body.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime));

        if (Quaternion.Angle(body.rotation, lookRotation) < 30f)
        {
            body.MovePosition(body.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void AdvanceWaypoint()
    {
        if (pingPong)
        {
            index += direction;
            if (index >= waypoints.Length || index < 0)
            {
                direction = -direction;
                index += direction * 2;
            }
        }
        else
        {
            index = (index + 1) % waypoints.Length;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);
            int next = (i + 1) % waypoints.Length;
            if (waypoints[next] != null && (i < waypoints.Length - 1 || !pingPong))
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}