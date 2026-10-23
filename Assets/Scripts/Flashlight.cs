using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private Key toggleKey = Key.F;
    [SerializeField] private bool startOn = true;

    [Header("Cone Check")]
    [Range(0.5f, 1f)]
    [SerializeField] private float coneUsage = 0.9f;
    [SerializeField] private LayerMask occluders;
    [SerializeField] private float occlusionMargin = 0.25f;

    private Light spot;

    public bool IsOn => spot != null && spot.enabled;

    private void Awake()
    {
        spot = GetComponent<Light>();
        spot.enabled = startOn;
    }

    private void Update()
    {
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            spot.enabled = !spot.enabled;
        }
    }

    public bool Illuminates(Vector3 point)
    {
        if (!IsOn) return false;

        Vector3 origin = transform.position;
        Vector3 toPoint = point - origin;
        float distance = toPoint.magnitude;

        if (distance > spot.range) return false;
        if (distance < 0.01f) return true;

        float angle = Vector3.Angle(transform.forward, toPoint);
        if (angle > spot.spotAngle * 0.5f * coneUsage) return false;

        float checkDistance = Mathf.Max(0f, distance - occlusionMargin);
        if (Physics.Raycast(origin, toPoint / distance, checkDistance, occluders, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Light l = GetComponent<Light>();
        if (l == null) return;

        Gizmos.color = Color.yellow;
        float half = l.spotAngle * 0.5f * coneUsage;
        Vector3 left = Quaternion.AngleAxis(-half, transform.up) * transform.forward;
        Vector3 right = Quaternion.AngleAxis(half, transform.up) * transform.forward;
        Vector3 up = Quaternion.AngleAxis(-half, transform.right) * transform.forward;
        Vector3 down = Quaternion.AngleAxis(half, transform.right) * transform.forward;
        Gizmos.DrawRay(transform.position, left * l.range);
        Gizmos.DrawRay(transform.position, right * l.range);
        Gizmos.DrawRay(transform.position, up * l.range);
        Gizmos.DrawRay(transform.position, down * l.range);
    }
}