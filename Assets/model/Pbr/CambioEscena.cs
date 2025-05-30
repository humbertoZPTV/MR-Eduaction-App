using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    [Tooltip("Nombre de la escena a la que se cambiará.")]
    public string nombreDeLaEscena;

    [Tooltip("Si es true, el cambio de escena ocurrirá automáticamente al iniciar. Si es false, se debe llamar a CambiarAEscena() desde otro script.")]
    public bool cambiarAlIniciar = false;

    [Tooltip("Tiempo de espera en segundos antes de cambiar de escena (solo si cambiarAlIniciar es true).")]
    public float tiempoDeEspera = 0f;

    void Start()
    {
        if (cambiarAlIniciar)
        {
            Invoke("CambiarAEscena", tiempoDeEspera);
        }
    }

    // Esta función se llama para cambiar de escena.  Puede ser llamada desde otro script,
    // o desde un evento (como un botón, una colisión, etc.).
    public void CambiarAEscena()
    {
        if (string.IsNullOrEmpty(nombreDeLaEscena))
        {
            Debug.LogError("CambioEscena: El nombre de la escena no está configurado.");
            return;
        }

        SceneManager.LoadScene(nombreDeLaEscena);
    }

    //Funcion para cambiar de escena, con un parametro string.
    public void CambiarAEscena(string _escena)
    {
        if (string.IsNullOrEmpty(_escena))
        {
            Debug.LogError("CambioEscena: El nombre de la escena no está configurado.");
            return;
        }

        SceneManager.LoadScene(_escena);
    }
}