using System.Collections;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    public int damageAmount = 10; // Cantidad de da�o que inflige al jugador
    public float attackInterval = 3f; // Intervalo entre ataques en segundos

    private Transform player; // Referencia al transform del jugador
    private bool isPlayerDetected = false; // Indica si el jugador est� dentro del rango de detecci�n
    private Animator animator; // Referencia al componente Animator

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // El comportamiento de ataque est� gestionado por la corutina AttackCycle  
        if (isPlayerDetected)
        {
            RotateTowardsPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador entr� en el rango de detecci�n del enemigo
            Debug.Log("Player detected!");
            isPlayerDetected = true;
            StartCoroutine(AttackCycle());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador sali� del rango de detecci�n del enemigo
            Debug.Log("Player exited!");
            isPlayerDetected = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verificar si el jugador est� en estado de invulnerabilidad
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null && !healthController.IsInvulnerable())
            {
                // El enemigo ha colisionado f�sicamente con el jugador y puede infligir da�o
                InflictDamage(collision.gameObject);
            }
        }
    }
    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void InflictDamage(GameObject playerObj)
    {
        // Aqu� puedes implementar la l�gica para infligir da�o al jugador
        // Por ejemplo, puedes reducir la salud del jugador
        Debug.Log("Inflicting damage to player!");

        // Ejemplo b�sico: Reducir la salud del jugador si tiene un script de salud
        HealthController healthController = playerObj.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(10);
        }
    }

    IEnumerator AttackCycle()
    {
        while (isPlayerDetected)
        {
            // Iniciar animaci�n de ataque
            animator.SetTrigger("Attack");

            // Esperar a que la animaci�n de ataque termine
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            animator.SetTrigger("Idle");
            // Esperar el intervalo de tiempo antes del pr�ximo ataque
            yield return new WaitForSeconds(attackInterval);
        }

        // Al salir del ciclo, poner al enemigo en estado idle
        animator.SetTrigger("Idle");
    }
}
