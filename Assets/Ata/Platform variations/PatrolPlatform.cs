using UnityEngine;
using System.Collections.Generic;

public class PatrolPlatform : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private bool loopPath = true;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 60f;

    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool movingForward = true;

    void Start()
    {
        if (patrolPoints.Count > 0 && patrolPoints[0] != null)
        {
            transform.position = patrolPoints[0].position;
        }
    }

    void Update()
    {
        if (patrolPoints.Count < 2) return;

        // Platform dönüþü
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Bekleme durumu
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SelectNextPoint();
            }
            return;
        }

        // Hedefe doðru hareket
        if (patrolPoints[currentPointIndex] != null)
        {
            Vector3 targetPosition = patrolPoints[currentPointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedefe ulaþtýk mý?
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
        }
    }

    void SelectNextPoint()
    {
        if (loopPath)
        {
            // Döngüsel hareket
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        }
        else
        {
            // Ýleri-geri hareket
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= patrolPoints.Count)
                {
                    currentPointIndex = patrolPoints.Count - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    movingForward = true;
                }
            }
        }
    }

    // Editor'da path'i görselleþtir
    void OnDrawGizmos()
    {
        if (patrolPoints.Count < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            if (patrolPoints[i] != null && patrolPoints[i + 1] != null)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
        }

        if (loopPath && patrolPoints[0] != null && patrolPoints[patrolPoints.Count - 1] != null)
        {
            Gizmos.DrawLine(patrolPoints[patrolPoints.Count - 1].position, patrolPoints[0].position);
        }
    }
}