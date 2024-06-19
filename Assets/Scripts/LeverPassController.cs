using UnityEngine;
using TMPro;

public class LeverPassController : MonoBehaviour
{
    public DoorPassController linkedDoor; // Referencia a la puerta asociada a esta palanca
    public TextMeshProUGUI interactionText; // Texto de interacci�n que se muestra cuando el jugador est� cerca
    public Animator animator; // Referencia al Animator de la palanca
    public AudioClip activationSoundClip; // Clip de sonido al activar la palanca
    public AudioClip deactivationSoundClip; // Clip de sonido al desactivar la palanca
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

        // Configurar el AudioSource para los sonidos de activaci�n y desactivaci�n
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // No reproducir autom�ticamente al despertar
        audioSource.clip = activationSoundClip; // Clip por defecto al activar la palanca
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        leverActivated = !leverActivated; // Cambiar el estado de la palanca
        Debug.Log("Lever toggled! New state: " + leverActivated);

        // Reproducir el sonido correspondiente
        if (leverActivated)
        {
            if (activationSoundClip != null)
            {
                audioSource.PlayOneShot(activationSoundClip);
            }
        }
        else
        {
            if (deactivationSoundClip != null)
            {
                audioSource.PlayOneShot(deactivationSoundClip);
            }
        }

        // Reproducir la animaci�n de activaci�n o desactivaci�n
        if (animator != null)
        {
            if (leverActivated)
            {
                animator.SetTrigger("ActivateLever");
            }
            else
            {
                animator.SetTrigger("DeactivateLever");
            }
        }

        // Notificar a la puerta que se ha cambiado el estado de una palanca
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
