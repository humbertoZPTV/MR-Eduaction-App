using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Referencia a la c�mara principal
    private Camera mainCamera;

    void Start()
    {
        // Obtener la c�mara principal al inicio
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Si la c�mara principal existe, hacer que el objeto mire hacia ella
        if (mainCamera != null)
        {
            // Hacer que el objeto mire hacia la c�mara
            transform.LookAt(mainCamera.transform);
        }
    }
}