using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelController> levelPrefabs;
    private int _index;
    private LevelController _current;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        if (_index >= levelPrefabs.Count)
        {
            _index = 0;
        }

        if (levelPrefabs != null && levelPrefabs.Count > 0)
        {
            if (_current != null)
            {
                Destroy(_current.gameObject);
            }

            _current = Instantiate(levelPrefabs[_index]);
            _current.MyStart(_index);

            if (BulletManager.Instance != null)
            {
                BulletManager.Instance.bulletRemain = 5; // Varsayılan bullet sayısı
            }

            // Yeni level başlarken tüm eski bullet'ları temizle
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
        }
    }

    public void ResetLevel()
    {
        _index = 0;
        if (_current != null)
        {
            Destroy(_current.gameObject);
            _current = null;
        }

        // Reset sırasında tüm bullet'ları temizle
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        if (BulletManager.Instance != null)
        {
            BulletManager.Instance.bulletRemain = 5;
        }
    }

    public bool CheckLevelComplete()
    {
        if (BulletManager.Instance == null) return false;
        return BulletManager.Instance.bulletRemain <= 0;
    }

    public void CompleteLevel()
    {
        Debug.Log($"Completing level {_index + 1}");
        _index++;

        if (_current != null)
        {
            Destroy(_current.gameObject);
        }

        // Level tamamlandığında tüm bullet'ları temizle
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelComplete();
        }
    }

    public int GetCurrentLevelIndex()
    {
        return _index;
    }

    public void SetLevelIndex(int index)
    {
        _index = index;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}