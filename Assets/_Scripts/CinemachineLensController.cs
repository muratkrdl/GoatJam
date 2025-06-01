using UnityEngine;
using Unity.Cinemachine;

namespace _Scripts.Controllers
{
    public class CinemachineLensController : MonoBehaviour
    {
        [Header("Cinemachine Settings")]
        [SerializeField] private CinemachineCamera virtualCamera;

        [Header("Player Reference")]
        [SerializeField] private Transform playerTransform;

        [Header("Lens Settings")]
        [SerializeField] private float minLensSize = 5f;
        [SerializeField] private float maxLensSize = 15f;

        [Header("Height Settings")]
        [SerializeField] private float minHeight = 0f;
        [SerializeField] private float maxHeight = 50f;

        [Header("Sensitivity")]
        [Range(0.1f, 5f)]
        [SerializeField] private float sensitivity = 1f;

        [Header("Smoothing")]
        [SerializeField] private bool useSmoothing = true;
        [Range(0.1f, 10f)]
        [SerializeField] private float smoothSpeed = 2f;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;

        private float currentTargetLensSize;
        private float initialPlayerHeight;
        private bool isInitialized = false;

        void Start()
        {
            InitializeComponent();
        }

        void Update()
        {
            if (!isInitialized) return;

            UpdateLensSize();

            if (showDebugInfo)
            {
                DebugInfo();
            }
        }

        void InitializeComponent()
        {
            // Virtual Camera otomatik bulma
            if (virtualCamera == null)
            {
                virtualCamera = FindFirstObjectByType<CinemachineCamera>();
                if (virtualCamera == null)
                {
                    Debug.LogError("CinemachineCamera bulunamad�!");
                    return;
                }
            }

            // Player otomatik bulma
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
                else
                {
                    Debug.LogError("Player Transform bulunamad�!");
                    return;
                }
            }

            // �lk player y�ksekli�ini kaydet
            if (playerTransform != null)
            {
                initialPlayerHeight = playerTransform.position.y;
                currentTargetLensSize = virtualCamera.Lens.OrthographicSize;
                isInitialized = true;

                Debug.Log($"CinemachineLensController initialized. Initial height: {initialPlayerHeight}");
            }
        }

        void UpdateLensSize()
        {
            if (playerTransform == null || virtualCamera == null) return;

            // Player'�n mevcut y�ksekli�i
            float currentHeight = playerTransform.position.y;

            // Ba�lang��tan itibaren y�kselme miktar�
            float heightDifference = (currentHeight - initialPlayerHeight) * sensitivity;

            // Height'i belirlenen aral��a s�n�rla
            float clampedHeight = Mathf.Clamp(heightDifference, 0f, maxHeight - minHeight);

            // Height'i lens size'a �evir
            float normalizedHeight = clampedHeight / (maxHeight - minHeight);
            currentTargetLensSize = Mathf.Lerp(minLensSize, maxLensSize, normalizedHeight);

            // Lens size'� uygula
            if (useSmoothing)
            {
                // Smooth ge�i�
                float currentLensSize = virtualCamera.Lens.OrthographicSize;
                float newLensSize = Mathf.Lerp(currentLensSize, currentTargetLensSize, smoothSpeed * Time.deltaTime);

                virtualCamera.Lens.OrthographicSize = newLensSize;
            }
            else
            {
                // Direkt de�i�im
                virtualCamera.Lens.OrthographicSize = currentTargetLensSize;
            }
        }

        void DebugInfo()
        {
            if (playerTransform == null) return;

            float currentHeight = playerTransform.position.y;
            float heightDiff = currentHeight - initialPlayerHeight;

            Debug.Log($"Player Height: {currentHeight:F2} | Height Diff: {heightDiff:F2} | Target Lens: {currentTargetLensSize:F2} | Current Lens: {virtualCamera.Lens.OrthographicSize:F2}");
        }

        // Public fonksiyonlar - di�er scriptlerden �a�r�labilir
        public void SetMinMaxLensSize(float min, float max)
        {
            minLensSize = min;
            maxLensSize = max;
        }

        public void SetMinMaxHeight(float min, float max)
        {
            minHeight = min;
            maxHeight = max;
        }

        public void SetSensitivity(float newSensitivity)
        {
            sensitivity = Mathf.Clamp(newSensitivity, 0.1f, 5f);
        }

        public void ResetInitialHeight()
        {
            if (playerTransform != null)
            {
                initialPlayerHeight = playerTransform.position.y;
                Debug.Log($"Initial height reset to: {initialPlayerHeight}");
            }
        }

        public float GetCurrentLensSize()
        {
            return virtualCamera != null ? virtualCamera.Lens.OrthographicSize : 0f;
        }

        public float GetTargetLensSize()
        {
            return currentTargetLensSize;
        }

        // Gizmos ile editor'da g�rselle�tirme
        void OnDrawGizmos()
        {
            if (!showDebugInfo || playerTransform == null) return;

            // Player pozisyonu
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, 0.5f);

            // Initial height line
            Gizmos.color = Color.blue;
            Vector3 initialPos = new Vector3(playerTransform.position.x, initialPlayerHeight, playerTransform.position.z);
            Gizmos.DrawLine(initialPos + Vector3.left * 2f, initialPos + Vector3.right * 2f);

            // Max height line
            Gizmos.color = Color.red;
            Vector3 maxPos = new Vector3(playerTransform.position.x, initialPlayerHeight + maxHeight, playerTransform.position.z);
            Gizmos.DrawLine(maxPos + Vector3.left * 2f, maxPos + Vector3.right * 2f);
        }
    }
}