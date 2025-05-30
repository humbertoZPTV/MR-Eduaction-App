using UnityEngine;

public class SeguirFusible : MonoBehaviour
{
    public string tagDelFusible; // El tag del fusible que este objeto debe seguir
    private Transform fusibleTransform;
    public bool seguir = false;

    void LateUpdate()
    {
        if (seguir)
        {
            if (fusibleTransform == null)
            {
                GameObject fusible = GameObject.FindGameObjectWithTag(tagDelFusible);
                if (fusible != null)
                {
                    fusibleTransform = fusible.transform;
                }
            }

            if (fusibleTransform != null)
            {
                transform.position = fusibleTransform.position;
                transform.rotation = fusibleTransform.rotation;
            }
        }
    }

    public void EmpezarASeguir()
    {
        seguir = true;
    }
}