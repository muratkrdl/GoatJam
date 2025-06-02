using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicSceneManager : MonoBehaviour
{
    [Header("Comic Pages")]
    public List<Image> comicPages = new List<Image>(); // Arka plan comic sayfalarý

    [Header("Sprites to Hide")]
    public List<GameObject> spritesToHide = new List<GameObject>(); // Kapatýlacak spritelar

    [Header("Settings")]
    public float fadeOutDuration = 0.5f; // Sprite kaybolma süresi
    public bool useClickSound = true;
    public AudioClip clickSound;
    public AudioClip click2Sound; // Son sayfa için farklý ses dosyasý

    [Header("UI")]
    public Text instructionText; // "Space veya Sol Týk ile devam edin" gibi
    public GameObject completionPanel; // Tüm spritelar kapatýldýðýnda gösterilecek panel

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

        // Baþlangýçta tüm spritelar görünür olmalý
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

        // Instruction text'i güncelle
        UpdateInstructionText();
    }

    void Update()
    {
        // Eðer geçiþ animasyonu devam ediyorsa input almayý bekle
        if (isTransitioning) return;

        // Space tuþu veya sol mouse týklamasý kontrolü
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            HideNextSprite();
        }
    }

    void HideNextSprite()
    {
        // Eðer tüm spritelar kapatýldýysa
        if (currentSpriteIndex >= spritesToHide.Count)
        {
            OnAllSpritesHidden();
            return;
        }

        // Geçerli sprite'ý kontrol et
        GameObject currentSprite = spritesToHide[currentSpriteIndex];
        if (currentSprite != null && currentSprite.activeInHierarchy)
        {
            // Son sprite mý kontrol et ve uygun sesi çal
            bool isLastSprite = (currentSpriteIndex == spritesToHide.Count - 1);
            PlayClickSound(isLastSprite);

            // Sprite'ý kaybet
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

        // Hangi component kullanýldýðýný belirle
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

        // Sprite'ý tamamen gizle
        sprite.SetActive(false);

        isTransitioning = false;
    }

    void PlayClickSound(bool isLastSprite = false)
    {
        if (useClickSound && audioSource != null)
        {
            // Son sprite için click2 sesini çal, diðerleri için normal click sesini
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
                instructionText.text = $"Devam etmek için Space veya Sol Týk ({currentSpriteIndex + 1}/{spritesToHide.Count})";
            }
            else
            {
                instructionText.text = "Comic tamamlandý!";
            }
        }
    }

    void OnAllSpritesHidden()
    {
        Debug.Log("Tüm spritelar kapatýldý! Comic sahne tamamlandý.");

        // Completion panel'i göster
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        // Buraya comic sahne bittiðinde yapýlacak iþlemleri ekleyebilirsiniz
        // Örneðin: sahne geçiþi, skor hesaplama, vb.
        OnComicCompleted();
    }

    // Bu metodu override edebilir veya UnityEvent olarak kullanabilirsiniz
    protected virtual void OnComicCompleted()
    {
        // Comic tamamlandýðýnda yapýlacak iþlemler
        // Örnek: ana menüye dön, sonraki seviyeye geç, vb.

        // Örnek kullaným:
        // SceneManager.LoadScene("MainMenu");
        // GameManager.Instance.CompleteLevel();
    }

    // Public metodlar - diðer scriptlerden çaðýrýlabilir
    public void ResetComicScene()
    {
        currentSpriteIndex = 0;
        isTransitioning = false;

        // Tüm spritelarý tekrar göster
        foreach (GameObject sprite in spritesToHide)
        {
            if (sprite != null)
            {
                sprite.SetActive(true);

                // Alpha deðerini sýfýrla
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
        // Tüm spritelarý anýnda gizle
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