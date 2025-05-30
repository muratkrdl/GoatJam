using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Stretch Settings")]
    [SerializeField] private float maxArmLength = 2f;
    [SerializeField] private float armStretchSpeed = 10f;
    [SerializeField] private AnimationCurve stretchCurve;

    [Header("Visual Effects")]
    [SerializeField] private TrailRenderer leftHandTrail;
    [SerializeField] private TrailRenderer rightHandTrail;
    [SerializeField] private ParticleSystem grabParticles;

    private PlayerController playerController;
    private bool isStretching = false;
    private Vector3 targetHandPosition;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        // Trail renderer'lar� ba�lang��ta kapat
        if (leftHandTrail != null) leftHandTrail.enabled = false;
        if (rightHandTrail != null) rightHandTrail.enabled = false;
    }

    void Update()
    {
        UpdateArmStretching();
        UpdateVisualEffects();
    }

    void UpdateArmStretching()
    {
        if (Input.GetMouseButton(0) && !playerController.IsGrabbing())
        {
            // Mouse pozisyonuna do�ru kollar� uzat
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            // En yak�n eli belirle
            float leftDistance = Vector3.Distance(leftHand.position, mouseWorldPos);
            float rightDistance = Vector3.Distance(rightHand.position, mouseWorldPos);

            Transform closerHand = leftDistance < rightDistance ? leftHand : rightHand;
            Transform closerArm = leftDistance < rightDistance ? leftArm : rightArm;

            // Kolu uzat
            StretchArm(closerArm, closerHand, mouseWorldPos);
            isStretching = true;
        }
        else if (isStretching)
        {
            // Kollar� normale d�nd�r
            ResetArms();
            isStretching = false;
        }
    }

    void StretchArm(Transform arm, Transform hand, Vector3 targetPos)
    {
        // Kol uzunlu�unu hesapla
        Vector3 direction = (targetPos - arm.position).normalized;
        float distance = Vector3.Distance(arm.position, targetPos);
        distance = Mathf.Min(distance, maxArmLength);

        // Kolu hedefe do�ru d�nd�r
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arm.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Kolu uzat (scale ile)
        float currentLength = arm.localScale.y;
        float targetLength = distance / arm.GetComponent<SpriteRenderer>().size.y;
        arm.localScale = new Vector3(arm.localScale.x,
                                     Mathf.Lerp(currentLength, targetLength, armStretchSpeed * Time.deltaTime),
                                     arm.localScale.z);

        // El pozisyonunu g�ncelle
        hand.position = arm.position + direction * distance;
    }

    void ResetArms()
    {
        // Kollar� varsay�lan pozisyona d�nd�r
        if (leftArm != null)
        {
            leftArm.localScale = Vector3.Lerp(leftArm.localScale, Vector3.one, armStretchSpeed * Time.deltaTime);
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, Quaternion.identity, armStretchSpeed * Time.deltaTime);
        }

        if (rightArm != null)
        {
            rightArm.localScale = Vector3.Lerp(rightArm.localScale, Vector3.one, armStretchSpeed * Time.deltaTime);
            rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, Quaternion.identity, armStretchSpeed * Time.deltaTime);
        }
    }

    void UpdateVisualEffects()
    {
        // Trail efektleri
        bool isMovingFast = GetComponent<Rigidbody2D>().linearVelocity.magnitude > 10f;

        if (leftHandTrail != null)
            leftHandTrail.enabled = isMovingFast;

        if (rightHandTrail != null)
            rightHandTrail.enabled = isMovingFast;

        // Grab particle efekti
        if (grabParticles != null && playerController.IsGrabbing())
        {
            if (!grabParticles.isPlaying)
                grabParticles.Play();
        }
        else if (grabParticles != null && grabParticles.isPlaying)
        {
            grabParticles.Stop();
        }
    }

    public void PlayGrabEffect(Vector3 position)
    {
        if (grabParticles != null)
        {
            grabParticles.transform.position = position;
            grabParticles.Play();
        }
    }
}