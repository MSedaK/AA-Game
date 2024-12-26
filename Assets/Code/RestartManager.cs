using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        SetupButtons();
        UpdateScoreText();
    }

    private void SetupButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnRestartClick);
        }
        else
        {
            Debug.LogError("Restart button is missing!");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(OnMainMenuClick);
        }
    }

    private void OnRestartClick()
    {
        Debug.Log("Restart button clicked");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
            // Yedek plan olarak direkt sahne yükleme
            Time.timeScale = 1;
            SceneManager.LoadScene("Game");
        }
    }

    private void OnMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            int lastScore = PlayerPrefs.GetInt("LastScore", 0);
            scoreText.text = "Final Score: " + lastScore;
        }
    }
}
