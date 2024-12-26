using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failColor = Color.red;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        SetNormal();
    }

    public void SetNormal()
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = normalColor;
        }
    }

    public void OnSuccess()
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = successColor;
        }
    }

    public void OnFail()
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = failColor;
        }
    }
}
