using UnityEngine;

public class BasicRotatingPlatform : MonoBehaviour, IGrabbable
{
    [Header("Rotation Settings")]
    [SerializeField] private float minRotationSpeed = 30f;
    [SerializeField] private float maxRotationSpeed = 120f;
    [SerializeField] private float speedChangeRate = 0.5f;

    private float currentRotationSpeed;
    private float targetRotationSpeed;

    [Header("Platform Settings")]
    [SerializeField] private bool canPlayerGrab = true;

    void Start()
    {
        // Ba�lang�� h�z�n� rastgele belirle
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        targetRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    void Update()
    {
        // H�z de�i�imi
        currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, targetRotationSpeed, speedChangeRate * Time.deltaTime);

        // Hedef h�za ula�t�ysa yeni hedef belirle
        if (Mathf.Approximately(currentRotationSpeed, targetRotationSpeed))
        {
            targetRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        }

        // Platformu d�nd�r
        transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
    }

    public bool CanPlayerGrab()
    {
        return canPlayerGrab;
    }

    // IGrabbable implementation
    public bool CanGrab()
    {
        return canPlayerGrab;
    }

    public float GetRotationSpeed()
    {
        return currentRotationSpeed;
    }
}