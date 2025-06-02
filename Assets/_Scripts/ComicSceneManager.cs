using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicSceneManager : MonoBehaviour
{
    [Header("Comic Pages")]
    public List<Image> comicPages = new List<Image>(); // Arka plan comic sayfalar�

    [Header("Sprites to Hide")]
    public List<GameObject> spritesToHide = new List<GameObject>(); // Kapat�lacak spritelar

    [Header("Settings")]
    public float fadeOutDuration = 0.5f; // Sprite kaybolma s�resi
    public bool useClickSound = true;
    public AudioClip clickSound;
    public AudioClip click2Sound; // Son sayfa i�in farkl� ses dosyas�

    [Header("UI")]
    public Text instructionText; // "Space veya Sol T�k ile devam edin" gibi
    public GameObject completionPanel; // T�m spritelar kapat�ld���nda g�sterilecek panel

    private int currentSpriteIndex = 0;
    private bool isTransitioning = false;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource component'i ekle
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && useClickSound)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ba�lang��ta t�m spritelar g�r�n�r olmal�
        foreach (GameObject sprite in spritesToHide)
        {
            if (sprite != null)
            {
                sprite.SetActive(true);
            }
        }

        // Completion panel'i gizle
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        // Instruction text'i g�ncelle
        UpdateInstructionText();
    }

    void Update()
    {
        // E�er ge�i� animasyonu devam ediyorsa input almay� bekle
        if (isTransitioning) return;

        // Space tu�u veya sol mouse t�klamas� kontrol�
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            HideNextSprite();
        }
    }

    void HideNextSprite()
    {
        // E�er t�m spritelar kapat�ld�ysa
        if (currentSpriteIndex >= spritesToHide.Count)
        {
            OnAllSpritesHidden();
            return;
        }

        // Ge�erli sprite'� kontrol et
        GameObject currentSprite = spritesToHide[currentSpriteIndex];
        if (currentSprite != null && currentSprite.activeInHierarchy)
        {
            // Son sprite m� kontrol et ve uygun sesi �al
            bool isLastSprite = (currentSpriteIndex == spritesToHide.Count - 1);
            PlayClickSound(isLastSprite);

            // Sprite'� kaybet
            StartCoroutine(FadeOutSprite(currentSprite));
        }

        currentSpriteIndex++;
        UpdateInstructionText();
    }

    IEnumerator FadeOutSprite(GameObject sprite)
    {
        isTransitioning = true;

        // Sprite Renderer veya Image component'ini bul
        SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        Image imageComponent = sprite.GetComponent<Image>();

        float elapsedTime = 0f;
        Color originalColor = Color.white;

        // Hangi component kullan�ld���n� belirle
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else if (imageComponent != null)
        {
            originalColor = imageComponent.color;
        }

        // Fade out animasyonu
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);

            Color newColor = originalColor;
            newColor.a = alpha;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = newColor;
            }
            else if (imageComponent != null)
            {
                imageComponent.color = newColor;
            }

            yield return null;
        }

        // Sprite'� tamamen gizle
        sprite.SetActive(false);

        isTransitioning = false;
    }

    void PlayClickSound(bool isLastSprite = false)
    {
        if (useClickSound && audioSource != null)
        {
            // Son sprite i�in click2 sesini �al, di�erleri i�in normal click sesini
            if (isLastSprite && click2Sound != null)
            {
                audioSource.PlayOneShot(click2Sound);
            }
            else if (clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }

    void UpdateInstructionText()
    {
        if (instructionText != null)
        {
            if (currentSpriteIndex < spritesToHide.Count)
            {
                instructionText.text = $"Devam etmek i�in Space veya Sol T�k ({currentSpriteIndex + 1}/{spritesToHide.Count})";
            }
            else
            {
                instructionText.text = "Comic tamamland�!";
            }
        }
    }

    void OnAllSpritesHidden()
    {
        Debug.Log("T�m spritelar kapat�ld�! Comic sahne tamamland�.");

        // Completion panel'i g�ster
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        // Buraya comic sahne bitti�inde yap�lacak i�lemleri ekleyebilirsiniz
        // �rne�in: sahne ge�i�i, skor hesaplama, vb.
        OnComicCompleted();
    }

    // Bu metodu override edebilir veya UnityEvent olarak kullanabilirsiniz
    protected virtual void OnComicCompleted()
    {
        // Comic tamamland���nda yap�lacak i�lemler
        // �rnek: ana men�ye d�n, sonraki seviyeye ge�, vb.

        // �rnek kullan�m:
        // SceneManager.LoadScene("MainMenu");
        // GameManager.Instance.CompleteLevel();
    }

    // Public metodlar - di�er scriptlerden �a��r�labilir
    public void ResetComicScene()
    {
        currentSpriteIndex = 0;
        isTransitioning = false;

        // T�m spritelar� tekrar g�ster
        foreach (GameObject sprite in spritesToHide)
        {
            if (sprite != null)
            {
                sprite.SetActive(true);

                // Alpha de�erini s�f�rla
                SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();
                Image img = sprite.GetComponent<Image>();

                if (sr != null)
                {
                    Color color = sr.color;
                    color.a = 1f;
                    sr.color = color;
                }
                else if (img != null)
                {
                    Color color = img.color;
                    color.a = 1f;
                    img.color = color;
                }
            }
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        UpdateInstructionText();
    }

    public void SkipToEnd()
    {
        // T�m spritelar� an�nda gizle
        StopAllCoroutines();

        foreach (GameObject sprite in spritesToHide)
        {
            if (sprite != null)
            {
                sprite.SetActive(false);
            }
        }

        currentSpriteIndex = spritesToHide.Count;
        isTransitioning = false;
        OnAllSpritesHidden();
    }
}