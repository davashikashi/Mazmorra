using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public int damageAmount = 30; // Cantidad de daño que inflige el fuego
    public float attackInterval = 5f; // Intervalo entre ataques
    public ParticleSystem fireEffect; // Sistema de partículas para el fuego
    public GameObject Player;

    private Animator animator; // Referencia al componente Animator
    private bool isPlayerInRange = false; // Indica si el jugador está en el área de daño

    public AudioClip FireClip;
    private AudioSource FireSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        fireEffect.Stop();
        StartCoroutine(AttackCycle()); // Iniciar el ciclo de ataque al comienzo

        FireSource = GetComponent<AudioSource>();
        FireSource.clip = FireClip;
    }

    void Update()
    {
        // Aquí puedes añadir cualquier lógica adicional necesaria en el Update
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered!");
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited!");
            isPlayerInRange = false;
        }
    }

    IEnumerator AttackCycle()
    {
        while (true) // Bucle infinito para asegurarse de que el intervalo se ejecute siempre
        {

            // Activar el efecto de fuego
            fireEffect.Play();

            if(FireSource != null) 
            {
                FireSource.Play();
            }

            // Infligir daño al jugador si está en rango
            if (isPlayerInRange)
            {
                InflictDamage(Player);
            }

            // Mantener el fuego activo por la duración de la animación de ataque
            yield return new WaitForSeconds(fireEffect.main.duration); // Suponiendo que la duración del fuego es igual a la duración de las partículas

            // Detener el efecto de fuego para la siguiente iteración
            fireEffect.Stop();

            // Esperar el intervalo de ataque
            yield return new WaitForSeconds(attackInterval);
        }
    }

    void InflictDamage(GameObject playerObj)
    {
        // Aquí puedes implementar la lógica para infligir daño al jugador
        // Por ejemplo, puedes reducir la salud del jugador
        Debug.Log("Inflicting damage to player!");

        // Ejemplo básico: Reducir la salud del jugador si tiene un script de salud
        HealthController healthController = playerObj.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount);
        }
    }
}
