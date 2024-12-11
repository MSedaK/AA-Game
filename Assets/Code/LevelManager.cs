using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnStartLevel()
    {
        var prefab = levelPrefabs[_index];
        _current = Instantiate(prefab);
        _current.MyStart(_index);
    }

    public void OnFinishLevel()
    {
        _index++;
        Destroy(_current.gameObject);
        GameManager.Instance.OnSuccess();
        GameManager.Instance.AddScore(1);  // Skor artırma
    }

    public bool IsLevelCompleted()
    {
        return BulletManager.Instance.bulletRemain == 0;
    }


}