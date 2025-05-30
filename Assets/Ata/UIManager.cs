using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

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

    private bool isPaused = false;

    void Start()
    {
        // Ana menü butonlarý
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        // Settings butonlarý
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

        // Credits butonlarý
        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(OnCreditsBackClicked);

        // Pause menü butonlarý
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);

        if (pauseQuitButton != null)
            pauseQuitButton.onClick.AddListener(OnQuitClicked);

        // Ýlk durumlarý ayarla
        ShowMainMenu();

        // Credits text'i ayarla
        if (creditsText != null)
        {
            creditsText.text = "2D Rage Game\n\n" +
                              "Developed by: Your Name\n" +
                              "Art: Artist Name\n" +
                              "Music: Musician Name\n\n" +
                              "Special Thanks:\n" +
                              "Unity Technologies\n" +
                              "Our Amazing Players!";
        }
    }

    void Update()
    {
        // ESC tuþu kontrolü (sadece oyun sahnesinde)
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    // Ana Menü Fonksiyonlarý
    void OnPlayClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        SceneController.Instance.LoadGameScene();
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Settings Fonksiyonlarý
    void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance?.SetMusicVolume(value);
        if (musicValueText != null)
            musicValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance?.SetSFXVolume(value);
        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void OnSettingsBackClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowMainMenu();
    }

    // Credits Fonksiyonlarý
    void OnCreditsBackClicked()
    {
        SoundManager.Instance?.PlayButtonClick();
        ShowMainMenu();
    }

    // Pause Menü Fonksiyonlarý
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

    // Panel Gösterme Fonksiyonlarý
    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        // Slider deðerlerini güncelle
        UpdateSettingsUI();
    }

    void ShowCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
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
}