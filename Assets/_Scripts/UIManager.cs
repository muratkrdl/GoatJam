using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIManager : MonoBehaviour
{
    [Header("Main Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Tutorial Panel")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Sprite[] tutorialSprites;
    private int currentSpriteIndex = 0;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;
    [SerializeField] private Button settingsBackButton;

    [Header("Credits UI")]
    [SerializeField] private Button creditsBackButton;
    [SerializeField] private TextMeshProUGUI creditsText;

    [Header("In-Game UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button pauseQuitButton;

    [Header("Game Over UI")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button deathMainMenuButton;

    [Header("Victory UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button winMainMenuButton;
    [SerializeField] private Button winRestartButton;

    private bool isPaused = false;
    private bool isInTutorial = false;
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";

    void Start()
    {
        // Ana men� butonlar�
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        // Settings butonlar�
        if (settingsBackButton != null)
            settingsBackButton.onClick.AddListener(OnSettingsBackClicked);

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            musicSlider.value = SoundManager.Instance != null ? SoundManager.Instance.GetMusicVolume() : 1f;
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            sfxSlider.value = SoundManager.Instance != null ? SoundManager.Instance.GetSFXVolume() : 1f;
        }

        // Credits butonlar�
        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(OnCreditsBackClicked);

        // Pause men� butonlar�
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);

        if (pauseQuitButton != null)
            pauseQuitButton.onClick.AddListener(OnQuitClicked);

        // Death panel butonlar�
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (deathMainMenuButton != null)
            deathMainMenuButton.onClick.AddListener(OnMainMenuClicked);

        // Victory panel butonlar�
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);

        if (winMainMenuButton != null)
            winMainMenuButton.onClick.AddListener(OnMainMenuClicked);

        if (winRestartButton != null)
            winRestartButton.onClick.AddListener(OnRestartClicked);

        // �lk durumlar� ayarla
        ShowMainMenu();
    }

    void Update()
    {
        // Tutorial input kontrol�
        if (isInTutorial)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                NextTutorialSprite();
            }
        }

        // ESC tu�u kontrol� (sadece oyun sahnesinde)
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isInTutorial)
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    // Ana Men� Fonksiyonlar�
    void OnPlayClicked()
    {
        SoundManager.Instance?.PlayButtonClick();

        // Tutorial daha �nce tamamlanm�� m� kontrol et
        if (PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 0)
        {
            ShowTutorial(); // �lk kez oynuyorsa tutorial g�ster
        }
        else
        {
            StartGame(); // Tutorial tamamlanm��sa direkt oyunu ba�lat
        }
    }

    void OnSettingsClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowSettings();
    }

    void OnCreditsClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowCredits();
    }

    void OnQuitClicked()
    {
        SoundManager.Instance?.PlayButtonClick();

        // Quit'e bas�nca tutorial'� s�f�rla
        PlayerPrefs.DeleteKey(TUTORIAL_COMPLETED_KEY);
        PlayerPrefs.Save();

        Debug.Log("Tutorial reset on quit!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Tutorial Fonksiyonlar�
    void ShowTutorial()
    {
        if (tutorialPanel != null && tutorialSprites != null && tutorialSprites.Length > 0)
        {
            // Panelleri ayarla
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (creditsPanel != null) creditsPanel.SetActive(false);

            tutorialPanel.SetActive(true);

            // Tutorial durumunu ba�lat
            isInTutorial = true;
            currentSpriteIndex = 0;

            // �lk sprite'� g�ster
            if (tutorialImage != null)
                tutorialImage.sprite = tutorialSprites[0];
        }
        else
        {
            // Tutorial sprite'lar� yoksa direkt oyunu ba�lat
            Debug.LogWarning("Tutorial sprites not assigned! Starting game directly.");
            StartGame();
        }
    }

    void NextTutorialSprite()
    {
        currentSpriteIndex++;

        // E�er daha sprite varsa g�ster
        if (currentSpriteIndex < tutorialSprites.Length)
        {
            if (tutorialImage != null)
                tutorialImage.sprite = tutorialSprites[currentSpriteIndex];
        }
        else
        {
            // T�m sprite'lar bitti, tutorial'� tamamla
            CompleteTutorial();
        }
    }

    void CompleteTutorial()
    {
        // Tutorial'� tamamland� olarak i�aretle
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
        PlayerPrefs.Save();

        // Tutorial panelini kapat ve ana men�ye d�n
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        isInTutorial = false;

        // Ana men�y� tekrar g�ster
        ShowMainMenu();
    }

    void StartGame()
    {
        // Tutorial'� kapat
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        isInTutorial = false;

        // Oyunu ba�lat
        SceneController.Instance.LoadGameScene();
    }

    // Settings Fonksiyonlar�
    void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance?.SetMusicVolume(value);
        if (musicValueText != null)
            musicValueText.text = Mathf.RoundToInt(value * 100) + "%";
        SoundManager.Instance?.PlayButtonClick();
    }

    void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance?.SetSFXVolume(value);
        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(value * 100) + "%";
        SoundManager.Instance?.PlayButtonClick();
    }

    void OnSettingsBackClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowMainMenu();
    }

    // Credits Fonksiyonlar�
    void OnCreditsBackClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowMainMenu();
    }

    // Pause Men� Fonksiyonlar�
    void OnResumeClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ResumeGame();
    }

    void OnMainMenuClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;
        SceneController.Instance.LoadMainMenu();
    }

    // Victory Panel Fonksiyonlar�
    void OnNextLevelClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;

        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneController.Instance.LoadMainMenu();
        }
    }

    void OnRestartClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;
        SceneController.Instance.ReloadCurrentScene();
    }

    // Panel G�sterme Fonksiyonlar�
    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        isInTutorial = false;
    }

    void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        UpdateSettingsUI();
    }

    void ShowCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    void UpdateSettingsUI()
    {
        if (SoundManager.Instance != null)
        {
            if (musicSlider != null)
                musicSlider.value = SoundManager.Instance.GetMusicVolume();

            if (sfxSlider != null)
                sfxSlider.value = SoundManager.Instance.GetSFXVolume();

            if (musicValueText != null)
                musicValueText.text = Mathf.RoundToInt(SoundManager.Instance.GetMusicVolume() * 100) + "%";

            if (sfxValueText != null)
                sfxValueText.text = Mathf.RoundToInt(SoundManager.Instance.GetSFXVolume() * 100) + "%";
        }
    }

    // Victory Panel Y�netimi
    public void ShowWinPanel()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void HideWinPanel()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // Death Panel Y�netimi
    public void ShowDeathPanel()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void HideDeathPanel()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // Oyun Duraklatma
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        SoundManager.Instance?.PlayButtonClick();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        SoundManager.Instance?.PlayButtonClick();
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    // Debug fonksiyonu - Tutorial'� s�f�rlamak i�in
    [ContextMenu("Reset Tutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(TUTORIAL_COMPLETED_KEY);
        PlayerPrefs.Save();
        Debug.Log("Tutorial reset! Player can see tutorial again.");
    }
}