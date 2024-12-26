using UnityEngine;
using TMPro;
using System.Collections;

public class BulletController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer line;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float finalOffset = 0.5f;

    private bool isCompleted = false;
    private bool isCollided = false;
    public bool IsCompleted => isCompleted;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer zaten tamamlanmışsa (merkeze saplanmışsa) çarpışmayı kontrol etme
        if (isCompleted) return;

        // Bullet'a veya engele çarpma kontrolü
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (!isCollided)
            {
                isCollided = true;
                Debug.Log("Collision detected with: " + collision.gameObject.tag);
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnGameOver();
                }
            }
        }
    }

    public void MyStart(int remain)
    {
        if (text != null)
        {
            text.SetText(remain.ToString());
        }
        isCompleted = false;
        isCollided = false;

        if (line != null)
        {
            line.enabled = false;
        }
    }

    public void Move(Vector3 target, float time)
    {
        StartCoroutine(_Move(target, time));
    }

    private IEnumerator _Move(Vector3 targetPosition, float time)
    {
        var passed = 0f;
        var init = transform.position;

        Vector3 directionToCenter = (targetPosition - init).normalized;
        Vector3 adjustedTarget = targetPosition - (directionToCenter * finalOffset);

        while (passed < time && !isCollided)
        {
            yield return null;
            passed += Time.deltaTime;
            var normalize = passed / time;
            var newPosition = Vector3.Lerp(init, adjustedTarget, normalize);
            transform.position = newPosition;
        }

        if (!isCollided)
        {
            transform.position = adjustedTarget;
            GameObject center = GameObject.FindGameObjectWithTag("Center");
            if (center != null)
            {
                OnCompleted(center.transform);
            }
        }
    }

    public void OnCompleted(Transform parent)
    {
        if (isCompleted || isCollided) return;

        isCompleted = true;
        if (parent != null)
        {
            // Mevcut pozisyonu kaydet
            Vector3 currentPos = transform.position;

            // Merkeze olan yönü hesapla (merkeze doğru bakan vektör)
            Vector3 directionToCenter = (parent.position - currentPos).normalized;

            // Merkeze dik açıyı hesapla (90 derece)
            float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;

            // Parent'a ekle
            transform.SetParent(parent);

            // Pozisyonu koru ve rotasyonu ayarla
            transform.position = currentPos;

            // Bullet'ı merkeze dik olacak şekilde döndür
            // -90 yerine 90 kullanarak dik pozisyonu ayarlıyoruz
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            // Local rotation'ı da ayarla ki merkez dönerken doğru pozisyonda kalsın
            transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
        }

        if (line != null)
        {
            line.enabled = true;
        }

        if (LevelManager.Instance != null)
        {
            if (LevelManager.Instance.CheckLevelComplete())
            {
                LevelManager.Instance.CompleteLevel();
            }
        }
    }
}