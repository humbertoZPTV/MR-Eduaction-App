using UnityEngine;

public class Fusible : MonoBehaviour
{
    public GameObject motor;
    public float distanciaMaxima = 0.5f;
    private bool puedeColisionar = false;

    void Start()
    {
        Invoke("ActivarColision", 1.0f);
    }

    private void ActivarColision()
    {
        puedeColisionar = true;
    }

    // Nuevo método para reactivar la colisión
    public void ReactivarColision()
    {
        puedeColisionar = true;
    }

    void Update()
    {
        if (puedeColisionar && motor != null && FindObjectOfType<GameManager>() != null)
        {
            float distancia = Vector3.Distance(transform.position, motor.transform.position);

            if (distancia < distanciaMaxima)
            {
                puedeColisionar = false; // Desactivar *antes* de llamar a RespuestaSeleccionada
                GameManager gm = FindObjectOfType<GameManager>();
                if (gameObject.CompareTag("correcta"))
                {
                    gm.RespuestaSeleccionada(true);
                }
                else
                {
                    gm.RespuestaSeleccionada(false);
                }
            }
        }
    }
}