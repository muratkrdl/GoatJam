using UnityEngine;

public class AcceleratingPlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float initialRotationSpeed = 30f;
    [SerializeField] private float maxRotationSpeed = 500f;
    [SerializeField] private float accelerationRate = 50f;
    [SerializeField] private float decelerationRate = 100f;

    [Header("Visual Effects")]
    [SerializeField] private Color maxSpeedColor = Color.red;
    [SerializeField] private ParticleSystem speedParticles;

    private float currentRotationSpeed;
    private bool playerAttached = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentRotationSpeed = initialRotationSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // Hýz kontrolü
        if (playerAttached)
        {
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, maxRotationSpeed, accelerationRate * Time.deltaTime);
        }
        else
        {
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, initialRotationSpeed, decelerationRate * Time.deltaTime);
        }

        // Platformu döndür
        transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        // Görsel efektler
        UpdateVisualEffects();
    }

    void UpdateVisualEffects()
    {
        if (spriteRenderer != null)
        {
            float speedRatio = (currentRotationSpeed - initialRotationSpeed) / (maxRotationSpeed - initialRotationSpeed);
            spriteRenderer.color = Color.Lerp(originalColor, maxSpeedColor, speedRatio);
        }

        if (speedParticles != null)
        {
            var emission = speedParticles.emission;
            emission.rateOverTime = Mathf.Lerp(0, 50, (currentRotationSpeed - initialRotationSpeed) / (maxRotationSpeed - initialRotationSpeed));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerAttached = true;

            if (speedParticles != null && !speedParticles.isPlaying)
            {
                speedParticles.Play();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerAttached = false;

            if (speedParticles != null && speedParticles.isPlaying)
            {
                speedParticles.Stop();
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return currentRotationSpeed;
    }
}