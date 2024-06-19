using UnityEngine;
using TMPro;

public class LeverController : MonoBehaviour
{
    public DoorController linkedDoor; // Referencia a la puerta asociada a esta palanca
    public TextMeshProUGUI interactionText; // Texto de interacci�n que se muestra cuando el jugador est� cerca
    public Animator animator; // Referencia al Animator de la palanca
    public AudioClip activationSoundClip; // Clip de sonido al activar la palanca
    private AudioSource audioSource; // Referencia al componente AudioSource

    private bool leverActivated = false; // Estado actual de la palanca
    private bool playerInRange = false; // Si el jugador est� dentro del �rea de activaci�n

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Aseg�rate de que el texto de interacci�n est� oculto al inicio
        }

        // Asegurarse de que la puerta conoce esta palanca
        if (linkedDoor != null && !linkedDoor.linkedLevers.Contains(this))
        {
            linkedDoor.linkedLevers.Add(this);
        }

        // Asegurarse de que el Animator est� asignado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Configurar el AudioSource para el sonido de activaci�n
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = activationSoundClip;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !leverActivated)
        {
            playerInRange = true;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true); // Mostrar el texto de interacci�n
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Ocultar el texto de interacci�n
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !leverActivated)
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        leverActivated = true;
        Debug.Log("Lever activated!");

        // Reproducir la animaci�n de activaci�n
        if (animator != null)
        {
            animator.SetTrigger("ActivateLever");
        }

        // Reproducir el sonido de activaci�n
        if (audioSource != null && activationSoundClip != null)
        {
            audioSource.PlayOneShot(activationSoundClip);
        }

        // Notificar a la puerta que se ha activado una palanca
        if (linkedDoor != null)
        {
            linkedDoor.CheckLevers();
        }

        // Ocultar el texto de interacci�n una vez activado
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    public bool IsActivated()
    {
        return leverActivated;
    }
}
