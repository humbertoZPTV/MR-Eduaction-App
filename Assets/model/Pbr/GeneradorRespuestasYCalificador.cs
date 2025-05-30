using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GeneradorRespuestasYCalificador : MonoBehaviour
{
    [Header("UI de Preguntas y Respuestas")]
    public TMP_Text preguntaText;
    public TMP_Text[] respuestasText = new TMP_Text[3];

    [Header("Configuración de Escenas")]
    public string escenaCorrecta;
    public string escenaIncorrecta;

    private int respuestasCorrectas = 0;
    private int respuestasIncorrectas = 0;

    [System.Serializable]
    public class Pregunta
    {
        public string preguntaTexto;
        public string[] respuestas = new string[3];
        public int respuestaCorrectaIndex;
    }

    public Pregunta[] preguntasYRespuestas = new Pregunta[10];

    void Start()
    {
        CambiarPreguntaYRespuestas();
    }

    public void CambiarPreguntaYRespuestas()
    {
        int indexPregunta = Random.Range(0, preguntasYRespuestas.Length);
        Pregunta preguntaActual = preguntasYRespuestas[indexPregunta];

        preguntaText.text = preguntaActual.preguntaTexto;

        for (int i = 0; i < preguntaActual.respuestas.Length; i++)
        {
            respuestasText[i].text = preguntaActual.respuestas[i];
        }

        AsignarRespuestaCorrecta(preguntaActual.respuestaCorrectaIndex);
    }

    private void AsignarRespuestaCorrecta(int respuestaCorrectaIndex)
    {
        respuestasText[respuestaCorrectaIndex].text += " (Correcta)";
    }

    public void RegistrarRespuesta(bool esCorrecta)
    {
        if (esCorrecta)
        {
            respuestasCorrectas++;
            if (respuestasCorrectas >= 7)
            {
                SceneManager.LoadScene(escenaCorrecta);
            }
        }
        else
        {
            respuestasIncorrectas++;
            if (respuestasIncorrectas >= 3)
            {
                SceneManager.LoadScene(escenaIncorrecta);
            }
        }
    }
}