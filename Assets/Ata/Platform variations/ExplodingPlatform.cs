using UnityEngine;
using System.Collections;

public class ExplodingPlatform : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private float explosionForce = 2000f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float respawnTime = 5f;

    [Header("Visual Settings")]
    [SerializeField] private Gradient heatingGradient;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private float maxShakeAmount = 0.3f;
    [SerializeField] private ParticleSystem explosionEffect;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 45f;

    private bool isActivated = false;
    private float currentFuseTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;
    private Collider2D platformCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    void Update()
    {
        // Normal dönüþ
        if (!isActivated)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // Patlama sayacý
        if (isActivated)
        {
            currentFuseTime += Time.deltaTime;
            float progress = currentFuseTime / fuseTime;

            // Renk deðiþimi
            if (spriteRenderer != null)
            {
                spriteRenderer.color = heatingGradient.Evaluate(progress);
            }

            // Titreme efekti
            float shakeAmount = shakeCurve.Evaluate(progress) * maxShakeAmount;
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;

            // Patlama zamaný
            if (currentFuseTime >= fuseTime)
            {
                Explode();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
        }
    }

    void Explode()
    {
        // Patlama efekti
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Çevredeki objelere kuvvet uygula
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                float forceMagnitude = Mathf.Lerp(explosionForce, 0, distance / explosionRadius);

                rb.AddForce(direction * forceMagnitude);
            }
        }

        // Platformu gizle
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        // Yeniden doðuþ
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        // Reset
        transform.position = originalPosition;
        spriteRenderer.enabled = true;
        platformCollider.enabled = true;
        spriteRenderer.color = Color.white;
        isActivated = false;
        currentFuseTime = 0f;
    }

    // Editor'da patlama alanýný göster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}