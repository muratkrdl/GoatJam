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

    // Ana men�ye git
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

    // Mevcut sahneyi yeniden y�kle
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadSceneAsync(currentScene.name));
    }

    // Asenkron sahne y�kleme (isim ile)
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Loading screen olu�tur
        ShowLoadingScreen();

        float startTime = Time.time;

        // Sahne y�klemeyi ba�lat
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Y�kleme tamamlanana kadar bekle
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Loading screen'de ilerleme g�ster
            UpdateLoadingProgress(progress);

            // Y�kleme %90'a geldi�inde ve minimum s�re ge�tiyse sahneyi aktif et
            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Loading screen'i kapat
        HideLoadingScreen();
    }

    // Asenkron sahne y�kleme (index ile)
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

    // Loading screen g�ster
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

    // Loading ilerleme g�ncelleme
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