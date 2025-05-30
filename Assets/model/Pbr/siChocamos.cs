using UnityEngine;

public class siChocamos : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private AudioSource audioSource;
    private SpawnChido spawnChido;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spawnChido = FindObjectOfType<SpawnChido>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DropZone"))
        {
            GameObject other = collision.gameObject;

            if (other.CompareTag("correcta"))
            {
                audioSource.PlayOneShot(sonidoCorrecto);
            }
            else if (other.CompareTag("incorrecta1") || other.CompareTag("incorrecta2"))
            {
                audioSource.PlayOneShot(sonidoIncorrecto);
            }

            // Esperar a que termine el sonido y luego respawnear
            Invoke("RespawnObjects", audioSource.clip.length);
        }
    }

    private void RespawnObjects()
    {
        spawnChido.StartSpawn();
    }
}