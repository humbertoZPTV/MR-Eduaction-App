using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using TMPro;

public class SpawnChido : MonoBehaviour
{
    [Header("Prefabs de Respuestas")]
    public GameObject incorrecta1Prefab;
    public GameObject incorrecta2Prefab;
    public GameObject correctaPrefab;

    [Header("Configuración del Spawn")]
    public MRUK.RoomFilter SpawnOnStart = MRUK.RoomFilter.AllRooms; // Usamos AllRooms y la primera habitación
    public int MaxIterations = 1000;
    public float SurfaceClearanceDistance = 0.1f;

    [Tooltip("Objetos que se deben evitar al spawnear.  Por ejemplo, el motor.")]
    public List<GameObject> ObjetosAEVitar = new List<GameObject>();

    private void Start()
    {
        if (MRUK.Instance && SpawnOnStart != MRUK.RoomFilter.None)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                StartSpawn();
            });
        }
    }

    public void StartSpawn()
    {
        if (MRUK.Instance.Rooms.Count > 0)
        {
            MRUKRoom room = MRUK.Instance.Rooms[0];
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null && gameManager.preguntas.Count > 0)
            {
                Pregunta primeraPregunta = gameManager.preguntas[0];
                SpawnPrefab(room, incorrecta1Prefab, primeraPregunta.respuestaIncorrecta1);
                SpawnPrefab(room, incorrecta2Prefab, primeraPregunta.respuestaIncorrecta2);
                SpawnPrefab(room, correctaPrefab, primeraPregunta.respuestaCorrecta);
            }
            else
            {
                Debug.LogError("GameManager no encontrado o no tiene preguntas.");
            }
        }
        else
        {
            Debug.LogWarning("No se detectaron habitaciones.");
        }
    }

    private void SpawnPrefab(MRUKRoom room, GameObject prefab, string textoInicial)
    {
        if (room == null) return;

        var prefabBounds = Utilities.GetPrefabBounds(prefab);
        float minRadius = 0.0f;
        const float clearanceDistance = 0.01f;
        float baseOffset = -prefabBounds?.min.y ?? 0.0f;
        float centerOffset = prefabBounds?.center.y ?? 0.0f;
        Bounds adjustedBounds = new();

        if (prefabBounds.HasValue)
        {
            minRadius = Mathf.Min(-prefabBounds.Value.min.x, -prefabBounds.Value.min.z, prefabBounds.Value.max.x, prefabBounds.Value.max.z);
            if (minRadius < 0f)
            {
                minRadius = 0f;
            }

            var min = prefabBounds.Value.min;
            var max = prefabBounds.Value.max;
            min.y += clearanceDistance;
            if (max.y < min.y)
            {
                max.y = min.y;
            }

            adjustedBounds.SetMinMax(min, max);
        }

        bool foundValidSpawnPosition = false;
        for (int i = 0; i < MaxIterations; ++i)
        {
            Vector3 spawnPosition = Vector3.zero;
            Vector3 spawnNormal = Vector3.zero;

            if (room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, minRadius, new LabelFilter(MRUKAnchor.SceneLabels.TABLE), out var pos, out var normal))
            {
                spawnPosition = pos + normal * baseOffset;
                spawnNormal = normal;
                var center = spawnPosition + normal * centerOffset;

                if (!room.IsPositionInRoom(center)) continue;
                if (room.IsPositionInSceneVolume(center)) continue;
                if (room.Raycast(new Ray(pos, normal), SurfaceClearanceDistance, out _)) continue;

                bool cercaDeObjetoAEVitar = false;
                foreach (GameObject objetoAEVitar in ObjetosAEVitar)
                {
                    if (Vector3.Distance(spawnPosition, objetoAEVitar.transform.position) < 1.0f)
                    {
                        cercaDeObjetoAEVitar = true;
                        break;
                    }
                }
                if (cercaDeObjetoAEVitar) continue;

                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnNormal);

                GameObject instancia = Instantiate(prefab, spawnPosition, spawnRotation, transform);
                SeguirFusible seguirFusible = instancia.GetComponent<SeguirFusible>();

                if (seguirFusible != null)
                {
                    TextMeshPro tmp = seguirFusible.GetComponentInChildren<TextMeshPro>();
                    if (tmp != null)
                    {
                        tmp.text = textoInicial;
                    }
                }

                foundValidSpawnPosition = true;
                break;
            }
        }

        if (!foundValidSpawnPosition)
        {
            Debug.LogWarning($"No se encontró una posición válida para el prefab {prefab.name} en la habitación {room.name}.");
        }
    }
}