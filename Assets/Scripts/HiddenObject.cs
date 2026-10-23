using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private float hideDelay = 0.2f;
    [SerializeField] private Vector3 aimOffset = Vector3.zero;

    private Renderer[] renderers;
    private float lastLitTime = -999f;
    private bool consumed;

    public bool IsRevealed { get; private set; }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        if (flashlight == null) flashlight = FindAnyObjectByType<Flashlight>();
        SetVisible(false);
    }

    private void Update()
    {
        if (consumed || flashlight == null) return;

        if (flashlight.Illuminates(transform.TransformPoint(aimOffset)))
        {
            lastLitTime = Time.time;
        }

        bool shouldShow = Time.time - lastLitTime <= hideDelay;
        if (shouldShow != IsRevealed) SetVisible(shouldShow);
    }

    public void Consume()
    {
        consumed = true;
        SetVisible(false);
    }

    public void Restore()
    {
        consumed = false;
    }

    private void SetVisible(bool visible)
    {
        IsRevealed = visible;
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.TransformPoint(aimOffset), 0.2f);
    }
}