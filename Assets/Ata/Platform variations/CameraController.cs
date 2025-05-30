using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float lookAheadDistance = 3f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    [Header("Camera Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minY = -50f;
    [SerializeField] private float maxY = 50f;

    [Header("Zoom Settings")]
    [SerializeField] private float baseZoom = 5f;
    [SerializeField] private float velocityZoomMultiplier = 0.1f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDecay = 1f;

    private Camera cam;
    private Rigidbody2D targetRb;
    private float shakeIntensity = 0f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
            transform.position = target.position + offset;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        FollowTarget();
        UpdateZoom();
        UpdateShake();
    }

    void FollowTarget()
    {
        // Look ahead based on velocity
        Vector3 lookAhead = Vector3.zero;
        if (targetRb != null)
        {
            lookAhead = targetRb.linearVelocity.normalized * lookAheadDistance;
        }

        // Target position
        Vector3 desiredPosition = target.position + offset + lookAhead;

        // Smooth follow
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / followSpeed);

        // Apply bounds
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        transform.position = smoothedPosition;
    }

    void UpdateZoom()
    {
        if (targetRb == null || cam == null) return;

        // Velocity bazl� zoom
        float velocityMagnitude = targetRb.linearVelocity.magnitude;
        float targetZoom = baseZoom + (velocityMagnitude * velocityZoomMultiplier);
        targetZoom = Mathf.Clamp(targetZoom, baseZoom, maxZoom);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    void UpdateShake()
    {
        if (shakeIntensity > 0)
        {
            transform.position += Random.insideUnitSphere * shakeIntensity;
            shakeIntensity -= shakeDecay * Time.deltaTime;

            if (shakeIntensity <= 0)
                shakeIntensity = 0;
        }
    }

    public void ShakeCamera(float intensity)
    {
        shakeIntensity = intensity;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    // Editor'da bounds'lar� g�ster
    void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.yellow;
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}