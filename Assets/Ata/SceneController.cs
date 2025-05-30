using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    // Singleton
    private static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<SceneController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SceneController");
                    instance = go.AddComponent<SceneController>();
                }
            }
            return instance;
        }
    }

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private float minimumLoadTime = 0.5f;

    private GameObject currentLoadingScreen;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Ana menüye git
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync(mainMenuSceneName));
    }

    // Oyun sahnesine git
    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync(gameSceneName));
    }

    // Belirli bir sahneye git (index ile)
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncByIndex(sceneIndex));
    }

    // Belirli bir sahneye git (isim ile)
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Mevcut sahneyi yeniden yükle
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadSceneAsync(currentScene.name));
    }

    // Asenkron sahne yükleme (isim ile)
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Loading screen oluþtur
        ShowLoadingScreen();

        float startTime = Time.time;

        // Sahne yüklemeyi baþlat
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Yükleme tamamlanana kadar bekle
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Loading screen'de ilerleme göster
            UpdateLoadingProgress(progress);

            // Yükleme %90'a geldiðinde ve minimum süre geçtiyse sahneyi aktif et
            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Loading screen'i kapat
        HideLoadingScreen();
    }

    // Asenkron sahne yükleme (index ile)
    private IEnumerator LoadSceneAsyncByIndex(int sceneIndex)
    {
        ShowLoadingScreen();

        float startTime = Time.time;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            UpdateLoadingProgress(progress);

            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        HideLoadingScreen();
    }

    // Loading screen göster
    private void ShowLoadingScreen()
    {
        if (loadingScreenPrefab != null && currentLoadingScreen == null)
        {
            currentLoadingScreen = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(currentLoadingScreen);
        }
    }

    // Loading screen gizle
    private void HideLoadingScreen()
    {
        if (currentLoadingScreen != null)
        {
            Destroy(currentLoadingScreen);
            currentLoadingScreen = null;
        }
    }

    // Loading ilerleme güncelleme
    private void UpdateLoadingProgress(float progress)
    {
        if (currentLoadingScreen != null)
        {
            LoadingScreen loadingScript = currentLoadingScreen.GetComponent<LoadingScreen>();
            if (loadingScript != null)
            {
                loadingScript.UpdateProgress(progress);
            }
        }
    }

    // Sahne bilgilerini al
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public bool IsMainMenu()
    {
        return GetCurrentSceneName() == mainMenuSceneName;
    }

    public bool IsGameScene()
    {
        return GetCurrentSceneName() == gameSceneName;
    }
}