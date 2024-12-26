using UnityEngine;

public class CenterCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            if (bullet != null && !bullet.IsCompleted)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnGameOver();
                }
            }
        }
    }
}