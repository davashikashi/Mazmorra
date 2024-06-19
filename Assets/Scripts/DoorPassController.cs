using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPassController : MonoBehaviour
{
    public Transform doorTransform; // Transform de la puerta
    public Vector3 openPosition; // Posición de la puerta abierta
    public Vector3 closedPosition; // Posición de la puerta cerrada
    public float doorSpeed = 2f; // Velocidad de movimiento de la puerta
    public List<bool> requiredLeverStates; // Estados requeridos de las palancas
    public AudioClip openSoundClip; // Clip de sonido al abrir la puerta
    public AudioClip closeSoundClip; // Clip de sonido al cerrar la puerta
    private AudioSource audioSource; // Referencia al componente AudioSource

    private bool isOpen = false; // Estado actual de la puerta

    [HideInInspector]
    public List<LeverPassController> linkedLevers; // Lista de palancas asociadas a esta puerta

    void Start()
    {
        // Asegurarse de que doorTransform esté asignado
        if (doorTransform == null)
        {
            doorTransform = transform;
        }

        // Establecer la posición inicial de la puerta como cerrada
        doorTransform.position = closedPosition;

        // Configurar el AudioSource para los sonidos de apertura y cierre
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // No reproducir automáticamente al despertar
        audioSource.clip = openSoundClip; // Asignar el AudioClip de apertura
    }

    public void CheckLevers()
    {
        for (int i = 0; i < linkedLevers.Count; i++)
        {
            if (linkedLevers[i].IsActivated() != requiredLeverStates[i])
            {
                CloseDoor();
                return;
            }
        }
        OpenDoor();
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            // Reproducir el sonido de apertura
            if (openSoundClip != null)
            {
                audioSource.PlayOneShot(openSoundClip);
            }

            // Detener todas las corutinas anteriores y comenzar a abrir la puerta
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openPosition));
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            // Reproducir el sonido de cierre
            if (closeSoundClip != null)
            {
                audioSource.PlayOneShot(closeSoundClip);
            }

            // Detener todas las corutinas anteriores y comenzar a cerrar la puerta
            StopAllCoroutines();
            StartCoroutine(MoveDoor(closedPosition));
            isOpen = false;
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(doorTransform.position, targetPosition) > 0.01f)
        {
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPosition, doorSpeed * Time.deltaTime);
            yield return null;
        }
        doorTransform.position = targetPosition;
    }
}
