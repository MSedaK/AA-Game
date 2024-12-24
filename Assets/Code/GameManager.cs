using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ColorManager _colorManager;

    private int score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        LevelManager.Instance.OnStartLevel();
        _colorManager.SetNormal();
    }

    public void OnGameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0; // Oyun duraklatılıyor
        _colorManager.OnFail();

        // MainMenu sahnesine geçiş yap
        SceneManager.LoadScene("MainMenu");
    }

    public void OnSuccess()
    {
        _colorManager.OnSuccess();

        // Skoru artır
        AddScore(1); // Her level geçişinde 1 puan ekle

        IEnumerator Do()
        {
            yield return new WaitForSeconds(1);
            OnStart();
        }

        StartCoroutine(Do());
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score); // Skoru konsola yazdır
    }
}
