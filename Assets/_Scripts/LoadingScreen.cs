using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject spinner;

    [Header("Loading Animation")]
    [SerializeField] private float spinnerSpeed = 200f;
    [SerializeField] private string[] loadingDots = { ".", "..", "..." };

    private float dotTimer = 0f;
    private int currentDotIndex = 0;

    void Start()
    {
        // Canvas'� en �stte g�ster
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 999;
        }

        // Ba�lang�� de�erleri
        UpdateProgress(0f);
    }

    void Update()
    {
        // Spinner d�nd�rme
        if (spinner != null)
        {
            spinner.transform.Rotate(0f, 0f, -spinnerSpeed * Time.deltaTime);
        }

        // Loading text animasyonu
        AnimateLoadingText();
    }

    public void UpdateProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        // Progress bar g�ncelle
        if (progressBar != null)
        {
            progressBar.fillAmount = progress;
        }

        // Progress text g�ncelle
        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
        }
    }

    void AnimateLoadingText()
    {
        if (loadingText == null) return;

        dotTimer += Time.deltaTime;

        if (dotTimer >= 0.5f)
        {
            dotTimer = 0f;
            currentDotIndex = (currentDotIndex + 1) % loadingDots.Length;
            loadingText.text = "Loading" + loadingDots[currentDotIndex];
        }
    }
}