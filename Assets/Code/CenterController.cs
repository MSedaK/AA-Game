using UnityEngine;

public class CenterController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Inspector'dan ayarlanabilir hýz
    private bool isRotating = false;

    private void Start()
    {
        // Baþlangýçta dönmeyi baþlat
        StartRotate(rotationSpeed);
    }

    public void StartRotate(float speed)
    {
        rotationSpeed = speed;
        isRotating = true;
    }

    public void StopRotate()
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
    }
}