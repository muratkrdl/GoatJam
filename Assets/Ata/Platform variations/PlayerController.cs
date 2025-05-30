using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseJumpForce = 15f;
    [SerializeField] private float velocityMultiplier = 1.5f;
    [SerializeField] private float maxVelocity = 30f;
    [SerializeField] private float dragInAir = 0.5f;

    [Header("Grab Settings")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float handRadius = 0.3f;
    [SerializeField] private LayerMask grabbableLayer;

    [Header("Visual Settings")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 30;
    [SerializeField] private float trajectoryTimeStep = 0.1f;

    private Rigidbody2D rb;
    private Transform currentPlatform;
    private Vector3 platformOffset;
    private float currentAngularVelocity;
    private bool isGrabbing = false;
    private bool canGrab = true;

    // Platform bilgileri
    private Vector3 lastPlatformPosition;
    private float lastPlatformRotation;
    private float platformRotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Yer �ekimi yok
        rb.linearDamping = dragInAir;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    void Update()
    {
        // Mouse input kontrol�
        if (Input.GetMouseButton(0) && canGrab && !isGrabbing)
        {
            TryGrab();
        }
        else if (Input.GetMouseButtonUp(0) && isGrabbing)
        {
            Release();
        }

        // Trajectory g�sterimi
        if (isGrabbing && trajectoryLine != null)
        {
            ShowTrajectory();
        }
    }

    void FixedUpdate()
    {
        if (isGrabbing && currentPlatform != null)
        {
            // Platform rotasyon h�z�n� hesapla
            float deltaRotation = currentPlatform.eulerAngles.z - lastPlatformRotation;
            if (deltaRotation > 180) deltaRotation -= 360;
            if (deltaRotation < -180) deltaRotation += 360;

            platformRotationSpeed = deltaRotation / Time.fixedDeltaTime;
            lastPlatformRotation = currentPlatform.eulerAngles.z;

            // Player'� platform ile d�nd�r
            RotateWithPlatform();
        }
    }

    void TryGrab()
    {
        // Sol el kontrol�
        Collider2D leftGrab = Physics2D.OverlapCircle(leftHand.position, handRadius, grabbableLayer);
        Collider2D rightGrab = Physics2D.OverlapCircle(rightHand.position, handRadius, grabbableLayer);

        Collider2D grabbed = leftGrab ?? rightGrab;

        if (grabbed != null)
        {
            // Tutunabilir mi kontrol et
            IGrabbable grabbable = grabbed.GetComponent<IGrabbable>();
            if (grabbable != null && grabbable.CanGrab())
            {
                AttachToPlatform(grabbed.transform);
            }
        }
    }

    void AttachToPlatform(Transform platform)
    {
        isGrabbing = true;
        currentPlatform = platform;

        // Velocity'yi s�f�rla
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Platform'a g�re offset hesapla
        platformOffset = platform.InverseTransformPoint(transform.position);

        // Platform rotasyon takibi i�in ba�lang�� de�erleri
        lastPlatformPosition = platform.position;
        lastPlatformRotation = platform.eulerAngles.z;

        // Ses efekti
        SoundManager.Instance?.PlayAttach();
    }

    void RotateWithPlatform()
    {
        if (currentPlatform == null) return;

        // Yeni pozisyonu hesapla
        Vector3 rotatedOffset = currentPlatform.TransformPoint(platformOffset);
        transform.position = rotatedOffset;

        // Player rotasyonunu platform ile senkronize et (opsiyonel)
        // transform.rotation = currentPlatform.rotation;
    }

    void Release()
    {
        if (!isGrabbing || currentPlatform == null) return;

        isGrabbing = false;

        // F�rlatma y�n�n� hesapla
        Vector2 releaseDirection = (transform.position - currentPlatform.position).normalized;

        // Platform'un d�n�� h�z�na g�re te�etsel h�z
        float radius = Vector2.Distance(transform.position, currentPlatform.position);
        float tangentialSpeed = Mathf.Abs(platformRotationSpeed) * Mathf.Deg2Rad * radius;

        // Te�et y�n� hesapla (d�n�� y�n�ne g�re)
        Vector2 tangentDirection = new Vector2(-releaseDirection.y, releaseDirection.x);
        if (platformRotationSpeed < 0) tangentDirection *= -1;

        // Toplam f�rlatma kuvveti
        Vector2 launchVelocity = releaseDirection * baseJumpForce;
        launchVelocity += tangentDirection * tangentialSpeed * velocityMultiplier;

        // H�z limiti
        if (launchVelocity.magnitude > maxVelocity)
        {
            launchVelocity = launchVelocity.normalized * maxVelocity;
        }

        // Kuvvet uygula
        rb.linearVelocity = launchVelocity;

        // Ses efekti
        SoundManager.Instance?.PlayDetach();

        // Trajectory'yi gizle
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }

        currentPlatform = null;

        // K�sa bir s�re tekrar tutunmay� engelle
        canGrab = false;
        Invoke(nameof(EnableGrab), 0.2f);
    }

    void EnableGrab()
    {
        canGrab = true;
    }

    void ShowTrajectory()
    {
        if (currentPlatform == null) return;

        trajectoryLine.enabled = true;
        trajectoryLine.positionCount = trajectoryPoints;

        // F�rlatma parametrelerini hesapla
        Vector2 startPos = transform.position;
        Vector2 releaseDirection = (transform.position - currentPlatform.position).normalized;

        float radius = Vector2.Distance(transform.position, currentPlatform.position);
        float tangentialSpeed = Mathf.Abs(platformRotationSpeed) * Mathf.Deg2Rad * radius;

        Vector2 tangentDirection = new Vector2(-releaseDirection.y, releaseDirection.x);
        if (platformRotationSpeed < 0) tangentDirection *= -1;

        Vector2 launchVelocity = releaseDirection * baseJumpForce;
        launchVelocity += tangentDirection * tangentialSpeed * velocityMultiplier;

        if (launchVelocity.magnitude > maxVelocity)
        {
            launchVelocity = launchVelocity.normalized * maxVelocity;
        }

        // Trajectory noktalar�n� hesapla
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * trajectoryTimeStep;
            Vector2 point = startPos + launchVelocity * time;

            // Drag etkisi (basitle�tirilmi�)
            point += -launchVelocity.normalized * (0.5f * dragInAir * time * time);

            trajectoryLine.SetPosition(i, point);
        }
    }

    // Collision detections
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Platform �arpmas� sesi
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            SoundManager.Instance?.PlayLand();
        }
    }


    public bool IsGrabbing()
    {
        return isGrabbing;
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        // El pozisyonlar�n� g�ster
        if (leftHand != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(leftHand.position, handRadius);
        }

        if (rightHand != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(rightHand.position, handRadius);
        }

        // Platform ba�lant�s�n� g�ster
        if (isGrabbing && currentPlatform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentPlatform.position);
        }
    }
}