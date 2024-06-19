using UnityEngine;

public class FireController : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo
    public float stoppingDistance = 2f; // Distancia a la cual el enemigo se detiene frente al jugador
    public int damageAmount = 10; // Cantidad de da�o que inflige al jugador

    private Transform player; // Referencia al transform del jugador
    private bool isPlayerDetected = false; // Indica si el jugador est� dentro del rango de detecci�n

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encontrar al jugador por su tag "Player"
    }

    void Update()
    {
        if (isPlayerDetected)
        {
            // Calcular la direcci�n hacia donde debe moverse el enemigo
            Vector3 moveDirection = player.position - transform.position;
            moveDirection.y = 0; // Mantener el movimiento en el plano horizontal

            // Si el jugador est� dentro del rango de seguimiento
            if (moveDirection.magnitude <= stoppingDistance)
            {
                // Detener al enemigo
                // Puedes a�adir aqu� l�gica adicional, como atacar al jugador
            }
            else
            {
                // Mover al enemigo hacia el jugador
                transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);

                // Rotar al enemigo hacia la direcci�n del movimiento
                Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador entr� en el rango de detecci�n del enemigo
            Debug.Log("Player detected!");
            isPlayerDetected = true;
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

    void InflictDamage(GameObject playerObj)
    {
        // Aqu� puedes implementar la l�gica para infligir da�o al jugador
        // Por ejemplo, puedes reducir la salud del jugador
        Debug.Log("Inflicting damage to player!");

        // Ejemplo b�sico: Reducir la salud del jugador si tiene un script de salud
        HealthController healthController = playerObj.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount);
        }
    }
}
