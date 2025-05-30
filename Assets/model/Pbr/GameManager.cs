using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Necesario para OrderBy
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Pregunta> preguntas = new List<Pregunta>();
    public TextMeshPro textoCanvasRobot;
    public AudioClip sonidoError;
    public AudioClip sonidoAcierto;
    public AudioSource audioSource;
    public GameObject motor;
    public string escenaVictoria;
    public string escenaDerrota;

    [Header("Fusibles")]
    public GameObject fusibleCorrecto;
    public GameObject fusibleIncorrecto1;
    public GameObject fusibleIncorrecto2;

    private Vector3 posicionInicialCorrecto;  // Guardar la posición inicial
    private Vector3 posicionInicialIncorrecto1;
    private Vector3 posicionInicialIncorrecto2;
    private Quaternion rotacionInicialCorrecto;
    private Quaternion rotacionInicialIncorrecto1;
    private Quaternion rotacionInicialIncorrecto2;


    private int preguntaActual = 0;
    private int errores = 0;
    private int aciertos = 0;

    void Start()
    {
        // Guardar las posiciones iniciales de los fusibles
        posicionInicialCorrecto = fusibleCorrecto.transform.position;
        posicionInicialIncorrecto1 = fusibleIncorrecto1.transform.position;
        posicionInicialIncorrecto2 = fusibleIncorrecto2.transform.position;
        rotacionInicialCorrecto = fusibleCorrecto.transform.rotation;
        rotacionInicialIncorrecto1 = fusibleIncorrecto1.transform.rotation;
        rotacionInicialIncorrecto2 = fusibleIncorrecto2.transform.rotation;

        // Desactivar los fusibles al inicio
        DesactivarFusibles();
        StartCoroutine(SecuenciaDeInicio());
    }

    IEnumerator SecuenciaDeInicio()
    {
        textoCanvasRobot.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        textoCanvasRobot.gameObject.SetActive(true);
        textoCanvasRobot.text = "¡Hola! Necesito tu ayuda para encender el motor. Coloca los fusibles correctos.";
        yield return new WaitForSeconds(6f);
        ComenzarJuego();
    }

    public void ComenzarJuego()
    {
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        if (preguntaActual < preguntas.Count)
        {
            textoCanvasRobot.text = preguntas[preguntaActual].textoPregunta;
            AsignarRespuestasAFusibles();
        }
        else
        {
            FinDelJuego();
        }
    }

    void AsignarRespuestasAFusibles()
    {
        // 1. BARAJAR LAS RESPUESTAS (y los fusibles, si quieres que cambien de posición)
        List<GameObject> fusibles = new List<GameObject>() { fusibleCorrecto, fusibleIncorrecto1, fusibleIncorrecto2 };
        List<string> respuestas = new List<string>() {
            preguntas[preguntaActual].respuestaCorrecta,
            preguntas[preguntaActual].respuestaIncorrecta1,
            preguntas[preguntaActual].respuestaIncorrecta2
        };

        var rnd = new System.Random();
        var ordenAleatorio = fusibles.OrderBy(x => rnd.Next()).ToList();
        var respuestasAleatorias = respuestas.OrderBy(x => rnd.Next()).ToList();


        //  2. Asignar los textos y tags
        if (ordenAleatorio.Count == 3)
        {
            TextMeshPro tmp;
            //Fusible 0
            tmp = ordenAleatorio[0].GetComponentInChildren<TextMeshPro>();
            if (tmp != null) tmp.text = respuestasAleatorias[0];
            if (respuestasAleatorias[0] == preguntas[preguntaActual].respuestaCorrecta)
            {
                ordenAleatorio[0].tag = "correcta";
            }
            else
            {
                ordenAleatorio[0].tag = "incorrecta";
            }

            //Fusible 1
            tmp = ordenAleatorio[1].GetComponentInChildren<TextMeshPro>();
            if (tmp != null) tmp.text = respuestasAleatorias[1];
            if (respuestasAleatorias[1] == preguntas[preguntaActual].respuestaCorrecta)
            {
                ordenAleatorio[1].tag = "correcta";
            }
            else
            {
                ordenAleatorio[1].tag = "incorrecta";
            }

            //Fusible 2
            tmp = ordenAleatorio[2].GetComponentInChildren<TextMeshPro>();
            if (tmp != null) tmp.text = respuestasAleatorias[2];
            if (respuestasAleatorias[2] == preguntas[preguntaActual].respuestaCorrecta)
            {
                ordenAleatorio[2].tag = "correcta";
            }
            else
            {
                ordenAleatorio[2].tag = "incorrecta";
            }

            //  3. Regresamos los objetos a su posicion original
            ordenAleatorio[0].transform.SetPositionAndRotation(posicionInicialCorrecto, rotacionInicialCorrecto);
            ordenAleatorio[1].transform.SetPositionAndRotation(posicionInicialIncorrecto1, rotacionInicialIncorrecto1);
            ordenAleatorio[2].transform.SetPositionAndRotation(posicionInicialIncorrecto2, rotacionInicialIncorrecto2);

            // 4. Activar los fusibles
            ordenAleatorio[0].SetActive(true);
            ordenAleatorio[1].SetActive(true);
            ordenAleatorio[2].SetActive(true);
            //  AQUI
            ReactivarFusibles();
            // Activar el seguimiento
            ActivarSeguimientoFusibles();

        }
        else
        {
            Debug.LogError("Error: No hay 3 fusibles en la lista.");
        }

    }

    void ActivarSeguimientoFusibles()
    {
        // Activar el seguimiento en los textos, buscando en los hijos de los fusibles.
        SeguirFusible seguir;

        seguir = fusibleCorrecto.GetComponentInChildren<SeguirFusible>();
        if (seguir != null) seguir.EmpezarASeguir();

        seguir = fusibleIncorrecto1.GetComponentInChildren<SeguirFusible>();
        if (seguir != null) seguir.EmpezarASeguir();

        seguir = fusibleIncorrecto2.GetComponentInChildren<SeguirFusible>();
        if (seguir != null) seguir.EmpezarASeguir();
    }



    public void RespuestaSeleccionada(bool esCorrecta)
    {
        if (esCorrecta)
        {
            aciertos++;
            audioSource.PlayOneShot(sonidoAcierto);
        }
        else
        {
            errores++;
            audioSource.PlayOneShot(sonidoError);
        }

        // Comprobar condiciones de victoria/derrota *antes* de pasar a la siguiente pregunta
        if (aciertos >= 7)
        {
            FinDelJuego();
            return;
        }
        else if (errores >= 3)
        {
            FinDelJuego();
            return;
        }
        // Desactivar los fusibles *antes* de pasar a la siguiente pregunta
        DesactivarFusibles();
        preguntaActual++;
        MostrarPregunta(); // Muestra la siguiente pregunta
    }

    void FinDelJuego()
    {
        // Desactivar los fusibles al final del juego, por si acaso.
        DesactivarFusibles();

        if (aciertos >= 7)
        {
            textoCanvasRobot.text = "¡Has ganado! ¡El motor funciona!";
            SceneManager.LoadScene(escenaVictoria);
        }
        else
        {
            textoCanvasRobot.text = "Te has quedado sin intentos. :(";
            SceneManager.LoadScene(escenaDerrota);
        }
    }

    void DesactivarFusibles()
    {
        fusibleCorrecto.SetActive(false);
        fusibleIncorrecto1.SetActive(false);
        fusibleIncorrecto2.SetActive(false);
    }

    void ReactivarFusibles()
    {
        Fusible fusible;

        fusible = fusibleCorrecto.GetComponent<Fusible>();
        if (fusible != null) fusible.ReactivarColision();

        fusible = fusibleIncorrecto1.GetComponent<Fusible>();
        if (fusible != null) fusible.ReactivarColision();

        fusible = fusibleIncorrecto2.GetComponent<Fusible>();
        if (fusible != null) fusible.ReactivarColision();
    }
}