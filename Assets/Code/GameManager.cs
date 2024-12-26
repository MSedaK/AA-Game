using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ColorManager _colorManager;
    private int score;
    private bool isRestarting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game" && isRestarting)
        {
            isRestarting = false;
            StartCoroutine(InitializeGameDelayed());
        }
    }

    private IEnumerator InitializeGameDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        ResetGameState();
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.StartLevel();
        }

        if (_colorManager != null)
        {
            _colorManager.SetNormal();
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        isRestarting = true;
        ResetGameState();
        Time.timeScale = 1;

        if (BulletManager.Instance != null)
        {
            BulletManager.Instance.ResetManager();
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetLevel();
            LevelManager.Instance.StartLevel();
        }

        SceneManager.LoadScene("Game");
    }

    private void ResetGameState()
    {
        Debug.Log("Resetting game state...");
        score = 0;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetLevel();
        }

        if (_colorManager != null)
        {
            _colorManager.SetNormal();
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(score);
            UIManager.Instance.HideGameOver();
        }
    }

    public void OnLevelComplete()
    {
        if (_colorManager != null)
        {
            _colorManager.OnSuccess();
        }
        AddScore(1);
        StartCoroutine(StartNextLevelDelayed());
    }

    private IEnumerator StartNextLevelDelayed()
    {
        yield return new WaitForSeconds(1f);
        StartGame();
    }

    public void OnGameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;

        if (_colorManager != null)
        {
            _colorManager.OnFail();
        }

        // Son skoru kaydet
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();

        // UI'ı güncelle
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(score);
        }

        // Restart sahnesine geç
        StartCoroutine(LoadRestartSceneDelayed());
    }

    private IEnumerator LoadRestartSceneDelayed()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Restart");
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(score);
        }
    }

    public int GetScore()
    {
        return score;
    }
}