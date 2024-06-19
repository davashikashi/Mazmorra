using System.Collections;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    public int damageAmount = 10; // Cantidad de daño que inflige al jugador
    public float attackInterval = 3f; // Intervalo entre ataques en segundos

    private Transform player; // Referencia al transform del jugador
    private bool isPlayerDetected = false; // Indica si el jugador está dentro del rango de detección
    private Animator animator; // Referencia al componente Animator

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // El comportamiento de ataque está gestionado por la corutina AttackCycle  
        if (isPlayerDetected)
        {
            RotateTowardsPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador entró en el rango de detección del enemigo
            Debug.Log("Player detected!");
            isPlayerDetected = true;
            StartCoroutine(AttackCycle());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador salió del rango de detección del enemigo
            Debug.Log("Player exited!");
            isPlayerDetected = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verificar si el jugador está en estado de invulnerabilidad
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null && !healthController.IsInvulnerable())
            {
                // El enemigo ha colisionado físicamente con el jugador y puede infligir daño
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
        // Aquí puedes implementar la lógica para infligir daño al jugador
        // Por ejemplo, puedes reducir la salud del jugador
        Debug.Log("Inflicting damage to player!");

        // Ejemplo básico: Reducir la salud del jugador si tiene un script de salud
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
            // Iniciar animación de ataque
            animator.SetTrigger("Attack");

            // Esperar a que la animación de ataque termine
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            animator.SetTrigger("Idle");
            // Esperar el intervalo de tiempo antes del próximo ataque
            yield return new WaitForSeconds(attackInterval);
        }

        // Al salir del ciclo, poner al enemigo en estado idle
        animator.SetTrigger("Idle");
    }
}
