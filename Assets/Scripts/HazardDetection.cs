using System.Diagnostics;
using UnityEngine;

public class HazardDetection : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask sightBlockers;

    [Header("Hearing (CheckSphere)")]
    [SerializeField] private float hearingRadius = 4f;
    [SerializeField] private float hearingGain = 0.4f;
    [SerializeField] private float minNoiseSpeed = 0.5f;

    [Header("Front Zone (CheckBox)")]
    [SerializeField] private Vector3 frontBoxHalfExtents = new Vector3(0.8f, 1f, 1f);
    [SerializeField] private float frontBoxOffset = 1.5f;
    [SerializeField] private float frontZoneGain = 1.2f;

    [Header("Body Contact (CheckCapsule)")]
    [SerializeField] private float bodyRadius = 1.1f;
    [SerializeField] private float bodyHalfLength = 0.5f;

    [Header("Sight (Raycast)")]
    [SerializeField] private float eyeHeight = 0.6f;
    [SerializeField] private float viewDistance = 12f;
    [SerializeField] private float viewAngle = 70f;
    [SerializeField] private float sightGainClose = 2f;
    [SerializeField] private float sightGainFar = 0.8f;

    [Header("Alert Meter")]
    [SerializeField] private float catchThreshold = 0.95f;
    [SerializeField] private float cooldownRate = 0.15f;
    [SerializeField] private Color calmColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color alertColor = Color.red;

    [Header("Debug")]
    [SerializeField] private bool logAlert = false;

    public float Alert { get; private set; }

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    private PlayerHealth playerHealth;
    private Rigidbody playerBody;
    private Collider playerCollider;
    private Renderer guardRenderer;

    private void Awake()
    {
        if (player == null) player = FindAnyObjectByType<PlayerMovement>();
        guardRenderer = GetComponent<Renderer>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerBody = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponentInChildren<Collider>();
    }

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnPlayerDied += ResetAlert;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnPlayerDied -= ResetAlert;
    }

    private void Update()
    {
        float gain = 0f;

        Vector3 capStart = transform.position + Vector3.down * bodyHalfLength;
        Vector3 capEnd = transform.position + Vector3.up * bodyHalfLength;
        if (Physics.CheckCapsule(capStart, capEnd, bodyRadius, playerMask, QueryTriggerInteraction.Ignore))
        {
            Alert = 1f;
        }

        // Hearing: player must be inside the radius AND moving fast enough to make noise
        if (Physics.CheckSphere(transform.position, hearingRadius, playerMask, QueryTriggerInteraction.Ignore)
            && HorizontalSpeed() > minNoiseSpeed)
        {
            gain += hearingGain;
        }

        Vector3 boxCenter = transform.position + transform.forward * frontBoxOffset;
        if (Physics.CheckBox(boxCenter, frontBoxHalfExtents, transform.rotation, playerMask, QueryTriggerInteraction.Ignore))
        {
            gain += frontZoneGain;
        }

        if (CanSeePlayer(out float distance01))
        {
            gain += Mathf.Lerp(sightGainClose, sightGainFar, distance01);
        }

        if (gain > 0f) Alert += gain * Time.deltaTime;
        else Alert -= cooldownRate * Time.deltaTime;
        Alert = Mathf.Clamp01(Alert);

        if (logAlert && Alert > 0.5f) UnityEngine.Debug.Log($"{name} alert = {Alert:F2}");

        UpdateVisual();

        if (Alert >= catchThreshold)
        {
            playerHealth.TakeDamage(100);
            UnityEngine.Debug.Log("You got caught :(");
        }
    }

    private bool CanSeePlayer(out float distance01)
    {
        distance01 = 1f;

        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 target = playerCollider.bounds.center;
        Vector3 toPlayer = target - eye;
        float distance = toPlayer.magnitude;

        if (distance > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(eye, toPlayer.normalized, out RaycastHit hit, viewDistance,
                            sightBlockers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.transform.IsChildOf(player.transform))
            {
                distance01 = distance / viewDistance;
                return true;
            }
        }
        return false;
    }

    private float HorizontalSpeed()
    {
        Vector3 v = playerBody.linearVelocity;
        return new Vector2(v.x, v.z).magnitude;
    }

    private void ResetAlert()
    {
        Alert = 0f;
    }

    private void UpdateVisual()
    {
        if (guardRenderer == null) return;

        float t = Mathf.Clamp01(Alert / catchThreshold);
        guardRenderer.material.SetColor(BaseColorId, Color.Lerp(calmColor, alertColor, t));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        Gizmos.color = Color.magenta;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * frontBoxOffset,
                                      transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, frontBoxHalfExtents * 2f);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * bodyHalfLength, bodyRadius);
        Gizmos.DrawWireSphere(transform.position + Vector3.down * bodyHalfLength, bodyRadius);

        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 left = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(eye, eye + left * viewDistance);
        Gizmos.DrawLine(eye, eye + right * viewDistance);
    }
}