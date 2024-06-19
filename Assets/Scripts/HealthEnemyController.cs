using UnityEngine;
using System.Collections;

public class HealthEnemyController : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima del enemigo
    private int currentHealth; // Salud actual
    private bool isInvulnerable = false; // Estado de invulnerabilidad
    public float invulnerabilityDuration = 2.0f; // Duración de la invulnerabilidad en segundos
    public AudioClip deathSoundClip; // Clip de sonido para la muerte del enemigo
    private AudioSource audioSource; 

    public GameObject objetoADestruir; // Referencia al objeto que se destruirá al morir

    void Start()
    {
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        // Asignar el clip de sonido para la muerte del enemigo
        audioSource.clip = deathSoundClip;
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            Debug.Log("Enemigo golpeado");
            currentHealth -= amount;

            if (deathSoundClip != null)
            {
                audioSource.PlayOneShot(deathSoundClip);
            }
            

            if (currentHealth <= 0)
            {

                
                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(BecomeInvulnerable());
            }


            
        }
    }

    IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    IEnumerator Die()
    {
        
        // Aquí puedes implementar la lógica para la muerte del enemigo
        // Desactivar el enemigo, reproducir animación de muerte, etc.
        Debug.Log(gameObject.name + " has died!");
        if (deathSoundClip != null)
        {
            audioSource.PlayOneShot(deathSoundClip);
        }
        yield return new WaitForSeconds(0.5f);

        // Destruir el objeto específico (ObjetoADestruir) si existe
        if (objetoADestruir != null)
        {
            Destroy(objetoADestruir);
        }
         // Destruir el objeto del enemigo
    }
}
