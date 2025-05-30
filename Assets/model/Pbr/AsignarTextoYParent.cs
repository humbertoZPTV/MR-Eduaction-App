using UnityEngine;
using TMPro;

public class AsignarTextoYParent : MonoBehaviour
{
    public TextMeshPro textoAsignado; // El TMP que se va a mover

    public void AsignarTexto(string texto)
    {
        if (textoAsignado != null)
        {
            textoAsignado.text = texto;
        }
    }

    public void HacerHijoDe(Transform nuevoPadre)
    {
        if (textoAsignado != null)
        {
            textoAsignado.transform.SetParent(nuevoPadre);
            textoAsignado.transform.localPosition = Vector3.zero; // Importante: Resetear posición local
            textoAsignado.transform.localRotation = Quaternion.identity;//Importante, reseteamos rotacion local.

        }
    }
}