using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Referencia a la cámara principal
    private Camera mainCamera;

    void Start()
    {
        // Obtener la cámara principal al inicio
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Si la cámara principal existe, hacer que el objeto mire hacia ella
        if (mainCamera != null)
        {
            // Hacer que el objeto mire hacia la cámara
            transform.LookAt(mainCamera.transform);
        }
    }
}