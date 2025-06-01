using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool _isGameOver;

    public void SetISGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "GameScene";

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

    void Update()
    {
        // R tu�una bas�ld���nda sahneyi yeniden y�kle
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_isGameOver) return;
            ReloadCurrentScene();
        }
    }
    // Ana men�ye git
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Oyun sahnesine git
    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Belirli bir sahneye git (index ile)
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Belirli bir sahneye git (isim ile)
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Son seviyeye ula��ld�ysa ana men�ye d�n
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            LoadMainMenu();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    // Mevcut sahneyi yeniden y�kle
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        _isGameOver = false;
        SceneManager.LoadScene(currentScene.name);
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