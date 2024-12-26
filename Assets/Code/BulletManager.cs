using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [SerializeField] private BulletController bulletPrefab;
    [Range(0, 1)][SerializeField] private float moveTime = 0.5f;
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, -3, 0);
    [SerializeField] private Vector3 targetPosition = new Vector3(0, 0, 0);

    public int bulletRemain = 5;

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

    public void SpawnAndMove()
    {
        if (bulletRemain <= 0) return;

        var bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.MyStart(bulletRemain);
        bullet.Move(targetPosition, moveTime);
        bulletRemain--;
    }
    public void ResetManager()
    {
        bulletRemain = 5;
    }
}